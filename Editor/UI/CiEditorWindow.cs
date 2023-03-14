using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityCiWizard.Editor.Jobs;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Screen;

namespace UnityCiWizard.Editor.UI {
	public abstract class CiEditorWindow : EditorWindow {
		[SerializeReference]
		private List<AbstractJob> _jobs;
		
		protected string CurrentPath { get; private set; }
		protected List<AbstractJob> Jobs => _jobs;

		private SerializedObject _serializedObject;
		private List<SerializedProperty> _jobsToRemove;
		private List<Tuple<Type, CiJobMenuAttribute>> _jobsMenuTypes;
		
		private Vector2 _scrollPosition;
		
		protected static void Show<T>() where T : CiEditorWindow {
			var window = (CiEditorWindow)GetWindow(typeof(T));
			var ciFile = new System.Diagnostics.StackTrace(true).GetFrame(1).GetFileName();
			if (ciFile != null) window.CurrentPath = Directory.GetParent(ciFile)?.FullName;
			window._jobs = window.ParseExisting();
			window._serializedObject = new SerializedObject(window);
			window.titleContent = new GUIContent(typeof(T).Name);
			window.Show();
		}

		private void OnEnable() {
			var typesWithMenuAttribute = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(a => a.GetTypes(), (a, t) => new {a, t})
				.Select(@t1 => new {@t1, attributes = @t1.t.GetCustomAttributes(typeof(CiJobMenuAttribute), false)})
				.Where(@t1 => @t1.attributes.Length > 0)
				.Select(@t1 => new Tuple<Type, CiJobMenuAttribute>(@t1.@t1.t, @t1.attributes[0] as CiJobMenuAttribute));
			_jobsMenuTypes = typesWithMenuAttribute.OrderBy(t => t.Item2.Order).ToList();
			_jobsToRemove = new List<SerializedProperty>();
		}

		private void OnGUI() {
			_scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
			GUILayout.Label("CI Jobs", EditorStyles.boldLabel);
			var jobs = _serializedObject.FindProperty(nameof(_jobs));
			for (var i = 0; i < jobs.arraySize; i++) {
				EditorGUILayout.PropertyField(jobs.GetArrayElementAtIndex(i), true);
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				if (EditorGUILayout.DropdownButton(new GUIContent("Remove"), FocusType.Passive,
					    new GUIStyle(GUI.skin.button) {fixedWidth = 100.0f, stretchWidth = false})) {
					_jobsToRemove.Add(jobs.GetArrayElementAtIndex(i));
				}
				if (_jobs[i] is UnityJob && EditorGUILayout.DropdownButton(new GUIContent("Execute"), FocusType.Passive,
					    new GUIStyle(GUI.skin.button) {fixedWidth = 100.0f, stretchWidth = false})) {
					var window = CreateInstance<TestExecutionPopupWindow>();
					window.Show(new Rect(position.x, position.y, width, height),
						(_jobs[i] as UnityJob)?.GetTestExecutionAdditionalParameters, ((UnityJob) _jobs[i]).TestExecute);

				}
				GUILayout.EndHorizontal();
			}

			foreach (var jobToRemove in _jobsToRemove) {
				jobToRemove.DeleteCommand();
			}
			_jobsToRemove.Clear();
			
			GUILayout.Space(20.0f);
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (EditorGUILayout.DropdownButton(new GUIContent("Add a new job"), FocusType.Passive,
				    new GUIStyle(GUI.skin.button) {fixedWidth = 100.0f, stretchWidth = false})) {
				ShowCreateNewJob();
			}
			GUILayout.EndHorizontal();

			GUILayout.FlexibleSpace();
			
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			var cancel =GUILayout.Button("Cancel");
			var apply = GUILayout.Button("Apply");
			GUILayout.EndHorizontal();
			GUILayout.Space(20.0f);
			GUILayout.EndScrollView();
			if (cancel) Close();
			if (!apply) return;
			_serializedObject.ApplyModifiedProperties();
			Apply();
			Close();
		}

		private void ShowCreateNewJob() {
			var menu = new GenericMenu();
			foreach (var jobMenuType in _jobsMenuTypes) {
				menu.AddItem(new GUIContent(jobMenuType.Item2.JobMenuName), false, OnCreateNewJob, jobMenuType.Item1);
			}
			menu.ShowAsContext();
		}

		private void OnCreateNewJob(object jobType) {
			_serializedObject.ApplyModifiedProperties();
			_jobs.Add((AbstractJob)Activator.CreateInstance((Type)jobType)); 
			_serializedObject.Update();
		}

		protected abstract List<AbstractJob> ParseExisting();

		protected abstract void Apply();
	}
}