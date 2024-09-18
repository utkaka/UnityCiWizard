using System;
using System.Collections.Generic;
using CiWizard.Editor.Jobs.Common;
using JetBrains.Annotations;

namespace CiWizard.Editor.Jobs {
	public abstract class UnityJob : AbstractJob {
		public virtual string[] TestExecutionVariables => Array.Empty<string>();
		
		protected UnityJob(JobArtifacts artifacts) : base(artifacts) { }


		public void TestExecute([CanBeNull] Dictionary<string, string> arguments) {
			if (arguments == null) {
				Execute();
				return;
			}
			foreach (var environmentVariable in arguments.Keys) {
				Environment.SetEnvironmentVariable(environmentVariable, arguments[environmentVariable]);
			}
			Execute();
		}
		public abstract void Execute();
	}
}