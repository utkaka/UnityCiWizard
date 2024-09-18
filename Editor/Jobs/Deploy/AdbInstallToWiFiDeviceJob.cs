using CiWizard.Editor.Jobs.Common;
using UnityEngine;

namespace CiWizard.Editor.Jobs.Deploy {
    [CreateAssetMenu(fileName = "AdbInstallToWiFiDevice", menuName = "CI/Jobs/Deploy/Adb install to wifi device")]
    public class AdbInstallToWiFiDeviceJob : AdbInstallToWiredDevicesJob {
        public override string TemplateFileName => "Jobs/Deploy/AdbInstallToWiFiDevice";
        public override JobCondition When => JobCondition.Manual;
    }
}