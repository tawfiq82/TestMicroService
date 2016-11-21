using System.Threading.Tasks;
using TestMicroService.Domain.Respositories;
using DimensionData.Toolset.Cqrs;
using DimensionData.Toolset.Validation;
using TestMicroService.Commands.Contracts;
using TestMicroService.Domain.Entities;

namespace TestMicroService.Domain.CommandHandlers
{
	public class CreateTestHandler : ICommandHandler<CreateTest>
	{
		private readonly ITestStore _testStore;

		public CreateTestHandler(ITestStore testStore)
		{
			this._testStore = testStore;
		}

		public async Task Handle(CreateTest command)
		{
			Check.NotNull(command, nameof(command));

			var test = new Test(command.Name);
			await this._testStore.Save(test);
		}
	}
}
