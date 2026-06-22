using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ProceduralMazeGenerator))]
public class MazeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ProceduralMazeGenerator gen = (ProceduralMazeGenerator)target;
        if (GUILayout.Button("GENERATE MAZE"))
        {
            gen.GenerateMaze();
        }
    }
}