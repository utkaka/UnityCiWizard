using System.Collections.Generic;
using UnityEditor;

namespace UnityCiWizard.Editor.Jobs.Build {
	[CiJobMenu("Build/MacOS", 3)]
	public class MacOsBuildJob : AbstractBuildJob {
		public override string TemplateFileName => "Build/PlatformBuildJob";
		
		public override BuildPlayerOptions ConstructBuildOptions(Dictionary<string, string> arguments) {
			var buildPlayerOptions = base.ConstructBuildOptions(arguments);
			buildPlayerOptions.target = BuildTarget.StandaloneOSX;
			return buildPlayerOptions;
		}
		
		protected override string GetBuildPath() {
			return base.GetBuildPath() + ".app";
		}
		
		protected override IEnumerable<KeyValuePair<string, string>> GetInternalVariables() {
			foreach (var internalVariable in base.GetInternalVariables()) {
				yield return internalVariable;
			}
			yield return new KeyValuePair<string, string>(TemplateJobUnityModule, BuildTarget.StandaloneOSX.GetUnityModule());
			yield return new KeyValuePair<string, string>(TemplateJobUnityEditorTarget, BuildTarget.StandaloneOSX.GetUnityEditorTarget());
		}
	}
}