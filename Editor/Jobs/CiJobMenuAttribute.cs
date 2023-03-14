using System;

namespace UnityCiWizard.Editor.Jobs {
	public class CiJobMenuAttribute : Attribute{
		public string JobMenuName { get; }
		public int Order { get; }

		public CiJobMenuAttribute(string jobMenuName, int order) {
			JobMenuName = jobMenuName;
			Order = order;
		}
	}
}