using DimensionData.Toolset.Domain;
using Microsoft.Azure.Documents.Client;
using TestMicroService.Domain.Entities;
using TestMicroService.Domain.Respositories;
using TestMicroService.DomainData.DataModel;
using TestMicroService.Toolset.Azure.DocumentDb;

namespace TestMicroService.DomainData.Respositories
{
	/// <summary>
	/// This is the entity framework event store for the <see cref="Test"/> aggregate root.
	/// </summary>
	/// <seealso cref="DocumentDbEventStore{TAggregateRoot,TEventSet}" />
	/// <seealso cref="ITestStore" />
	public class TestStore :
		DocumentDbEventStore<Test, TestEvent>,
		ITestStore
	{
		// TODO: Add support for snapshot
		// public TestStore(DocumentClient client, IDomainEventBus domainEventBus, ISnapshotRepository<Test> snapshotRepository) : base(client, domainEventBus, snapshotRepository)
		public TestStore(DocumentClient client, IDomainEventBus domainEventBus) : base(client, domainEventBus)
		{
		}
	}
}
