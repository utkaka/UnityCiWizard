using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace UnityCiWizard.Editor.Jobs {
	[CiJobMenu("Semantic version", 1)]
	public class SemanticVersionJob : UnityJob {
		private const string ArgumentJobCommitTitleAndDescription = "jobCommitTitleAndDescription";
		public override string TemplateFileName => "SemanticVersion";
		public override string[] GetRequiredCommandLineArguments => new[] {ArgumentJobCommitTitleAndDescription};
		public override string[] GetTestExecutionAdditionalParameters => new[] {ArgumentJobCommitTitleAndDescription};

		public SemanticVersionJob() {
			Name = "Version Update";
		}

		public override void TestExecute(Dictionary<string, string> arguments) {
			Execute(arguments);
		}

		public override void Execute(Dictionary<string, string> arguments) {
			var commitTitleAndDescription = arguments[ArgumentJobCommitTitleAndDescription];
			var commitTitleAndDescriptionSplit = commitTitleAndDescription.Split(';');
			foreach (var titleOrDescription in commitTitleAndDescriptionSplit) {
				if (!DetectSemanticVersion(titleOrDescription)) continue;
				break;
			}
		}

		private bool DetectSemanticVersion(string commitTitleOrDescription) {
			commitTitleOrDescription = commitTitleOrDescription.Trim();
			var commitType = commitTitleOrDescription.Split(':')[0].ToLower();
			var bundleVersionSplit = PlayerSettings.bundleVersion.Split('.');
			if (bundleVersionSplit.Length != 3)
				throw new ArgumentException("Bundle version has to have {major}.{minor}.{patch} format");
			PlayerSettings.Android.bundleVersionCode += 1;
			PlayerSettings.iOS.buildNumber = PlayerSettings.Android.bundleVersionCode.ToString();
			var major = int.Parse(bundleVersionSplit[0]);
			var minor = int.Parse(bundleVersionSplit[1]);
			var patch = int.Parse(bundleVersionSplit[2]);
			switch (commitType) {
				case "fix":
					patch++;
					break;
				case "feat":
					patch = 0;
					minor++;
					break;
				case "mile":
					patch = 0;
					minor = 0;
					major++;
					break;
				default:
					Debug.Log($"Nothing to do with {commitTitleOrDescription}");
					return false;
			}
			var file = new StreamWriter("old_bundle_version.txt", false);
			file.WriteLine(PlayerSettings.bundleVersion);
			file.Close();
			PlayerSettings.bundleVersion = $"{major}.{minor}.{patch}";
			return true;
		}
	}
}