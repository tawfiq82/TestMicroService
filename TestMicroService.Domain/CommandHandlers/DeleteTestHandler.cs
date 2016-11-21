using System.Threading.Tasks;
using DimensionData.Toolset.Cqrs;
using DimensionData.Toolset.Validation;
using TestMicroService.Commands.Contracts;
using TestMicroService.Domain.Respositories;
using TestMicroService.Domain.Exceptions;

namespace TestMicroService.Domain.CommandHandlers
{
	public class DeleteTestHandler : ICommandHandler<DeleteTest>
	{
		private readonly ITestStore _testStore;

		public DeleteTestHandler(ITestStore testStore)
		{
			this._testStore = testStore;
		}

		public async Task Handle(DeleteTest command)
		{
			Check.NotNull(command, nameof(command));

			var test = await _testStore.GetById(command.Id);

			if (test == null)
				throw new TestNotFoundException(command.Id);

			if (test.IsDeleted)
				throw new TestAlreadyDeletedException(command.Id);

			test.Delete(command.Id);
			await this._testStore.Save(test);
		}
	}
}
