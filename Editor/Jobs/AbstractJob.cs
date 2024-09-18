using CiWizard.Editor.Jobs.Common;
using UnityEngine;

namespace CiWizard.Editor.Jobs {
	public abstract class AbstractJob : ScriptableObject {
		[Header("General")]
		[SerializeField]
		private JobCondition _when;
		[SerializeField]
		private string _branches = "main";
		[SerializeField]
		private JobCachePolicy _cachePolicy;
		[SerializeField]
		private ArtifactCondition _cacheWhen;
		[SerializeField]
		private JobCache _cache;
		[SerializeField]
		private JobArtifacts _artifacts;
		
		public abstract string TemplateFileName { get; }
		public virtual JobCondition When => _when;
		public virtual string Branches => _branches.Replace("/", "\\/");

		public JobCachePolicy CachePolicy => _cachePolicy;
		public ArtifactCondition CacheWhen => _cacheWhen;

		public JobCache Cache => _cache;
		public JobArtifacts Artifacts => _artifacts;

		protected AbstractJob(JobArtifacts artifacts) {
			_artifacts = artifacts;
		}

		protected AbstractJob() { }
	}
}