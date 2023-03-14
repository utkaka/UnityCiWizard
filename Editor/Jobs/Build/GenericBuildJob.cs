using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityCiWizard.Editor.Jobs.Build {
	[CiJobMenu("Build/Generic", 0)]
	[Serializable]
	public class GenericBuildJob : AbstractBuildJob {
		private const string TemplateJobBuildPlayerTarget = "$TEMPLATE_BUILD_PLAYER_TARGET";
		private const string ArgumentJobTarget = "jobTarget";

		[SerializeField]
		private BuildTarget _buildTarget;
		
		public override string TemplateFileName => "Build/GenericBuildJob";

		public override string[] GetRequiredCommandLineArguments =>
			base.GetRequiredCommandLineArguments.Concat(new[] {ArgumentJobTarget}).ToArray();
		
		public override void TestExecute(Dictionary<string, string> arguments) {
			arguments?.Add(ArgumentJobTarget, GetVariable(TemplateJobBuildPlayerTarget));
			base.TestExecute(arguments);
		}
		
		public override BuildPlayerOptions ConstructBuildOptions(Dictionary<string, string> arguments) {
			var buildPlayerOptions = base.ConstructBuildOptions(arguments);
			var buildTarget = (BuildTarget)Enum.Parse(typeof(BuildTarget), arguments[ArgumentJobTarget]);
			buildPlayerOptions.target = buildTarget;
			return buildPlayerOptions;
		}
		
		public override void SetVariables(Dictionary<string, string> variables) {
			base.SetVariables(variables);
			_buildTarget = (BuildTarget)Enum.Parse(typeof(BuildTarget), variables[TemplateJobBuildPlayerTarget]);
		}

		protected override IEnumerable<KeyValuePair<string, string>> GetInternalVariables() {
			foreach (var internalVariable in base.GetInternalVariables()) {
				yield return internalVariable;
			}
			yield return new KeyValuePair<string, string>(TemplateJobUnityModule, _buildTarget.GetUnityModule());
			yield return new KeyValuePair<string, string>(TemplateJobUnityEditorTarget, _buildTarget.GetUnityEditorTarget());
			yield return new KeyValuePair<string, string>(TemplateJobBuildPlayerTarget, _buildTarget.ToString());
		}
	}
}