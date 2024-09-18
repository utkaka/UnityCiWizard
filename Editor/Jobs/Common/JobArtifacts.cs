using System;
using UnityEngine;

namespace CiWizard.Editor.Jobs.Common {
    [Serializable]
    public class JobArtifacts {
        [SerializeField]
        private ArtifactCondition _when;
        [SerializeField] 
        private string[] _paths;
        public ArtifactCondition When => _when;
        public string[] Paths => _paths;

        public JobArtifacts(ArtifactCondition when, string[] paths) {
            _when = when;
            _paths = paths;
        }
    }
}