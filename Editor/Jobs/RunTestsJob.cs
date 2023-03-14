using System;
using System.Collections.Generic;

namespace UnityCiWizard.Editor.Jobs {
	[CiJobMenu("Run Tests", 3)]
	public class RunTestsJob : UnityJob {
		public override string TemplateFileName => "RunTests";
		public override string[] GetRequiredCommandLineArguments => Array.Empty<string>();
		public override string[] GetTestExecutionAdditionalParameters => Array.Empty<string>();

		public RunTestsJob() {
			Name = "Run Tests";
		}

		public override void TestExecute(Dictionary<string, string> arguments) {
		}

		public override void Execute(Dictionary<string, string> arguments) {
		}
	}
}