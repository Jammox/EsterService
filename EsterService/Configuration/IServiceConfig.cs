using System.Collections.Generic;

namespace EsterService.Configuration
{
	public interface IServiceConfig
	{
		List<ConfigurationItem> Items { get; }
		void Load();
	}
}
