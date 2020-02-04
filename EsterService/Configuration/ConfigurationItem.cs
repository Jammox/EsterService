using System;

namespace EsterService.Configuration
{
	/// <summary>
	/// Represents a configuration item for the service.
	/// </summary>
	public class ConfigurationItem
	{
		public string Name { get; set; }
		public string File { get; set; }
		public string Path { get; set; }
		public string Options { get; set; }
		public TimeSpan Delay { get; set; }
		public TimeSpan Interval { get; set; }
		public bool Active { get; set; }
	}
}
