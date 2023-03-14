using System.Collections.Generic;

namespace UnityCiWizard.Editor.Jobs {
	[CiJobMenu("Extract Android Keystore", 2)]
	public class ExtractAndroidKeystoreJob : AbstractJob {
		public override string TemplateFileName => "ExtractAndroidKeystore";
		
		public ExtractAndroidKeystoreJob() {
			Name = "Extract Android Keystore";
		}
		
		protected override IEnumerable<KeyValuePair<string, string>> GetInternalVariables() {
			yield break;
		}
	}
}