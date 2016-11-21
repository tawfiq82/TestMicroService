namespace TestMicroService.Domain.Respositories
{
	using DimensionData.Toolset.EventSourcing;

	using TestMicroService.Domain.Entities;

	public interface ITestStore : IEventStore<Test>
	{
	}
}
