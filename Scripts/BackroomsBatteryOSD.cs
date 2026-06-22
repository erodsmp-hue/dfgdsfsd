using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BackroomsBatteryOSD : MonoBehaviour
{
    [Header("System References")]
    [SerializeField] private FlashlightBatterySystem batterySystem;
    [SerializeField] private BatteryInventory inventory;

    [Header("Main UI References")]
    [SerializeField] private TextMeshProUGUI batteryLevelText;
    [SerializeField] private TextMeshProUGUI spareBatteriesText;

    [Header("VHS Ghosting Layers (Chromatic Aberration)")]
    [SerializeField] private TextMeshProUGUI batteryGhostRed;
    [SerializeField] private TextMeshProUGUI batteryGhostBlue;
    [SerializeField] private TextMeshProUGUI spareGhostRed;
    [SerializeField] private TextMeshProUGUI spareGhostBlue;

    [Header("VHS Styling")]
    [SerializeField] private Color normalColor = new Color(0.9f, 0.9f, 0.9f, 0.95f);
    [SerializeField] private Color warningColor = new Color(0.95f, 0.1f, 0.1f, 0.95f);
    [SerializeField] private float blinkSpeed = 6f;

    private void Awake()
    {
        if (batterySystem == null) batterySystem = FindFirstObjectByType<FlashlightBatterySystem>(FindObjectsInactive.Include);
        if (inventory == null) inventory = FindFirstObjectByType<BatteryInventory>(FindObjectsInactive.Include);
    }

    private void Update()
    {
        HandleInput();
        UpdateBatteryLevel();
        UpdateInventoryCount();
    }

    private void HandleInput()
    {
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            if (inventory != null && inventory.StoredBatteries > 0)
                inventory.TryUseStoredBattery();
        }
    }

    private void UpdateBatteryLevel()
    {
        if (batterySystem == null || batteryLevelText == null) return;

        float percent = batterySystem.GetBatteryPercent();
        int exactPercent = Mathf.Clamp(Mathf.RoundToInt(percent), 0, 100);

        // Added Character Spacing tag for that chunky Monospace feel
        string battString = "<cspace=0.1em>BATT ";
        if (percent > 75f) battString += "[███] ";
        else if (percent > 40f) battString += "[██_] ";
        else if (percent > 15f) battString += "[█__] ";
        else battString += "[___] ";

        battString += exactPercent.ToString("D2") + "%</cspace>";
        
        // Sync main text and ghosts
        batteryLevelText.text = battString;
        if (batteryGhostRed != null) batteryGhostRed.text = battString;
        if (batteryGhostBlue != null) batteryGhostBlue.text = battString;

        if (percent <= 15f)
        {
            float alpha = 0.2f + Mathf.PingPong(Time.time * blinkSpeed, 0.8f);
            Color flash = new Color(warningColor.r, warningColor.g, warningColor.b, alpha);
            batteryLevelText.color = flash;
            
            // Intensify the glitch separation when flashing red
            if (batteryGhostRed != null) batteryGhostRed.color = new Color(1f, 0f, 0f, alpha * 0.5f);
            if (batteryGhostBlue != null) batteryGhostBlue.color = new Color(0f, 0f, 1f, alpha * 0.5f);
        }
        else
        {
            batteryLevelText.color = normalColor;
            
            // Standard subtle VHS color bleed
            float hum = 0.3f + Mathf.Sin(Time.time * 15f) * 0.1f; // Slight analog hum
            if (batteryGhostRed != null) batteryGhostRed.color = new Color(1f, 0f, 0f, hum);
            if (batteryGhostBlue != null) batteryGhostBlue.color = new Color(0f, 0.5f, 1f, hum);
        }
    }

    private void UpdateInventoryCount()
    {
        if (inventory == null || spareBatteriesText == null) return;

        string spareText = inventory.StoredBatteries > 0 ? $"<cspace=0.1em>SPARE: {inventory.StoredBatteries}</cspace>" : "<cspace=0.1em>NO SPARE</cspace>";
        
        spareBatteriesText.text = spareText;
        if (spareGhostRed != null) spareGhostRed.text = spareText;
        if (spareGhostBlue != null) spareGhostBlue.text = spareText;

        if (inventory.StoredBatteries > 0)
        {
            spareBatteriesText.color = normalColor;
            float hum = 0.3f + Mathf.Sin(Time.time * 15f) * 0.1f;
            if (spareGhostRed != null) spareGhostRed.color = new Color(1f, 0f, 0f, hum);
            if (spareGhostBlue != null) spareGhostBlue.color = new Color(0f, 0.5f, 1f, hum);
        }
        else
        {
            float alpha = 0.5f + Mathf.PingPong(Time.time * 2f, 0.5f);
            spareBatteriesText.color = new Color(warningColor.r, warningColor.g, warningColor.b, alpha);
            if (spareGhostRed != null) spareGhostRed.color = new Color(1f, 0f, 0f, alpha * 0.5f);
            if (spareGhostBlue != null) spareGhostBlue.color = new Color(0f, 0f, 1f, alpha * 0.5f);
        }
    }
}