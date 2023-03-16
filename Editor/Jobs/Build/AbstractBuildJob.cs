using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace UnityCiWizard.Editor.Jobs.Build {
	[Serializable]
	public abstract class AbstractBuildJob : UnityJob {
		protected const string TemplateJobUnityModule = "$TEMPLATE_UNITY_MODULE";
		protected const string TemplateJobUnityEditorTarget = "$TEMPLATE_UNITY_EDITOR_TARGET";
		protected const string TemplateJobBuildName = "$TEMPLATE_BUILD_NAME";
		private const string TemplateJobBuildOptions = "$TEMPLATE_BUILD_OPTIONS";
		
		private const string ArgumentJobBuildPath = "jobBuildPath";
		private const string ArgumentJobOptions = "jobOptions";

		public override string[] GetRequiredCommandLineArguments => new[] {
			ArgumentJobBuildPath, ArgumentJobOptions
		};
		
		[SerializeField]
		private BuildOptions _buildOptions;

		public override string[] GetTestExecutionAdditionalParameters => new[]
			{ArgumentJobBuildPath};

		public override void TestExecute(Dictionary<string, string> arguments) {
			arguments?.Add(ArgumentJobOptions,
				GetInternalVariables().First(kv => kv.Key == TemplateJobBuildOptions).Value);
			Execute(arguments);
		}

		public override void Execute(Dictionary<string, string> arguments) {
			var buildPlayerOptions = ConstructBuildOptions(arguments);
			RunBuildPreprocessors(buildPlayerOptions, arguments);
			var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
			WriteAssetReport(report);
			RunBuildPostprocessors(buildPlayerOptions, arguments, report);
		}

		public virtual BuildPlayerOptions ConstructBuildOptions(Dictionary<string, string> arguments) {
			int.TryParse(arguments[ArgumentJobOptions], out var buildOptions);
			PlayerSettings.SplashScreen.showUnityLogo = false;
			var buildPlayerOptions = new BuildPlayerOptions {
				scenes = EditorBuildSettings.scenes.Where(s => s.enabled)
					.Where(s => !string.IsNullOrEmpty(s.path))
					.Select(s => s.path).ToArray(),
				locationPathName = arguments[ArgumentJobBuildPath],
				options = (BuildOptions)buildOptions,
				
			};
			return buildPlayerOptions;
		}

		public override void SetVariables(Dictionary<string, string> variables) {
			base.SetVariables(variables);
			_buildOptions = (BuildOptions)int.Parse(variables[TemplateJobBuildOptions]);
		}

		protected override IEnumerable<KeyValuePair<string, string>> GetInternalVariables() {
			foreach (var internalVariable in base.GetInternalVariables()) {
				yield return internalVariable;
			}
			yield return new KeyValuePair<string, string>(TemplateJobBuildName, GetBuildPath());
			yield return new KeyValuePair<string, string>(TemplateJobBuildOptions, ((int)_buildOptions).ToString());
		}

		protected virtual string GetBuildPath() {
			return PlayerSettings.productName;
		}

		private void RunBuildPreprocessors(BuildPlayerOptions buildPlayerOptions, Dictionary<string, string> arguments) {
			var type = typeof(ICiBuildPreprocessor);
			var types = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(s => s.GetTypes())
				.Where(t => type.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

			var instances = types.Select(t => Activator.CreateInstance(t) as ICiBuildPreprocessor);
			instances = instances.OrderBy(i => i.Order);
			foreach (var buildPreprocessor in instances) {
				buildPreprocessor.PreprocessBuild(buildPlayerOptions, arguments);
			}
		}

		private void WriteAssetReport(BuildReport report) {
			if (report.packedAssets.Length <= 0) return;
			var packedAssets = new List<PackedAssetInfo>();
			var totalSize = 0ul;
			var longestTypeLength = 0;
			var longestPathLength = 0;
			foreach (var asset in report.packedAssets) {
				foreach (var packedAsset in asset.contents) {
					packedAssets.Add(packedAsset);
					totalSize += packedAsset.packedSize;
					longestPathLength = Math.Max(longestPathLength, packedAsset.sourceAssetPath.Length);
					longestTypeLength = Math.Max(longestTypeLength, packedAsset.type.ToString().Split('.').Last().Length);
				}
			}
			var file = new StreamWriter("assetSizeReport.txt", false);
			foreach (var asset in packedAssets.OrderByDescending(pa => pa.packedSize)) {
				file.WriteLine(
					$"{asset.type.ToString().Split('.').Last().PadRight(longestTypeLength)} | {asset.sourceAssetPath.PadRight(longestPathLength)} | {asset.packedSize.ToString(),20} | {(100.0f * asset.packedSize / totalSize):0.000}%");
			}
			file.Close();
		}
		
		private void RunBuildPostprocessors(BuildPlayerOptions buildPlayerOptions, Dictionary<string, string> arguments, BuildReport buildReport) {
			var type = typeof(ICiBuildPostprocessor);
			var types = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(s => s.GetTypes())
				.Where(t => type.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

			var instances = types.Select(t => Activator.CreateInstance(t) as ICiBuildPostprocessor);
			instances = instances.OrderBy(i => i.Order);
			foreach (var buildPreprocessor in instances) {
				buildPreprocessor.PreprocessBuild(buildPlayerOptions, arguments, buildReport);
			}
		}
	}
}