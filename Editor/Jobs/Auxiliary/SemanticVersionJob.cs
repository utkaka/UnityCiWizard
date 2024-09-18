using CiWizard.Editor.Jobs.Common;
using UnityEngine;

namespace CiWizard.Editor.Jobs.Auxiliary {
	[CreateAssetMenu(fileName = "SemanticVersion", menuName = "CI/Jobs/Semantic version")]
	public class SemanticVersionJob : AbstractJob {
		public override string TemplateFileName => "Jobs/SemanticVersion";
		public SemanticVersionJob() : base(new JobArtifacts(ArtifactCondition.OnSuccess, new[] {"ProjectSettings/ProjectSettings.asset"})) {
		}
	}
}