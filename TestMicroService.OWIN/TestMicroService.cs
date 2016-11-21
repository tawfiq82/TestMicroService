using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMicroService.OWIN
{
	using DimensionData.Toolset.Validation;

	using global::TestMicroService.WebApi.App_Start;

	using Microsoft.Owin.Hosting;

	using Serilog;

	using Topshelf;

	internal class TestMicroService : ServiceControl
	{
		private readonly string _baseAddress;
		private IDisposable _serviceHost;

		public TestMicroService(string httpScheme, string hostName, int port)
		{
			Check.NotNullOrWhiteSpace(httpScheme, nameof(httpScheme));
			Check.NotNullOrWhiteSpace(hostName, nameof(hostName));
			Check.Requires(httpScheme, s => s == "http" || s == "https", nameof(httpScheme));

			_baseAddress = $"{httpScheme}://{hostName}:{port}";
		}

		public bool Start(HostControl hostControl)
		{
			try
			{
				_serviceHost = WebApp.Start<Startup>(_baseAddress);
				return true;
			}
			catch (Exception ex)
			{
				Log.Logger.Fatal(ex, ex.ToString());
				throw;
			}
		}

		public bool Stop(HostControl hostControl)
		{
			try
			{
				_serviceHost?.Dispose();
				_serviceHost = null;
				return true;
			}
			catch (Exception ex)
			{
				Log.Logger.Fatal(ex, ex.ToString());
				throw;
			}
		}
	}
}
