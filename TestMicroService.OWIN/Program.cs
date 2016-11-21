using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMicroService.OWIN
{
	using DimensionData.Toolset.SerilogLogger;

	using global::TestMicroService.WebApi.App_Start;

	using Serilog.Events;

	using Topshelf;

	class Program
	{
		static void Main(string[] args)
		{
			SerilogLoggerFactory.CreateDimensionDataConsoleLogger(Startup.ServiceName + "_Host", LogEventLevel.Verbose);

			string protocol = "http";
			string hostname = "+";
			int port = 16888;

			HostFactory.Run(
				hostConfig =>
					{
						hostConfig.UseSerilog();

						hostConfig.AddCommandLineDefinition("protocol", arg => protocol = arg);
						hostConfig.AddCommandLineDefinition("hostname", arg => hostname = arg);
						hostConfig.AddCommandLineDefinition("port", arg => port = int.Parse(arg));
						hostConfig.ApplyCommandLine();


						hostConfig.Service<TestMicroService>(() => new TestMicroService(protocol, hostname, port));

						hostConfig.RunAsNetworkService();
						hostConfig.SetServiceName("TestMicroService");
						hostConfig.SetDisplayName("Dimension Data - " + Startup.ServiceDescription);
						hostConfig.SetDescription("Dimension Data - " + Startup.ServiceDescription);
					});

		}
	}
}