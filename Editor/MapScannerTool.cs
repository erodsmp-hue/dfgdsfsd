#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

public class MapScannerTool : EditorWindow
{
    private GameObject mapRoot;

    [MenuItem("Backrooms/Tools/Map Scanner")]
    public static void ShowWindow() => GetWindow<MapScannerTool>("Map Scanner");

    private void OnGUI()
    {
        GUILayout.Label("Map Coordinate Scanner", EditorStyles.boldLabel);
        mapRoot = (GameObject)EditorGUILayout.ObjectField("Select Map Root Object", mapRoot, typeof(GameObject), true);

        if (GUILayout.Button("Scan & Export Coordinates"))
        {
            if (mapRoot == null)
            {
                Debug.LogError("Please assign the root object containing your map parts.");
                return;
            }
            ScanMap();
        }
    }

    private void ScanMap()
    {
        StringBuilder csv = new StringBuilder();
        csv.AppendLine("ObjectName, X, Y, Z");

        Transform[] allChildren = mapRoot.GetComponentsInChildren<Transform>();

        foreach (Transform child in allChildren)
        {
            // Ignore the root itself
            if (child.gameObject == mapRoot) continue;

            Vector3 pos = child.position;
            csv.AppendLine($"{child.name}, {pos.x:F2}, {pos.y:F2}, {pos.z:F2}");
        }

        string path = EditorUtility.SaveFilePanel("Export Map Data", "", "MapCoordinates", "csv");
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, csv.ToString());
            Debug.Log($"<b>[Map Scanner]</b> Scan complete! Saved to: {path}");
        }
    }
}
#endif