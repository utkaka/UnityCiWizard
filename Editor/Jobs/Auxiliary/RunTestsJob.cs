using CiWizard.Editor.Jobs.Common;
using UnityEngine;

namespace CiWizard.Editor.Jobs.Auxiliary {
	[CreateAssetMenu(fileName = "RunTests", menuName = "CI/Jobs/Run Tests")]
	public class RunTestsJob : UnityJob {
		public override string TemplateFileName => "Jobs/RunTests";
		public override void Execute() { }

		public RunTestsJob() : base(new JobArtifacts(ArtifactCondition.OnSuccess, new []{"tests.xml"})) {
		}
	}
}