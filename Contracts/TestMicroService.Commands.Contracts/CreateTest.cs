namespace TestMicroService.Commands.Contracts
{
	public class CreateTest : Command
	{
		public string Name { get; internal set; }

		public CreateTest(string name)
		{
			Name = name;
		}
	}
}
