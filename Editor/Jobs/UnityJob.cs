using System.Collections.Generic;
using JetBrains.Annotations;

namespace UnityCiWizard.Editor.Jobs {
	public abstract class UnityJob : AbstractJob {
		private const string TemplateJobType = "$TEMPLATE_JOB_TYPE";
		
		public abstract string[] GetRequiredCommandLineArguments { get; }
		public abstract string[] GetTestExecutionAdditionalParameters { get; }
		
		public abstract void TestExecute([CanBeNull] Dictionary<string, string> arguments);
		public abstract void Execute(Dictionary<string, string> arguments);

		protected override IEnumerable<KeyValuePair<string, string>> GetInternalVariables() {
			yield return new KeyValuePair<string, string>(TemplateJobType, GetType().FullName);
		}
	}
}