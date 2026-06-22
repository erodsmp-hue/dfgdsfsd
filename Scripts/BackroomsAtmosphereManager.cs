using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BackroomsAtmosphereManager : MonoBehaviour
{
    [Header("Settings")]
    public Color ambientColor = new Color(0.02f, 0.02f, 0.03f);
    public float fogDensity = 0.04f;
    public float exposure = -0.7f;

    private void Awake()
    {
        // 1. Force Fog
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = fogDensity;
        
        // 2. Force Ambient Lighting
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = ambientColor;
        RenderSettings.ambientIntensity = 0.1f;

        // 3. Optional: Dynamic Exposure Adjustment (If you have a global volume)
        Volume vol = GetComponent<Volume>();
        if (vol != null && vol.profile.TryGet(out ColorAdjustments colorAdjust))
        {
            colorAdjust.postExposure.value = exposure;
        }
        
        Debug.Log("[Atmosphere] Lighting and Fog forced to Horror settings.");
    }
}