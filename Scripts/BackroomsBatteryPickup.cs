using UnityEngine;

public class BackroomsBatteryPickup : MonoBehaviour
{
    // The player's raycast will call this function
    public void OnInteract(BatteryInventoryCanvasUIBackroomsCleanCentered inventory)
    {
        if (inventory != null)
        {
            // 1. Add to the inventory
            inventory.AddBattery();

            // 2. Play a pickup sound (Optional, but highly recommended)
            // AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            // 3. Destroy the battery from the world
            Destroy(gameObject);
        }
    }
}