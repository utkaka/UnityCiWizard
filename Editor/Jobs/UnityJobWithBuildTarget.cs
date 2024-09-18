using CiWizard.Editor.Jobs.Build;
using CiWizard.Editor.Jobs.Common;
using UnityEditor;

namespace CiWizard.Editor.Jobs {
    public abstract class UnityJobWithBuildTarget : UnityJob {
        protected abstract BuildTarget JobBuildTarget { get; }
        
        public override string TemplateFileName => "Jobs/UnityJobWithBuildTarget";
        
        public string UnityModule => JobBuildTarget.GetUnityModule();
        public string UnityEditorTarget => JobBuildTarget.GetUnityEditorTarget();
        public string RunnerTag => JobBuildTarget.GetJobRunnerTag();
        
        protected UnityJobWithBuildTarget(JobArtifacts artifacts) : base(artifacts) { }
    }
}