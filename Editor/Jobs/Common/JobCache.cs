using UnityEngine;

namespace CiWizard.Editor.Jobs.Common {
    [CreateAssetMenu(fileName = "Cache", menuName = "CI/Cache")]
    public class JobCache : ScriptableObject {
        [SerializeField] 
        private string[] _paths = { "Library", "BuildCache" };
        public string[] Paths => _paths;
    }
}