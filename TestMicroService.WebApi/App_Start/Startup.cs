using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMicroService.WebApi.App_Start
{
	using DimensionData.Toolset;
	using DimensionData.Toolset.Consul;
	using DimensionData.Toolset.DependencyInjection;
	using DimensionData.Toolset.Initialization;
	using DimensionData.Toolset.MassTransit;
	using DimensionData.Toolset.SerilogLogger;
	using DimensionData.Toolset.SimpleInjector;
	using DimensionData.Toolset.Vault;
	using DimensionData.Toolset.WebApi;

	using Owin;

	using TestMicroService.Domain;
	using TestMicroService.ReadModel;

	public class Startup
	{
        /// <summary>
        /// The service name.
        /// </summary>
        public const string ServiceName = "Test";

		/// <summary>
		/// The service description.
		/// </summary>
		public const string ServiceDescription = "Test Web API";

		/// <summary>
		/// Configures the application.
		/// </summary>
		/// <param name="app">The OWIN application initializer.</param>
		public void Configuration(IAppBuilder app)
		{
			var appBuilder = new OwinAppInitializer(app);
			Initialize(appBuilder);
			appBuilder.Build();
		}

		/// <summary>
		/// Initializes the application.
		/// </summary>
		/// <param name="app">The application initializer.</param>
		protected virtual void Initialize(IAppInitializer app)
		{
			app.UseSimpleInjector(containerBuilder =>
			{
				containerBuilder.LoadModule<CqrsModule>();
				// containerBuilder.LoadModule<DomainModule>();
				// containerBuilder.LoadModule<DomainDataModule>();
				//containerBuilder.LoadModule<ReadModelModule>();
			});

			app.UseSerilogFileLogger(ServiceName, "test/logging");

			app.UseAppSettingsConfigurationStore();
			app.UseAppSettingsSecureConfigurationStore();
			app.UseMassTransit(config => config.AddInMemoryHost());
		
			app.UseWebApi(config =>
			{
				config.UseSwagger();
			});

			app.UseSimpleInjectorWebApiDependencyResolver();

		}
	}
}
