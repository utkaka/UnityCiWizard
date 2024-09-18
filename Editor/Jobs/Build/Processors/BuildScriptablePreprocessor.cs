using UnityEditor;
using UnityEngine;

namespace CiWizard.Editor.Jobs.Build.Processors {
    public abstract class BuildScriptablePreprocessor : ScriptableObject {
        public abstract void PreprocessBuild(BuildPlayerOptions buildPlayerOptions);
    }
}