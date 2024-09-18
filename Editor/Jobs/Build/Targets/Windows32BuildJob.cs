using UnityEditor;
using UnityEngine;

namespace CiWizard.Editor.Jobs.Build.Targets {
    [CreateAssetMenu(fileName = "Windows32", menuName = "CI/Jobs/Build/Windows32")]
    public class Windows32BuildJob : AbstractBuildJob {
        protected override BuildTarget JobBuildTarget => BuildTarget.StandaloneWindows;
        public override string FileExtension => "exe";
    }
}