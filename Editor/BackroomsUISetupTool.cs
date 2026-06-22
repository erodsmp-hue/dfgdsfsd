#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using TMPro;

public class BackroomsUISetupTool : EditorWindow
{
    // This creates a new button at the very top of your Unity Editor
    [MenuItem("Backrooms Tools/Create Interact Prompt UI")]
    public static void CreateInteractPrompt()
    {
        // 1. Find your existing inventory script in the scene (Updated for modern Unity)
        var inventoryScript = FindFirstObjectByType<BatteryInventoryCanvasUIBackroomsCleanCentered>();
        
        if (inventoryScript == null)
        {
            Debug.LogError("Tool failed: Could not find 'BatteryInventoryCanvasUIBackroomsCleanCentered' in the scene. Make sure it is attached to your Canvas!");
            return;
        }

        // 2. Check if one already exists to prevent duplicates
        if (inventoryScript.interactPrompt != null)
        {
            Debug.LogWarning("An Interact Prompt is already assigned to your script!");
            return;
        }

        // 3. Create the new UI object
        GameObject promptObj = new GameObject("InteractPrompt_UI");
        
        // 4. Make it a child of the Canvas/Inventory object
        promptObj.transform.SetParent(inventoryScript.transform, false);

        // 5. Setup RectTransform to center it on the screen
        RectTransform rect = promptObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f); // Dead center
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(0f, -40f); // Slightly below dead center so it doesn't block the object
        rect.sizeDelta = new Vector2(400f, 50f);

        // 6. Add TextMeshPro and style it
        TextMeshProUGUI tmpText = promptObj.AddComponent<TextMeshProUGUI>();
        tmpText.text = "[E] Pick Up";
        tmpText.alignment = TextAlignmentOptions.Center;
        tmpText.fontSize = 24;
        
        // VHS Style Color: A slightly faded, ghostly yellowish-white
        tmpText.color = new Color(0.9f, 0.9f, 0.8f, 0.7f); 
        
        // Add a subtle shadow/boldness for readability
        tmpText.fontStyle = FontStyles.Bold;

        // 7. Auto-assign the new object to your script!
        inventoryScript.interactPrompt = promptObj;

        // 8. Hide it by default
        promptObj.SetActive(false);

        // 9. Register undo and save the scene state
        Undo.RegisterCreatedObjectUndo(promptObj, "Create Interact Prompt");
        EditorUtility.SetDirty(inventoryScript);

        Debug.Log("Success! '[E] Pick Up' UI has been generated, styled, and linked.");
        
        // Select the new object in the hierarchy so you can see it
        Selection.activeGameObject = promptObj;
    }
}
#endif