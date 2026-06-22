using UnityEngine;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class BackroomsVHSTracking : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform rootUI;
    [SerializeField] private TextMeshProUGUI trackingText;
    [SerializeField] private CanvasGroup trackingBackground; // Added a dark background block for the text
    
    [Header("Analog Flutter (Smooth Jitter)")]
    [SerializeField] private float flutterSpeed = 15f;
    [SerializeField] private float flutterAmount = 1.5f;

    [Header("Tracking Loss Settings")]
    [SerializeField] private float trackingLossChance = 0.0015f; // Extremely rare
    [SerializeField] private float maxTearX = 8f;

    private CanvasGroup canvasGroup;
    private Vector2 originalPos;
    private float trackingLossTimer;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (rootUI != null) originalPos = rootUI.anchoredPosition;
        if (trackingText != null) trackingText.enabled = false;
        if (trackingBackground != null) trackingBackground.alpha = 0f;
    }

    private void Update()
    {
        if (rootUI == null) return;

        // 1. Smooth Analog Flutter using Perlin Noise (No more harsh digital shaking)
        float noiseY = Mathf.PerlinNoise(Time.time * flutterSpeed, 0f) * 2f - 1f; // Returns -1 to 1 smoothly
        float currentShakeY = noiseY * flutterAmount;

        // 2. Trigger Random Major Tracking Loss
        if (trackingLossTimer <= 0f && Random.value < trackingLossChance)
        {
            trackingLossTimer = Random.Range(0.8f, 1.8f); // Lasts ~1 second
        }

        // 3. Handle Active Tracking Loss
        if (trackingLossTimer > 0f)
        {
            trackingLossTimer -= Time.deltaTime;
            
            // Smooth horizontal tearing using noise
            float noiseX = Mathf.PerlinNoise(0f, Time.time * 25f) * 2f - 1f;
            rootUI.anchoredPosition = new Vector2(originalPos.x + (noiseX * maxTearX), originalPos.y + currentShakeY);
            
            // Drop opacity randomly but smoothly
            canvasGroup.alpha = Mathf.Lerp(0.6f, 0.95f, Mathf.PerlinNoise(Time.time * 30f, 10f));

            // Show the Tracking UI
            if (trackingText != null && trackingBackground != null)
            {
                trackingText.enabled = true;
                trackingBackground.alpha = 1f;

                // Authentic VCR filling bar animation: [||||||    ]
                int totalBars = 10;
                int activeBars = Mathf.FloorToInt(Mathf.PingPong(Time.time * 15f, totalBars));
                string bars = new string('█', activeBars) + new string('_', totalBars - activeBars);
                trackingText.text = $"<cspace=0.1em>TRACKING  [{bars}]</cspace>";
            }
        }
        else
        {
            // Normal Stable State (Just the subtle vertical flutter)
            rootUI.anchoredPosition = new Vector2(originalPos.x, originalPos.y + currentShakeY);
            canvasGroup.alpha = 1f;
            
            if (trackingText != null) trackingText.enabled = false;
            if (trackingBackground != null) trackingBackground.alpha = 0f;
        }
    }
}