using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Grid grid = (Grid)target;
        if (GUILayout.Button("Add Prefab"))
        {
            grid.AddPrefab();
        }
    }
}
