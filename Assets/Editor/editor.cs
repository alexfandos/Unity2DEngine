using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ActionTriggerer))]
public class MyMonoBehaviourEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        ActionTriggerer script = (ActionTriggerer)target;

        // Ensure there is a ScriptableObject reference
        if (script.actions != null && script.actions.Count > 0)
        {
            foreach (var so in script.actions)
            {
                if (so != null)
                {
                    EditorGUILayout.LabelField(so.name, EditorStyles.boldLabel);

                    Editor soEditor = CreateEditor(so);
                    soEditor.OnInspectorGUI();
                    EditorGUILayout.Space();
                }
            }
        }
    }
}