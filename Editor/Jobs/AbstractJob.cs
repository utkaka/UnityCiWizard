using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityCiWizard.Editor.Jobs {
	[Serializable]
	public abstract class AbstractJob {
		public const string TemplateJobName = "$TEMPLATE_JOB_NAME";
		private const string TemplateJobConditionBranches = "$TEMPLATE_JOB_CONDITION_BRANCHES";

		public abstract string TemplateFileName { get; }

		[SerializeField]
		protected string Name;
		[SerializeField]
		private string _branches;

		protected AbstractJob() {
			_branches = "main";
		}

		public string GetVariable(string key) {
			return GetVariables().First(kv => kv.Key == key).Value;
		}

		public IEnumerable<KeyValuePair<string, string>> GetVariables() {
			yield return new KeyValuePair<string, string>(TemplateJobName, Name);
			yield return new KeyValuePair<string, string>(TemplateJobConditionBranches, _branches);
			foreach (var keyValuePair in GetInternalVariables()) {
				yield return keyValuePair;
			}
		}

		public virtual void SetVariables(Dictionary<string, string> variables) {
			Name = variables[TemplateJobName];
			_branches = variables[TemplateJobConditionBranches];
		}
		
		protected abstract IEnumerable<KeyValuePair<string, string>> GetInternalVariables();
	}
}