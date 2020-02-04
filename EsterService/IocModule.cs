using Common.Logging;
using EsterService.Configuration;
using EsterService.Scheduling;
using EsterService.Service;
using Ninject.Modules;
using System;

namespace EsterService
{
	class IocModule : NinjectModule
	{
		public override void Load()
		{
			Bind<ILog>().ToMethod(ctx =>
				{
					Type type = ctx.Request.ParentContext.Request.Service;
					return LogManager.GetLogger(type);
				});

			Bind<IServiceConfig>().To<EsterConfig>()
				.InSingletonScope();

			Bind<IThreadScheduler>().To<ThreadScheduler>()
				.InSingletonScope();

			Bind<IProcessController>().To<ProcessController>();

			Bind<Service.WinService>().ToSelf();
		}
	}
}

