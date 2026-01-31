using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(KeybindTarget))]
public class KeybindTargetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var t = (KeybindTarget)target;

        GUILayout.Space(10);

        if (!t.IsListening())
        {
            if (GUILayout.Button("Bind Key"))
            {
                t.StartRebind();
            }
        }
        else
        {
            GUILayout.Label("Waiting for input... (ESC = Cancel)");

            if (GUILayout.Button("Cancel"))
            {
                t.CancelRebind();
            }
        }
    }
}
