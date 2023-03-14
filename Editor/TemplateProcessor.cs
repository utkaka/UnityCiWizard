using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityCiWizard.Editor {
	public static class TemplateProcessor {
		public static string FillTemplate(string template, IEnumerable<KeyValuePair<string, string>> variables) {
			var templateBuilder = new StringBuilder(template);
			foreach (var templateVariable in variables) {
				templateBuilder.Replace(templateVariable.Key, templateVariable.Value);
			}

			return templateBuilder.ToString();
		}

		public static Dictionary<string, string> ParseFilledTemplate(string template, string filledTemplate, IEnumerable<KeyValuePair<string, string>> variables) {
			var result = new Dictionary<string, string>();
			var orderedVariables = variables.OrderBy(kv => template.IndexOf(kv.Key, StringComparison.Ordinal))
				.Select(kv => kv.Key).ToList();
			for (var i = 0; i < orderedVariables.Count; i++) {
				var variable = orderedVariables[i];
				var startIndex = template.IndexOf(variable, StringComparison.Ordinal);
				var endIndex = i < orderedVariables.Count - 1 ? template.IndexOf(orderedVariables[i + 1], StringComparison.Ordinal) : template.Length;
				var variableTemplateEndIndex = startIndex + variable.Length;
				var sameVariableNextIndex = template.IndexOf(variable, variableTemplateEndIndex, StringComparison.Ordinal);
				if (sameVariableNextIndex > -1 && endIndex > sameVariableNextIndex) endIndex = sameVariableNextIndex;
				var endSubstring = template.Substring(variableTemplateEndIndex, endIndex - variableTemplateEndIndex);
				var variableEndIndex = !string.IsNullOrEmpty(endSubstring)
					? filledTemplate.IndexOf(endSubstring, startIndex, StringComparison.Ordinal)
					: filledTemplate.Length;
				var variableValue = filledTemplate.Substring(startIndex, variableEndIndex - startIndex);
				template = template.Replace(variable, variableValue);
				result.Add(variable, variableValue);
			}
			return result;
		}
	}
}