using UnityEditor;
using UnityEngine;

namespace CiWizard.Editor.Jobs.Build.Targets {
    [CreateAssetMenu(fileName = "iOS", menuName = "CI/Jobs/Build/iOS")]
    public class IosBuildJob : AbstractBuildJob {
        protected override BuildTarget JobBuildTarget => BuildTarget.iOS;
        public override string FileExtension => "";
    }
}