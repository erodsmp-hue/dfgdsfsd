using UnityEngine;
using UnityEngine.UI;

public class BackroomsStaminaUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BackroomsPlayerVitals vitals;
    [SerializeField] private CanvasGroup uiGroup;
    [SerializeField] private RectTransform staminaFillRect; // Changed from Image to RectTransform to bypass the Sprite bug

    [Header("VHS Visual Settings")]
    [SerializeField] private Color highStaminaColor = new Color(0.2f, 1f, 0.2f, 0.65f); // Camcorder Green
    [SerializeField] private Color lowStaminaColor = new Color(1f, 0.1f, 0.1f, 0.85f);  // Warning Red
    [SerializeField] private float fadeSpeed = 8f;

    private Image fillImage;
    private float currentDisplayStamina = 1f;

    private void Start()
    {
        if (uiGroup != null) uiGroup.alpha = 0f;
        if (staminaFillRect != null) fillImage = staminaFillRect.GetComponent<Image>();
    }

    private void Update()
    {
        if (vitals == null || uiGroup == null || staminaFillRect == null || fillImage == null) return;

        // 1. Smoothly interpolate the visual bar for a slightly "laggy" analog tape feel
        currentDisplayStamina = Mathf.Lerp(currentDisplayStamina, vitals.Stamina01, Time.deltaTime * 12f);

        // FIX: Manipulating the Anchor perfectly scales the UI without needing a Source Sprite!
        staminaFillRect.anchorMax = new Vector2(currentDisplayStamina, 1f);

        // 2. Analog Color Shifting & Flicker
        Color targetColor = Color.Lerp(lowStaminaColor, highStaminaColor, currentDisplayStamina);

        if (vitals.IsExhausted)
        {
            // Aggressive, glitchy flashing when dead tired (locks out sprinting)
            float glitchAlpha = Random.value > 0.5f ? 0.2f : 0.9f;
            fillImage.color = new Color(lowStaminaColor.r, lowStaminaColor.g, lowStaminaColor.b, glitchAlpha);
        }
        else
        {
            // Subtle CRT screen hum/flicker while running normally
            float crtHum = 0.85f + Mathf.Sin(Time.time * 25f) * 0.15f;
            fillImage.color = new Color(targetColor.r, targetColor.g, targetColor.b, targetColor.a * crtHum);
        }

        // 3. Visibility Logic (Only visible when draining or exhausted)
        bool isMissingStamina = vitals.Stamina01 < 0.98f;
        float targetAlpha = (isMissingStamina || vitals.IsExhausted) ? 1f : 0f;
        uiGroup.alpha = Mathf.Lerp(uiGroup.alpha, targetAlpha, Time.deltaTime * fadeSpeed);
    }
}