using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityCiWizard.Editor.UI {
	public class TestExecutionPopupWindow : EditorWindow {
		private Action<Dictionary<string, string>> _executeMethod;
		private string[] _argumentKeys;
		private Dictionary<string, string> _arguments;
		private Vector2 _scrollPosition;

		public void Show(Rect parentRect, string[] arguments, Action<Dictionary<string, string>> executeMethod) {
			position = new Rect((parentRect.width - 250.0f) / 2.0f + parentRect.position.x,
				(parentRect.height - 150.0f) / 2.0f + parentRect.position.y, 300, 300);
			_arguments = new Dictionary<string, string>();
			_argumentKeys = arguments;
			foreach (var argumentKey in arguments) {
				_arguments.Add(argumentKey, "");
			}
			_executeMethod = executeMethod;
			ShowPopup();
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
			if (execute) _executeMethod?.Invoke(_arguments);
		}
	}
}