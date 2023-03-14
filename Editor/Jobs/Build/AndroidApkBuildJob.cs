using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityCiWizard.Editor.Jobs.Build {
	[CiJobMenu("Build/Android APK", 2)]
	[Serializable]
	public class AndroidApkBuildJob : AbstractBuildJob {
		private const string TemplateJobKeyStore = "$TEMPLATE_KEYSTORE_NAME";
		private const string TemplateJobKeyAlias = "$TEMPLATE_ALIAS_NAME";
		private const string TemplateJobKeyStorePass = "$TEMPLATE_KEYSTORE_PASS";
		private const string TemplateJobKeyAliasPass = "$TEMPLATE_ALIAS_PASS";
		
		private const string ArgumentJobKeyStore = "jobKeyStore";
		private const string ArgumentJobKeyAlias = "jobKeyAlias";
		private const string ArgumentJobKeyStorePass = "jobKeyStorePass";
		private const string ArgumentJobKeyAliasPass = "jobKeyAliasPass";
		
		public override string TemplateFileName => "Build/AndroidBuildJob";
		[SerializeField]
		private string _keyStore;
		[SerializeField]
		private string _keyAlias;
		[SerializeField]
		private string _keyStorePass;
		[SerializeField]
		private string _keyAliasPass;

		public AndroidApkBuildJob() {
			_keyStore = "ci.keystore";
			_keyAlias = "$UCI_ANDROID_KEYSTORE_ALIAS";
			_keyStorePass = "$UCI_ANDROID_KEYSTORE_PASS";
			_keyAliasPass = "$UCI_ANDROID_KEYSTORE_ALIAS_PASS";
		}
		
		public override string[] GetRequiredCommandLineArguments =>
			base.GetRequiredCommandLineArguments.Concat(new[] {ArgumentJobKeyStore, ArgumentJobKeyAlias, ArgumentJobKeyStorePass, ArgumentJobKeyAliasPass}).ToArray();
		
		public override void TestExecute(Dictionary<string, string> arguments) {
			arguments?.Add(ArgumentJobKeyStore, _keyStore);
			arguments?.Add(ArgumentJobKeyAlias, _keyAlias);
			arguments?.Add(ArgumentJobKeyStorePass, _keyStorePass);
			arguments?.Add(ArgumentJobKeyAliasPass, _keyAliasPass);
			base.TestExecute(arguments);
		}

		public override BuildPlayerOptions ConstructBuildOptions(Dictionary<string, string> arguments) {
			var buildPlayerOptions = base.ConstructBuildOptions(arguments);
			EditorUserBuildSettings.buildAppBundle = false;
			buildPlayerOptions.target = BuildTarget.Android;
			PlayerSettings.Android.useCustomKeystore = true;
			PlayerSettings.Android.keystoreName = arguments[ArgumentJobKeyStore];
			PlayerSettings.Android.keyaliasName = arguments[ArgumentJobKeyAlias];
			PlayerSettings.Android.keystorePass = arguments[ArgumentJobKeyStorePass];
			PlayerSettings.Android.keyaliasPass = arguments[ArgumentJobKeyAliasPass];
			return buildPlayerOptions;
		}
		
		public override void SetVariables(Dictionary<string, string> variables) {
			base.SetVariables(variables);
			_keyStore = variables[TemplateJobKeyStore];
			_keyAlias = variables[TemplateJobKeyAlias];
			_keyStorePass = variables[TemplateJobKeyStorePass];
			_keyAliasPass = variables[TemplateJobKeyAliasPass];
		}

		protected override string GetBuildPath() {
			return base.GetBuildPath() + ".apk";
		}

		protected override IEnumerable<KeyValuePair<string, string>> GetInternalVariables() {
			foreach (var internalVariable in base.GetInternalVariables()) {
				yield return internalVariable;
			}
			yield return new KeyValuePair<string, string>(TemplateJobUnityModule, BuildTarget.Android.GetUnityModule());
			yield return new KeyValuePair<string, string>(TemplateJobUnityEditorTarget, BuildTarget.Android.GetUnityEditorTarget());
			yield return new KeyValuePair<string, string>(TemplateJobKeyStore, _keyStore);
			yield return new KeyValuePair<string, string>(TemplateJobKeyAlias, _keyAlias);
			yield return new KeyValuePair<string, string>(TemplateJobKeyStorePass, _keyStorePass);
			yield return new KeyValuePair<string, string>(TemplateJobKeyAliasPass, _keyAliasPass);
		}
	}
}