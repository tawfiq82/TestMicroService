using System.Threading.Tasks;

namespace TestMicroService.ReadModel.EventHandlers
{
	using DimensionData.Toolset.Domain;

	using MicroService.Events.Contracts;

	internal sealed class TestCreatedHandler : IDomainEventHandler<TestCreated>
	{
		private readonly ReadModelDataContext _modelContext;

		public TestCreatedHandler(ReadModelDataContext modelContext)
		{
			this._modelContext = modelContext;
		}

		public async Task Handle(TestCreated @event)
		{
			this._modelContext.Tests.Add(new TestMicroService.ReadModel.DataModel.Test() { Id = @event.Id, Name = @event.Name });
			await this._modelContext.SaveChangesAsync();
		}

	}
}
