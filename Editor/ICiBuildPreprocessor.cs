using System.Collections.Generic;
using UnityEditor;

namespace UnityCiWizard.Editor {
	public interface ICiBuildPreprocessor {
		int Order { get; }
		void PreprocessBuild(BuildPlayerOptions options, Dictionary<string, string> arguments);
	}
}