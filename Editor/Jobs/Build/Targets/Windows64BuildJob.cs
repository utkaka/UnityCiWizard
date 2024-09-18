using UnityEditor;
using UnityEngine;

namespace CiWizard.Editor.Jobs.Build.Targets {
    [CreateAssetMenu(fileName = "Windows64", menuName = "CI/Jobs/Build/Windows64")]
    public class Windows64BuildJob : Windows32BuildJob {
        protected override BuildTarget JobBuildTarget => BuildTarget.StandaloneWindows64;
    }
}