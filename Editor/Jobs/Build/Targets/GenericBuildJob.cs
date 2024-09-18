using UnityEditor;
using UnityEngine;

namespace CiWizard.Editor.Jobs.Build.Targets {
	[CreateAssetMenu(fileName = "GenericBuild", menuName = "CI/Jobs/Build/Generic")]
	public class GenericBuildJob : AbstractBuildJob {
		[SerializeField]
		private BuildTarget _buildTarget;

		protected override BuildTarget JobBuildTarget => _buildTarget;
		public override string FileExtension => "";
	}
}