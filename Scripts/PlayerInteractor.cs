using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Settings")]
    public float interactRange = 3f;
    public KeyCode interactKey = KeyCode.E;

    [Header("References")]
    public BatteryInventoryCanvasUIBackroomsCleanCentered inventoryUI;

    // We store the battery we are currently looking at
    private BackroomsBatteryPickup currentTarget;

    private void Update()
    {
        HandleRaycast();
        HandleInteraction();
    }

    private void HandleRaycast()
    {
        RaycastHit hit;
        
        // Shoot a raycast from the center of the camera
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactRange))
        {
            BackroomsBatteryPickup battery = hit.collider.GetComponent<BackroomsBatteryPickup>();
            
            if (battery != null)
            {
                // We are looking at a battery
                currentTarget = battery;
                if (inventoryUI != null) inventoryUI.ShowInteractPrompt(true); // Turn ON the UI text
                return; // Stop here so we don't turn it off below
            }
        }

        // If the raycast hit nothing, or hit a wall, turn off the prompt
        currentTarget = null;
        if (inventoryUI != null) inventoryUI.ShowInteractPrompt(false); 
    }

    private void HandleInteraction()
    {
        // Only trigger if we press E AND we have a valid target
        if (Input.GetKeyDown(interactKey) && currentTarget != null)
        {
            currentTarget.OnInteract(inventoryUI);
            
            // Turn off the prompt immediately after picking it up
            currentTarget = null;
            if (inventoryUI != null) inventoryUI.ShowInteractPrompt(false); 
        }
    }
}