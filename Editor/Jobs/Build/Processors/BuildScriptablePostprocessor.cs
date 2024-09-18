using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace CiWizard.Editor.Jobs.Build.Processors {
    public abstract class BuildScriptablePostprocessor : ScriptableObject {
        public abstract void PostprocessBuild(BuildPlayerOptions buildPlayerOptions, string buildPath,
            BuildReport buildReport);
    }
}