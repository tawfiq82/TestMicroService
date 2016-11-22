namespace TestMicroService.WebApi.Model
{
	using TestMicroService.Shared.Contracts.Enums;

	public class TestConfig
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public TestType TestType { get; set; }
	}
}
