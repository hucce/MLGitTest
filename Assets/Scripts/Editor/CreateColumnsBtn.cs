using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreateColumns))]
public class CreateColumnsBtn : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CreateColumns generator = (CreateColumns)target;
        if (GUILayout.Button("Generate Cubes"))
        {
            generator.GenerateColumns();
        }
    }
}