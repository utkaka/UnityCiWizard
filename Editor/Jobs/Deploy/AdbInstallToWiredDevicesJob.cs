using System;
using CiWizard.Editor.Jobs.Build.Targets;
using CiWizard.Editor.Jobs.Common;
using UnityEngine;

namespace CiWizard.Editor.Jobs.Deploy {
    [CreateAssetMenu(fileName = "AdbInstallToWiredDevices", menuName = "CI/Jobs/Deploy/Adb install to wired devices")]
    public class AdbInstallToWiredDevicesJob : AbstractJob {
        [SerializeField]
        private AndroidApkBuildJob _apkToInstall;

        public override string Branches => _apkToInstall.Branches;
        public override string TemplateFileName => "Jobs/Deploy/AdbInstallToWiredDevices";

        public AndroidApkBuildJob ApkToInstall => _apkToInstall;

        public AdbInstallToWiredDevicesJob() : base(new JobArtifacts(ArtifactCondition.OnSuccess, Array.Empty<string>())) { }
    }
}