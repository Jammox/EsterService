using Ninject.Modules;

namespace EsterService.Configuration
{
	public class SettingsModule : NinjectModule
	{
		public override void Load()
		{
			Bind<ISettings>().To<AppSettings>()
				.InSingletonScope();
		}
	}
}
