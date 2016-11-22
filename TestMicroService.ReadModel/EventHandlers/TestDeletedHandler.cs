using System.Threading.Tasks;
using System.Data.Entity;
using DimensionData.Toolset.Domain;
using MicroService.Events.Contracts;

namespace TestMicroService.ReadModel.EventHandlers
{
	using AutoMapper.Execution;

	internal sealed class TestDeletedHandler : IDomainEventHandler<TestDeleted>
	{
		private readonly ReadModelDataContext _modelContext;

		public TestDeletedHandler(ReadModelDataContext modelContext)
		{
			this._modelContext = modelContext;
		}

		public async Task Handle(TestDeleted @event)
		{
			var test = await this._modelContext.Tests.FirstOrDefaultAsync(x => x.Id == @event.Id);

			if (test == null) return;

			test.IsDeleted = true;
			await this._modelContext.SaveChangesAsync();
		}
	}
}
