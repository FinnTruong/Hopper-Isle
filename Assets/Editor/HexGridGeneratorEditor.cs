using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(HexGridGenerator))]
public class HexGridGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        HexGridGenerator generator = target as HexGridGenerator;
        if (DrawDefaultInspector())
        {
            generator.GenerateGrid();
            generator.UnlockAllTile();
        }

        if (GUILayout.Button("Generate Map"))
        {
            generator.GenerateGrid();
            generator.UnlockAllTile();
        }
    }
}
