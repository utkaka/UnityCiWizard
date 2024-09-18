using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CiWizard.Editor.Jobs.Common {
	public class TestExecutionPopupWindow : EditorWindow {
		private Action<Dictionary<string, string>> _executeMethod;
		private string[] _argumentKeys;
		private Dictionary<string, string> _arguments;
		private Vector2 _scrollPosition;

		public void Show(string[] arguments, Action<Dictionary<string, string>> executeMethod) {
			_arguments = new Dictionary<string, string>();
			_argumentKeys = arguments;
			foreach (var argumentKey in arguments) {
				_arguments.Add(argumentKey, "");
			}
			_executeMethod = executeMethod;
			ShowModal();
		}
		
		private void OnGUI() {
			_scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
			foreach (var argumentKey in _argumentKeys) {
				GUILayout.BeginHorizontal();
				GUILayout.Label(argumentKey);
				_arguments[argumentKey] = EditorGUILayout.TextField(_arguments[argumentKey]);
				GUILayout.EndHorizontal();
			}
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			var cancel =GUILayout.Button("Cancel");
			var execute = GUILayout.Button("Execute");
			GUILayout.EndHorizontal();
			GUILayout.EndScrollView();
			if (!cancel && !execute) return;
			Close();
			if (execute) {
				EditorApplication.delayCall = () => {
					_executeMethod?.Invoke(_arguments);
					EditorApplication.delayCall = null;
				};
			}
		}
	}
}