using System;
using System.Configuration;
using DimensionData.Toolset.DependencyInjection;
using Microsoft.Azure.Documents.Client;
using TestMicroService.Domain.Respositories;
using TestMicroService.DomainData.Respositories;

namespace TestMicroService.DomainData
{
	public class DomainDataModule
		: IModule
	{
		public void Initialize(IDependencyContainerBuilder builder)
		{
			if (builder == null)
				throw new ArgumentNullException(nameof(builder));

			Uri endpoint = new Uri(ConfigurationManager.AppSettings["DocumentDbEndpoint"]);
			string key = ConfigurationManager.AppSettings["DocumentDbKey"];
			DocumentClient client = new DocumentClient(endpoint, key);
			builder.RegisterInstance(typeof(DocumentClient), client);

			builder.RegisterExecutionContextScope<ITestStore, TestStore>();
		}
	}
}
