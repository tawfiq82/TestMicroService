using System.Threading.Tasks;
using System.Data.Entity;
using DimensionData.Toolset.Domain;
using MicroService.Events.Contracts;

namespace TestMicroService.ReadModel.EventHandlers
{
	internal sealed class TestUpdatedHandler : IDomainEventHandler<TestUpdated>
	{
		private readonly ReadModelDataContext _modelContext;

		public TestUpdatedHandler(ReadModelDataContext modelContext)
		{
			this._modelContext = modelContext;
		}

		public async Task Handle(TestUpdated @event)
		{
			var test = await this._modelContext.Tests.FirstOrDefaultAsync(x => x.Id == @event.Id);

			if (test == null) return;

			test.Name = @event.Name;
			test.Description = @event.Description;
			test.TestType = @event.TestType;

			await this._modelContext.SaveChangesAsync();
		}

	}
}
