using UnityEditor;
using UnityEngine;

namespace CiWizard.Editor.Jobs.Build.Targets {
    [CreateAssetMenu(fileName = "WebGL", menuName = "CI/Jobs/Build/WebGL")]
    public class WebGlBuildJob : AbstractBuildJob {
        protected override BuildTarget JobBuildTarget => BuildTarget.WebGL;
        public override string FileExtension => "";
    }
}