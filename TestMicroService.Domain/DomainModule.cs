using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMicroService.Domain
{
	using DimensionData.Toolset.Cqrs.Decorators;
	using DimensionData.Toolset.Cqrs.Messaging;
	using DimensionData.Toolset.DependencyInjection;

	using MicroService.Events.Contracts;

	using TestMicroService.Commands.Contracts;
	using TestMicroService.Domain.CommandHandlers;

	public class DomainModule : IModule
	{
		public static void SubscribeToSystemEvents(IHostConfiguration host)
		{
			if (host == null)
			{
				throw new ArgumentNullException(nameof(host));
			}
		}

		public void Initialize(IDependencyContainerBuilder builder)
		{
			RegisterCommandHandlers(builder);
		}

		private static void RegisterCommandHandlers(IDependencyContainerBuilder builder)
		{
			var commonDecorators = new[]
			{
				// typeof(LogCommandDecorator<>),
				// typeof(AuthorizationCommandDecorator<>),
				typeof(ValidationCommandDecorator<>),
				// typeof(ReadCommittedTransactionCommandDecorator<>)
			};

			// builder.RegisterCommandHandlerWithDecorators<CreateTest, CreateTestHandler>(commonDecorators);
		}
	}
}
