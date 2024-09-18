using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CiWizard.Editor.Gitlab;
using CiWizard.Editor.Jobs;
using UnityEditor;
using UnityEngine;

namespace CiWizard.Editor {
	[CustomEditor(typeof(CiConfig))]
	public class CiConfigEditor : UnityEditor.Editor {
		private List<Tuple<Type, CreateAssetMenuAttribute>> _jobsMenuTypes;

		private void OnEnable() {
			var typesWithMenuAttribute = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(a => a.GetTypes(), (a, t) => new {a, t})
				.Where(t1 => t1.t.IsSubclassOf(typeof(AbstractJob)))
				.Select(t1 => new {t1, attributes = t1.t.GetCustomAttributes(typeof(CreateAssetMenuAttribute), false)})
				.Where(t1 => t1.attributes.Length > 0)
				.Select(t1 => new Tuple<Type, CreateAssetMenuAttribute>(t1.t1.t, t1.attributes[0] as CreateAssetMenuAttribute));
			_jobsMenuTypes = typesWithMenuAttribute.OrderBy(t => t.Item2.order).ToList();
		}

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			if (EditorGUILayout.DropdownButton(new GUIContent("Add a new job"), FocusType.Passive,
				    new GUIStyle(GUI.skin.button))) {
				ShowCreateNewJob();
			}
			serializedObject.ApplyModifiedProperties();
			
			EditorGUILayout.Space(10.0f);
			if (GUILayout.Button("Overwrite .gitlab-ci")) {
				var gitlabCi = new GitlabCi();
				gitlabCi.Apply((CiConfig)serializedObject.targetObject);
				EditorUtility.SetDirty(serializedObject.targetObject);
				AssetDatabase.SaveAssets();
			}
		}

		private void ShowCreateNewJob() {
			var menu = new GenericMenu();
			foreach (var jobMenuType in _jobsMenuTypes) {
				var menuName = jobMenuType.Item2.menuName;
				menuName = menuName.Replace("CI/Jobs/", "");
				menu.AddItem(new GUIContent(menuName), false, OnCreateNewJob, new Tuple<Type, string>(jobMenuType.Item1, jobMenuType.Item2.fileName));
			}
			menu.ShowAsContext();
		}

		private void OnCreateNewJob(object parameters) {
			var jobParameters = (Tuple<Type, string>)parameters;
			serializedObject.ApplyModifiedProperties();
			var job = CreateInstance(jobParameters.Item1);
			job.name = jobParameters.Item2;
			AssetDatabase.CreateAsset(job,
				Path.GetRelativePath(".",
					Path.Combine(Directory.GetParent(AssetDatabase.GetAssetPath(target)).FullName,
						$"{job.name}.asset")));
			((CiConfig)serializedObject.targetObject).AddJob((AbstractJob)job);
			serializedObject.Update();
			EditorUtility.SetDirty(serializedObject.targetObject);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			Selection.activeObject = job;
		}
	}
}