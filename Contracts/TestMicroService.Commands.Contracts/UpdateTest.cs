using System;

namespace TestMicroService.Commands.Contracts
{
	using TestMicroService.Shared.Contracts.Enums;

	public class UpdateTest : Command
	{
		public Guid Id { get; internal set; }

		public string Name { get; internal set; }

		public string Description { get; internal set; }

		public TestType TestType { get; internal set; }

		public UpdateTest(Guid id, string name, string description, TestType testType)
		{
			Id = id;
			Name = name;
			Description = description;
			TestType = testType;
		}
	}
}
