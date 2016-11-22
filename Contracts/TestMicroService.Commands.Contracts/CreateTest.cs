namespace TestMicroService.Commands.Contracts
{
	using TestMicroService.Shared.Contracts.Enums;

	public class CreateTest : Command
	{
		public string Name { get; internal set; }

		public string Description { get; internal set; }

		public TestType TestType { get; internal set; }

		public CreateTest(string name, string description, TestType testType)
		{
			Name = name;
			Description = description;
			TestType = testType;
		}
	}
}
