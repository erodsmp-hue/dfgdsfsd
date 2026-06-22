using UnityEngine;

public class BackroomsFlashlightDynamic : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FlashlightBatterySystem batterySystem;
    [SerializeField] private Light flashlightBeam;

    [Header("Decay Settings")]
    [SerializeField] private float maxIntensity = 2.5f;
    [SerializeField] private float maxRange = 15f;
    [SerializeField] private float criticalThreshold = 0.2f; // 20%

    private float baseIntensity;
    private float baseRange;

    private void Awake()
    {
        if (batterySystem == null) batterySystem = FindFirstObjectByType<FlashlightBatterySystem>(FindObjectsInactive.Include);
        if (flashlightBeam == null) flashlightBeam = GetComponentInChildren<Light>();
        
        baseIntensity = maxIntensity;
        baseRange = maxRange;
    }

    private void Update()
    {
        if (batterySystem == null || flashlightBeam == null) return;

        float percent = batterySystem.GetBatteryPercent() / 100f;

        // 1. Smoothly decay intensity and range
        flashlightBeam.intensity = Mathf.Lerp(0f, baseIntensity, percent);
        flashlightBeam.range = Mathf.Lerp(5f, baseRange, percent);

        // 2. Add an anxious flicker when battery is critical
        if (percent < criticalThreshold && percent > 0.05f)
        {
            float flicker = Mathf.PerlinNoise(Time.time * 20f, 0f);
            flashlightBeam.intensity *= (flicker > 0.7f ? 1f : 0.4f);
        }
        else if (percent <= 0.05f)
        {
            // Aggressive stutter when nearly dead
            flashlightBeam.enabled = Random.value > 0.2f;
        }
    }
}