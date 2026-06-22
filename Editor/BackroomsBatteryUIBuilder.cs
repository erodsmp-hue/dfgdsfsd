#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;

public class BackroomsHUDBuilder
{
    [MenuItem("Backrooms/Tools/Generate Full VHS HUD (Pass 3 - Fixed)")]
    public static void CreateFullHUD()
    {
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("HUD_Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            GameObject glass = new GameObject("Camcorder_GlassTint");
            glass.transform.SetParent(canvas.transform, false);
            var glassRect = glass.AddComponent<RectTransform>();
            glassRect.anchorMin = Vector2.zero; glassRect.anchorMax = Vector2.one;
            glassRect.sizeDelta = Vector2.zero;
            var img = glass.AddComponent<Image>();
            img.color = new Color(0.02f, 0.03f, 0.05f, 0.15f);
            img.raycastTarget = false;
        }

        GameObject groupObj = new GameObject("VHS_Full_OSD_TMP");
        groupObj.transform.SetParent(canvas.transform, false);
        RectTransform groupRect = groupObj.AddComponent<RectTransform>();
        groupRect.anchorMin = Vector2.zero; groupRect.anchorMax = Vector2.one;
        groupRect.sizeDelta = Vector2.zero;

        CanvasGroup cg = groupObj.AddComponent<CanvasGroup>();
        BackroomsVHSTracking trackingScript = groupObj.AddComponent<BackroomsVHSTracking>();

        // --- 1. BATTERY SYSTEM ---
        GameObject battGroup = new GameObject("Battery_Group");
        battGroup.transform.SetParent(groupObj.transform, false);
        SetupRect(battGroup, new Vector2(1, 1), new Vector2(1, 1), new Vector2(1, 1), new Vector2(-50, -40), new Vector2(400, 100));
        BackroomsBatteryOSD battScript = battGroup.AddComponent<BackroomsBatteryOSD>();
        TextMeshProUGUI[] bLvl = CreateStack("BattLvl", battGroup.transform, new Vector2(0, 0.5f), new Vector2(1, 1), 32, TextAlignmentOptions.TopRight, true);
        TextMeshProUGUI[] bSpr = CreateStack("BattSpr", battGroup.transform, new Vector2(0, 0), new Vector2(1, 0.5f), 24, TextAlignmentOptions.BottomRight, true);
        SerializedObject sBatt = new SerializedObject(battScript);
        sBatt.FindProperty("batteryLevelText").objectReferenceValue = bLvl[0]; sBatt.FindProperty("batteryGhostRed").objectReferenceValue = bLvl[1]; sBatt.FindProperty("batteryGhostBlue").objectReferenceValue = bLvl[2];
        sBatt.FindProperty("spareBatteriesText").objectReferenceValue = bSpr[0]; sBatt.FindProperty("spareGhostRed").objectReferenceValue = bSpr[1]; sBatt.FindProperty("spareGhostBlue").objectReferenceValue = bSpr[2];
        sBatt.ApplyModifiedProperties();

        // --- 2. RECORDING SYSTEM ---
        BackroomsRecordingOSD recScript = groupObj.AddComponent<BackroomsRecordingOSD>();
        GameObject topL = new GameObject("Rec_Group");
        topL.transform.SetParent(groupObj.transform, false);
        SetupRect(topL, new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 1), new Vector2(50, -40), new Vector2(400, 50));
        TextMeshProUGUI[] recStack = CreateStack("RecText", topL.transform, Vector2.zero, Vector2.one, 32, TextAlignmentOptions.TopLeft, true);

        GameObject botL = new GameObject("Time_Group");
        botL.transform.SetParent(groupObj.transform, false);
        SetupRect(botL, new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(50, 40), new Vector2(400, 100));
        TextMeshProUGUI[] timeStack = CreateStack("TimeText", botL.transform, new Vector2(0, 0), new Vector2(1, 0.4f), 32, TextAlignmentOptions.BottomLeft, true);
        TextMeshProUGUI[] dateStack = CreateStack("DateText", botL.transform, new Vector2(0, 0.4f), new Vector2(1, 1), 24, TextAlignmentOptions.TopLeft, true);

