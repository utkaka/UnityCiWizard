using System;
using System.Collections.Generic;
using System.IO;
using CiWizard.Editor.Jobs;
using UnityEditor;
using UnityEngine;

namespace CiWizard.Editor {
    [CreateAssetMenu(fileName = "CIConfig", menuName = "CI/CI Config", order = 0)]
    public class CiConfig : ScriptableObject {
        [SerializeField]
        private List<AbstractJob> _jobs;
        public IReadOnlyList<AbstractJob> Jobs => _jobs;
        
        public static void Execute() {
            BuildLogHandler.WriteSectionEnd("import_section");
            BuildLogHandler.WriteSectionBegin("build_section", "Executing CI function");
            Debug.unityLogger.logHandler = new BuildLogHandler();
            var configGuid = Environment.GetEnvironmentVariable("UCI_CFG_JOB_UNITY_CONFIG_GUID");
            var jobIndex = int.Parse(Environment.GetEnvironmentVariable("UCI_CFG_JOB_UNITY_INDEX") ?? "-1");
            var configAssetPath = AssetDatabase.GUIDToAssetPath(configGuid);
            var configAsset = AssetDatabase.LoadAssetAtPath<CiConfig>(configAssetPath);
            var job = configAsset.Jobs[jobIndex] as UnityJob;
            job?.Execute();
            BuildLogHandler.WriteSectionEnd("build_section");
            BuildLogHandler.WriteSectionBegin("exit_section", "Closing Unity");
        }

        public string GetConfigGuid() {
            return AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(this)).ToString();
        }

        public string GetProjectRelativePath() {
            var directory = new DirectoryInfo(Application.dataPath);
            return Path.GetRelativePath(GetGitProjectRoot(), directory.Parent.FullName);
        }
        
        public static string GetGitProjectRoot() {
            var directory = new DirectoryInfo(Application.dataPath);
            while (directory.Parent != null && !Directory.Exists(Path.Combine(directory.FullName, ".git"))) {
                directory = directory.Parent;
            }

            return directory.FullName;
        }

        internal void AddJob(AbstractJob job) {
            _jobs ??= new List<AbstractJob>();
            _jobs.Add(job);
        }
    }
}