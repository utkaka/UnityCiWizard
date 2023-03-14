using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityCiWizard.Editor.Jobs;
using UnityCiWizard.Editor.UI;
using UnityEditor;
using UnityEngine;

namespace UnityCiWizard.Editor.Gitlab {
	public class GitlabCiEditor : CiEditorWindow {
		private const string JobSeparator = "#--------------\n";
		private const string JobType = "#$TEMPLATE_JOB_TYPE\n";
		private const string OutputPath = ".gitlab-ci.yml";
		private const string TemplatesPath = "Templates";
		private const string MainTemplateName = "Main.yml";
		
		
		[MenuItem("Window/CI/Gitlab Wizard")]
		public static void ShowWizard() {
			Show<GitlabCiEditor>();
		}

		protected override List<AbstractJob> ParseExisting() {
			var existingJobs = new List<AbstractJob>();
			var directoryInfo = new DirectoryInfo(Application.dataPath).Parent;
			if (directoryInfo == null) return existingJobs;
			var configPath = Path.Combine(directoryInfo.FullName, OutputPath);
			if (!File.Exists(configPath)) return existingJobs;
			var ciFile = File.ReadAllText(configPath);
			var jobsSplit = ciFile.Split(new []{JobSeparator}, StringSplitOptions.RemoveEmptyEntries).Skip(1);
			foreach (var jobString in jobsSplit) {
				var firstLineBreak = jobString.IndexOf("\n", StringComparison.Ordinal);
				var typeString = jobString.Substring(1, firstLineBreak - 1);
				var jobStringWithoutType = jobString.Substring(firstLineBreak + 1);
				var job = (AbstractJob)Activator.CreateInstance(Type.GetType(typeString));
				job.SetVariables(TemplateProcessor.ParseFilledTemplate(
					File.ReadAllText(Path.Combine(CurrentPath, TemplatesPath, $"{job.TemplateFileName}.yml")),
					jobStringWithoutType, job.GetVariables()));
				existingJobs.Add(job);
			}
			return existingJobs;
		}

		protected override void Apply() {
			var mainBuilder = new StringBuilder(File.ReadAllText(Path.Combine(CurrentPath, TemplatesPath, MainTemplateName)));
			foreach (var buildJob in Jobs) {
				var jobBuildTemplate = File.ReadAllText(Path.Combine(CurrentPath, TemplatesPath, $"{buildJob.TemplateFileName}.yml"));
				mainBuilder.AppendLine();
				mainBuilder.AppendLine();
				mainBuilder.Append(JobSeparator);
				mainBuilder.Append(JobType.Replace("$TEMPLATE_JOB_TYPE", buildJob.GetType().FullName));
				mainBuilder.Append(TemplateProcessor.FillTemplate(jobBuildTemplate, buildJob.GetVariables()));
			}

			var directoryInfo = new DirectoryInfo(Application.dataPath).Parent;
			if (directoryInfo != null)
				File.WriteAllText(Path.Combine(directoryInfo.FullName, OutputPath), mainBuilder.ToString());
		}
	}
}