        SerializedObject sRec = new SerializedObject(recScript);
        sRec.FindProperty("recText").objectReferenceValue = recStack[0]; sRec.FindProperty("recRed").objectReferenceValue = recStack[1]; sRec.FindProperty("recBlue").objectReferenceValue = recStack[2];
        sRec.FindProperty("timecodeText").objectReferenceValue = timeStack[0]; sRec.FindProperty("timeRed").objectReferenceValue = timeStack[1]; sRec.FindProperty("timeBlue").objectReferenceValue = timeStack[2];
        sRec.FindProperty("dateText").objectReferenceValue = dateStack[0]; sRec.FindProperty("dateRed").objectReferenceValue = dateStack[1]; sRec.FindProperty("dateBlue").objectReferenceValue = dateStack[2];
        sRec.ApplyModifiedProperties();

        // --- 3. REDESIGNED TRACKING ERROR UI ---
        GameObject trackingBgObj = new GameObject("Tracking_Background");
        trackingBgObj.transform.SetParent(groupObj.transform, false);
        SetupRect(trackingBgObj, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0, 120), new Vector2(450, 60));
        
        Image trkBgImg = trackingBgObj.AddComponent<Image>();
        trkBgImg.color = new Color(0f, 0f, 0f, 0.7f); // Dark VCR Block
        CanvasGroup trkBgCg = trackingBgObj.AddComponent<CanvasGroup>();

        GameObject trackingTextObj = new GameObject("Tracking_Text");
        trackingTextObj.transform.SetParent(trackingBgObj.transform, false);
        SetupRect(trackingTextObj, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
        
        TextMeshProUGUI trkText = trackingTextObj.AddComponent<TextMeshProUGUI>();
        trkText.text = "TRACKING  [███_______]";
        trkText.fontSize = 28;
        trkText.alignment = TextAlignmentOptions.Center;
        trkText.fontStyle = FontStyles.Bold;
        trkText.color = new Color(0.2f, 1f, 0.2f, 0.9f); // Classic VCR Phosphor Green
        
        SerializedObject sTrk = new SerializedObject(trackingScript);
        sTrk.FindProperty("rootUI").objectReferenceValue = groupRect;
        sTrk.FindProperty("trackingText").objectReferenceValue = trkText;
        sTrk.FindProperty("trackingBackground").objectReferenceValue = trkBgCg;
        sTrk.ApplyModifiedProperties();

        Selection.activeGameObject = groupObj;
        Debug.Log("<b>[Backrooms HUD]</b> Pass 3 Fixed: Smoother flutter and authentic VCR tracking bar generated!");
    }

    private static void SetupRect(GameObject go, Vector2 aMin, Vector2 aMax, Vector2 pivot, Vector2 pos, Vector2 size)
    {
        RectTransform r = go.AddComponent<RectTransform>();
        r.anchorMin = aMin; r.anchorMax = aMax; r.pivot = pivot;
        r.anchoredPosition = pos; r.sizeDelta = size;
    }

    private static TextMeshProUGUI[] CreateStack(string name, Transform parent, Vector2 aMin, Vector2 aMax, int size, TextAlignmentOptions align, bool dropShadow)
    {
        TextMeshProUGUI red = CreateTextObj(name + "_Red", parent, aMin, aMax, size, align, new Vector2(-2.5f, 0f));
        TextMeshProUGUI blue = CreateTextObj(name + "_Blue", parent, aMin, aMax, size, align, new Vector2(2.5f, -1f));
        TextMeshProUGUI main = CreateTextObj(name + "_Main", parent, aMin, aMax, size, align, Vector2.zero);
        if (dropShadow) {
            main.fontMaterial.EnableKeyword("UNDERLAY_ON");
            main.fontMaterial.SetFloat("_UnderlayOffsetX", 1f); main.fontMaterial.SetFloat("_UnderlayOffsetY", -1f);
            main.fontMaterial.SetFloat("_UnderlayDilate", 0.5f); main.fontMaterial.SetColor("_UnderlayColor", new Color(0, 0, 0, 0.8f));
        }
        return new TextMeshProUGUI[] { main, red, blue };
    }

    private static TextMeshProUGUI CreateTextObj(string name, Transform parent, Vector2 aMin, Vector2 aMax, int size, TextAlignmentOptions align, Vector2 offset)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);
        RectTransform r = obj.AddComponent<RectTransform>();
        r.anchorMin = aMin; r.anchorMax = aMax; r.sizeDelta = Vector2.zero; r.anchoredPosition = offset;
        TextMeshProUGUI tmp = obj.AddComponent<TextMeshProUGUI>();
        tmp.fontSize = size; tmp.alignment = align; tmp.enableWordWrapping = false; tmp.fontStyle = FontStyles.Bold;
        return tmp;
    }
}
#endif