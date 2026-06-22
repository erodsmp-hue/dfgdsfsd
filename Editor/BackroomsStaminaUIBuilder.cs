#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class BackroomsStaminaUIBuilder
{
    [MenuItem("Backrooms/Tools/Generate Stamina UI")]
    public static void CreateStaminaUI()
    {
        // 1. Find or Create a Canvas
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("HUD_Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        // 2. Create the Main UI Group
        GameObject groupObj = new GameObject("StaminaUI_Group");
        groupObj.transform.SetParent(canvas.transform, false);
        
        RectTransform groupRect = groupObj.AddComponent<RectTransform>();
        groupRect.anchorMin = new Vector2(0.5f, 0f); // Bottom Center Alignment
        groupRect.anchorMax = new Vector2(0.5f, 0f);
        groupRect.pivot = new Vector2(0.5f, 0f);
        groupRect.sizeDelta = new Vector2(250f, 4f); // Thin minimalist line
        groupRect.anchoredPosition = new Vector2(0f, 40f); // Sit 40 pixels off the bottom

        CanvasGroup canvasGroup = groupObj.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0f; // Start hidden

        BackroomsStaminaUI uiScript = groupObj.AddComponent<BackroomsStaminaUI>();

        // 3. Create the Dark Background
        GameObject bgObj = new GameObject("Background");
        bgObj.transform.SetParent(groupObj.transform, false);
        RectTransform bgRect = bgObj.AddComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero; // Stretch
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = Vector2.zero;
        
        Image bgImage = bgObj.AddComponent<Image>();
        bgImage.color = new Color(0f, 0f, 0f, 0.35f); // Dark translucent

        // 4. Create the White Fill Bar
        GameObject fillObj = new GameObject("Fill");
        fillObj.transform.SetParent(groupObj.transform, false);
        RectTransform fillRect = fillObj.AddComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero; // Stretch
        fillRect.anchorMax = Vector2.one;
        fillRect.sizeDelta = Vector2.zero;

        Image fillImage = fillObj.AddComponent<Image>();
        fillImage.color = Color.white;
        fillImage.type = Image.Type.Filled;
        fillImage.fillMethod = Image.FillMethod.Horizontal;
        fillImage.fillOrigin = (int)Image.OriginHorizontal.Left;

        // 5. Automatically Link the Script References using SerializedObject
        SerializedObject serializedObject = new SerializedObject(uiScript);
        serializedObject.FindProperty("staminaFill").objectReferenceValue = fillImage;
        serializedObject.FindProperty("uiGroup").objectReferenceValue = canvasGroup;

        // 6. Attempt to auto-find the Player Vitals in the scene
        BackroomsPlayerVitals vitals = Object.FindFirstObjectByType<BackroomsPlayerVitals>();
        if (vitals != null)
        {
            serializedObject.FindProperty("vitals").objectReferenceValue = vitals;
        }

        serializedObject.ApplyModifiedProperties();

        // 7. Focus on it in the editor so you can see it
        Selection.activeGameObject = groupObj;
        Debug.Log("<b>[Backrooms HUD]</b> Minimal Stamina UI generated successfully!");
    }
}
#endif