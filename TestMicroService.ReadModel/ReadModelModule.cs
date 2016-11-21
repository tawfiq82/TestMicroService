namespace TestMicroService.ReadModel
{
	using DimensionData.Toolset.Cqrs.Decorators;
	using DimensionData.Toolset.DependencyInjection;

	using MicroService.Events.Contracts;

	using TestMicroService.Queries.Contracts.Queries;
	using TestMicroService.Queries.Contracts.Responses;
	using TestMicroService.ReadModel.EventHandlers;
	using TestMicroService.ReadModel.QueryHandlers;

	public class ReadModelModule : IModule
	{
		public void Initialize(IDependencyContainerBuilder builder)
		{
			RegisterDataContext(builder);
			RegisterQueryHandlers(builder);
			RegisterEventHandlers(builder);
		}

		private static void RegisterDataContext(IDependencyContainerBuilder builder)
		{
			builder.RegisterExecutionContextScope<ReadModelDataContext>();
		}

		private void RegisterEventHandlers(IDependencyContainerBuilder builder)
		{
			builder.RegisterDomainEventHandler<TestCreated, TestCreatedHandler>();
		}

		private void RegisterQueryHandlers(IDependencyContainerBuilder builder)
		{
			var commonDecorators = new[]
		   {
				typeof(LogQueryDecorator<,>),
				typeof(AuthorizationQueryDecorator<,>),
				typeof(ValidationQueryDecorator<,>)
			};

			builder
				.RegisterQueryHandlerWithDecorators
				<QueryTests, QueryTestsResponse, QueryTestsHandler>(commonDecorators);

		}
	}
}
