using System.Threading.Tasks;
using DimensionData.Toolset.Domain;
using MicroService.Events.Contracts;

namespace TestMicroService.ReadModel.EventHandlers
{
	
	internal sealed class TestCreatedHandler : IDomainEventHandler<TestCreated>
	{
		private readonly ReadModelDataContext _modelContext;

		public TestCreatedHandler(ReadModelDataContext modelContext)
		{
			this._modelContext = modelContext;
		}

		public async Task Handle(TestCreated @event)
		{
			this._modelContext.Tests.Add(new TestMicroService.ReadModel.DataModel.Test()
			{
				Id = @event.Id,
				Name = @event.Name,
				Description = @event.Description,
				TestType = @event.TestType
			});
			await this._modelContext.SaveChangesAsync();
		}

	}
}
