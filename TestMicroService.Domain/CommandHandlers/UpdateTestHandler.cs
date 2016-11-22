using System.Threading.Tasks;
using DimensionData.Toolset.Cqrs;
using DimensionData.Toolset.Validation;
using TestMicroService.Commands.Contracts;
using TestMicroService.Domain.Exceptions;
using TestMicroService.Domain.Respositories;

namespace TestMicroService.Domain.CommandHandlers
{
	public class UpdateTestHandler : ICommandHandler<UpdateTest>
	{
		private readonly ITestStore _testStore;

		public UpdateTestHandler(ITestStore testStore)
		{
			this._testStore = testStore;
		}

		public async Task Handle(UpdateTest command)
		{
			Check.NotNull(command, nameof(command));

			var test = await _testStore.GetById(command.Id);

			if (test == null)
				throw new TestNotFoundException(command.Id);

			test.Update(command.Id, command.Name, command.Description, command.TestType);
			await this._testStore.Save(test);
		}
	}
}
