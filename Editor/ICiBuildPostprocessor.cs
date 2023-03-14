using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace UnityCiWizard.Editor {
	public interface ICiBuildPostprocessor {
		int Order { get; }
		void PreprocessBuild(BuildPlayerOptions options, Dictionary<string, string> arguments, BuildReport buildReport);
	}
}