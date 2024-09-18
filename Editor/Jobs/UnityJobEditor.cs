using CiWizard.Editor.Jobs.Common;
using UnityEditor;
using UnityEngine;

namespace CiWizard.Editor.Jobs {
    [CustomEditor(typeof(UnityJob), true)]
    public class UnityJobEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            EditorGUILayout.Space(10.0f);
            if (!EditorGUILayout.DropdownButton(
                    new GUIContent("Execute"), FocusType.Passive,
                    new GUIStyle(GUI.skin.button))) return;
            var jobObject = (UnityJob)serializedObject.targetObject;
            var window = CreateInstance<TestExecutionPopupWindow>();
            window.Show(jobObject.TestExecutionVariables, jobObject.TestExecute);
        }
    }
}