using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMicroService.Domain.CommandHandlers
{
	using DimensionData.Toolset.Cqrs;
	using DimensionData.Toolset.Validation;

	using TestMicroService.Commands.Contracts;
	using TestMicroService.Domain.Entities;
	using TestMicroService.Domain.Repositories;

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
