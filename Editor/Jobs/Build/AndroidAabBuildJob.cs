using System.Collections.Generic;
using UnityEditor;

namespace UnityCiWizard.Editor.Jobs.Build {
	[CiJobMenu("Build/Android AAB", 1)]
	public class AndroidAabBuildJob : AndroidApkBuildJob {
		public override BuildPlayerOptions ConstructBuildOptions(Dictionary<string, string> arguments) {
			var buildOptions = base.ConstructBuildOptions(arguments);
			EditorUserBuildSettings.buildAppBundle = true;
			return buildOptions;
		}
		
		protected override string GetBuildPath() {
			return base.GetBuildPath().Replace("apk", "aab");
		}
	}
}