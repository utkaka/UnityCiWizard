using System.IO;
using System.Threading.Tasks;
using Scriban;
using Scriban.Parsing;
using Scriban.Runtime;

namespace CiWizard.Editor.Gitlab {
    public class GitlabTemplateLoader : ITemplateLoader {
        private const string TemplatesPath = "Templates";

        private string _templatePath;

        public GitlabTemplateLoader() {
            var currentPath = Directory.GetParent(new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName() ?? string.Empty)
                ?.FullName;
            _templatePath = Path.Combine(currentPath, TemplatesPath);
        }

        public string GetPath(TemplateContext context, SourceSpan callerSpan, string templateName) {
            return Path.Combine(_templatePath, $"{templateName}.yml");
        }

        public string Load(TemplateContext context, SourceSpan callerSpan, string templatePath) {
            return File.ReadAllText(templatePath);
        }

        public async ValueTask<string> LoadAsync(TemplateContext context, SourceSpan callerSpan, string templatePath) {
            return await File.ReadAllTextAsync(templatePath);
        }
    }
}