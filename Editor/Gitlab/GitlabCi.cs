using System.IO;
using Scriban;
using Scriban.Runtime;
using UnityEngine;

namespace CiWizard.Editor.Gitlab {
	public class GitlabCi {
		private const string OutputPath = ".gitlab-ci.yml";
		
		public void Apply(CiConfig config) {
			var context = new TemplateContext();
			context.TemplateLoader = new GitlabTemplateLoader();
			
			var model = new ScriptObject();
			model.Add("config_guid", config.GetConfigGuid());
			model.Add("project_path", config.GetProjectRelativePath());
			model.Add("jobs", config.Jobs);
			model.Import(typeof(GitlabTemplateFunctions));
			
			context.PushGlobal(model);

			var template = Template.Parse("{{include 'Main'}}");

			var directoryInfo = new DirectoryInfo(Application.dataPath).Parent;
			if (directoryInfo != null)
				File.WriteAllText(Path.Combine(CiConfig.GetGitProjectRoot(), OutputPath), template.Render(context));
		}
	}
}