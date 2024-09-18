using CiWizard.Editor.Jobs.Common;

namespace CiWizard.Editor.Gitlab {
    public static class GitlabTemplateFunctions {
        public static string CachePolicyToString(JobCachePolicy policy) {
            switch (policy) {
                case JobCachePolicy.Pull:
                    return "pull";
                case JobCachePolicy.Push:
                    return "push";
                case JobCachePolicy.PullPush:
                default:
                    return "pull-push";
            }
        }

        public static string JobConditionToString(JobCondition when) {
            switch (when) {
                case JobCondition.OnFailure:
                    return "on_failure";
                case JobCondition.Always:
                    return "always";
                case JobCondition.Manual:
                    return "manual";
                case JobCondition.OnSuccess:
                default:
                    return "on_success";
            }
        }

        public static string ArtifactConditionToString(ArtifactCondition when) {
            switch (when) {
                case ArtifactCondition.OnFailure:
                    return "on_failure";
                case ArtifactCondition.Always:
                    return "always";
                case ArtifactCondition.OnSuccess:
                default:
                    return "on_success";
            }
        }
    }
}