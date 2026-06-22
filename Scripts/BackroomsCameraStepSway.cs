using UnityEngine;

public class BackroomsCameraStepSway : MonoBehaviour
{
    [SerializeField] private BackroomsPlayer player;
    [SerializeField] private float swayIntensity = 0.05f;
    [SerializeField] private float landingImpactStrength = 0.3f;
    [SerializeField] private float recoverySpeed = 5f;

    private Vector3 originalLocalPos;
    private float landingRecovery;

    private void Start()
    {
        originalLocalPos = transform.localPosition;
    }

    private void Update()
    {
        if (player == null) return;

        // 1. Procedural Sway based on movement
        float swayX = Mathf.Sin(Time.time * 4f) * (player.MoveAmount01 * swayIntensity);
        float swayY = Mathf.Cos(Time.time * 8f) * (player.MoveAmount01 * swayIntensity);

        // 2. Landing Impact Jolt (The "Heavy" feeling)
        // We use the player's landingImpact value to pull the camera down suddenly
        float impactOffset = player.LandingImpact01 * landingImpactStrength;
        
        // Combine the sway and the impact jolt
        Vector3 targetPos = originalLocalPos + new Vector3(swayX, swayY - impactOffset, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * recoverySpeed);
    }
}