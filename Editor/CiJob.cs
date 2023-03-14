using System;
using System.Collections.Generic;
using System.Linq;
using UnityCiWizard.Editor.Jobs;

namespace UnityCiWizard.Editor {
	public static class CiJob {
		public static void Execute() {
			var jobTypeArgument = new[] {"ciJobType"};
			var job = (UnityJob) Activator.CreateInstance(
				Type.GetType(GetRequiredCommandLineArguments(jobTypeArgument)["ciJobType"]) ?? throw new InvalidOperationException());
			var jobArguments = GetRequiredCommandLineArguments(job.GetRequiredCommandLineArguments);
			job.Execute(jobArguments);
		}
		
		private static Dictionary<string, string> GetRequiredCommandLineArguments(string[] requiredArguments) {
			var result = new Dictionary<string, string>();
			var args = Environment.GetCommandLineArgs();
			for (var i = 0; i < args.Length; i++) {
				var arg = args[i];
				arg = arg.Substring(1, arg.Length - 1);
				if (!requiredArguments.Contains(arg)) continue;
				i++;
				var argumentValue = args[i];
				var doubleQuotesCount = argumentValue.Count(c => c == '"');
				while (doubleQuotesCount % 2 != 0) {
					i++;
					argumentValue += $" {args[i]}";
					doubleQuotesCount += args[i].Count(c => c == '"');
				}
				result.Add(arg, argumentValue.Replace("\"", ""));
			}
			return result;
		}
	}
}
