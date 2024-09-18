using UnityEditor;
using UnityEngine;

namespace CiWizard.Editor.Jobs.Build.Targets {
	[CreateAssetMenu(fileName = "MacOS", menuName = "CI/Jobs/Build/MacOS")]
	public class MacOsBuildJob : AbstractBuildJob {
		protected override BuildTarget JobBuildTarget => BuildTarget.StandaloneOSX;
		public override string FileExtension => "app";
	}
}