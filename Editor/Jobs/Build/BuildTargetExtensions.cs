using UnityEditor;

namespace CiWizard.Editor.Jobs.Build {
	public static class BuildTargetExtensions {
		public static string GetUnityModule(this BuildTarget buildTarget) {
			switch (buildTarget) {
				case BuildTarget.StandaloneOSX:
					return "mac-mono mac-il2cpp";
				case BuildTarget.iOS:
					return "ios";
				case BuildTarget.Android:
					return "android";
				case BuildTarget.WSAPlayer:
				case BuildTarget.StandaloneWindows:
				case BuildTarget.StandaloneWindows64:
					return "windows windows-mono windows-il2cpp";
				case BuildTarget.WebGL:
					return "webgl";
				case BuildTarget.StandaloneLinux64:
					return "linux linux-mono linux-il2cpp";
				case BuildTarget.Lumin:
					return "lumin";
				case BuildTarget.tvOS:
					return "appletv";
				case BuildTarget.PS4:
				case BuildTarget.XboxOne:
				case BuildTarget.Switch:
				case BuildTarget.Stadia:
				case BuildTarget.NoTarget:
				default:
					return "";
			}
		}

		public static string GetJobRunnerTag(this BuildTarget buildTarget) {
			switch (buildTarget) {
				case BuildTarget.StandaloneOSX:
				case BuildTarget.StandaloneLinux64:
				case BuildTarget.StandaloneWindows:
				case BuildTarget.StandaloneWindows64:
				case BuildTarget.WSAPlayer:
					return
						$"{buildTarget.GetUnityEditorTarget()}_{PlayerSettings.GetScriptingBackend(BuildPipeline.GetBuildTargetGroup(buildTarget))}";
				case BuildTarget.Android:
				case BuildTarget.WebGL:
				case BuildTarget.tvOS:
				case BuildTarget.iOS:
				case BuildTarget.Lumin:
				case BuildTarget.PS4:
				case BuildTarget.XboxOne:
				case BuildTarget.Switch:
				case BuildTarget.Stadia:
				case BuildTarget.NoTarget:
				default:
					return buildTarget.GetUnityEditorTarget();
			}
		}

		public static string GetUnityEditorTarget(this BuildTarget buildTarget) {
			switch (buildTarget) {
				case BuildTarget.StandaloneOSX:
					return "OSXUniversal";
				case BuildTarget.iOS:
					return "iOS";
				case BuildTarget.Android:
					return "Android";
				case BuildTarget.WSAPlayer:
					return "WindowsStoreApps";
				case BuildTarget.StandaloneWindows:
					return "Win";
				case BuildTarget.StandaloneWindows64:
					return "Win64";
				case BuildTarget.WebGL:
					return "WebGL";
				case BuildTarget.StandaloneLinux64:
					return "Linux64";
				case BuildTarget.tvOS:
					return "tvOS";
				case BuildTarget.Lumin:
				case BuildTarget.PS4:
				case BuildTarget.XboxOne:
				case BuildTarget.Switch:
				case BuildTarget.Stadia:
				case BuildTarget.NoTarget:
				default:
					return "";
			}
		}
	}
}