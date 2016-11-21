using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DimensionData.Toolset.Domain;
using DimensionData.Toolset.EventSourcing;
using DimensionData.Toolset.Serialization;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using TestMicroService.Toolset.Azure.DocumentDb.EventSourcing;

namespace TestMicroService.Toolset.Azure.DocumentDb
{
    public class DocumentDbEventStore<TAggregateRoot, TEventSet> : IEventStore<TAggregateRoot>
           where TAggregateRoot : EventSourcedAggregateRoot
           where TEventSet : DocumentDbDomainEvent
    {
        private readonly IStringSerializer _serializer = new JsonObjectSerializer();

        // TODO, use IOC instead of hard-coded DB collection URI
        private readonly Uri _databaseCollectionUri = UriFactory.CreateDocumentCollectionUri("TestDB", "dd-cloud");

        /// <summary>
        ///     Initializes a new instance of the <see cref="DocumentDbEventStore{TAggregateRoot, TEventSet}" /> class.
        /// </summary>
        /// <param name="client">The document DB client.</param>
        public DocumentDbEventStore(DocumentClient client)
        {
            Client = client;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DocumentDbEventStore{TAggregateRoot, TEventSet}" /> class.
        /// </summary>
        /// <param name="client">The document DB client.</param>
        /// <param name="domainEventBus">The domain event bus.</param>
        protected DocumentDbEventStore(DocumentClient client, IDomainEventBus domainEventBus)
        {
            Client = client;
            DomainEventBus = domainEventBus;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DocumentDbEventStore{TAggregateRoot, TEventSet}" /> class.
        /// </summary>
        /// <param name="client">The document DB client.</param>
        /// <param name="snapshotRepository">The snapshot repository.</param>
        protected DocumentDbEventStore(DocumentClient client, ISnapshotRepository<TAggregateRoot> snapshotRepository)
        {
            Client = client;
            SnapshotRepository = snapshotRepository;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DocumentDbEventStore{TAggregateRoot, TEventSet}" /> class.
        /// </summary>
        /// <param name="client">The document DB client.</param>
        /// <param name="domainEventBus">The domain event bus.</param>
        /// <param name="snapshotRepository">The snapshot repository.</param>
        protected DocumentDbEventStore(DocumentClient client, IDomainEventBus domainEventBus,
            ISnapshotRepository<TAggregateRoot> snapshotRepository)
        {
            Client = client;
            DomainEventBus = domainEventBus;
            SnapshotRepository = snapshotRepository;
        }

        /// <summary>
        ///     Gets the document DB client.
        /// </summary>
        protected DocumentClient Client { get; }

        /// <summary>
        ///     Gets the domain event bus.
        /// </summary>
        /// <value>The domain event bus.</value>
        protected IDomainEventBus DomainEventBus { get; }

        /// <summary>
        ///     Gets the snapshot repository.
        /// </summary>
        /// <value>The snapshot repository.</value>
        protected ISnapshotRepository<TAggregateRoot> SnapshotRepository { get; }

        /// <summary>
        /// Gets a value indicating whether this event store can publish events.
        /// </summary>
        /// <value>
        /// <c>true</c> if this event store can publish events; otherwise, <c>false</c>.
        /// </value>
        protected bool CanPublishEvents => DomainEventBus != null;

        /// <summary>
        /// Gets a value indicating whether this event store can process snapshots.
        /// </summary>
        /// <value>
        /// <c>true</c> if this event store can process snapshots; otherwise, <c>false</c>.
        /// </value>
        protected bool CanProcessSnapshots => SnapshotRepository != null;

        /// <summary>
        /// Checks that if the given aggregate root has been updated in the event store by another process.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root.</param>
        /// <returns></returns>
        /// <exception cref="AggregateConcurrencyException"></exception>
        protected async Task ConcurrencyCheck(TAggregateRoot aggregateRoot)
        {
            IDocumentQuery<TEventSet> eventQuery = Client
                .CreateDocumentQuery<TEventSet>(_databaseCollectionUri)
                .Where(eventSet => eventSet.AggregateId == aggregateRoot.Id)
                .AsDocumentQuery();

            FeedResponse<TEventSet> result = await eventQuery.ExecuteNextAsync<TEventSet>();

            int persistedVersion = result.Max(eventSet => (int?)eventSet.AggregateVersion) ?? 0;
            if (aggregateRoot.Version != persistedVersion + 1)
            {
                string errorMessage =
                    $"The aggregate with aggregate root of type {aggregateRoot.GetType().Name} has been modified and the event stream cannot be appended.";
                throw new AggregateConcurrencyException(errorMessage);
            }
        }

        public async Task Save(TAggregateRoot aggregateRoot)
        {
            // Check for concurrency issues
            await ConcurrencyCheck(aggregateRoot);

            // Persist Events
            foreach (var uncommittedEvent in aggregateRoot.UncommittedEvents)
            {
                TEventSet @event = (TEventSet)Activator.CreateInstance(typeof(TEventSet), uncommittedEvent);
                await Client.CreateDocumentAsync(_databaseCollectionUri, @event);
            }

            // Check if snapshot is required
            bool snapshotRequired = CanProcessSnapshots &&
                aggregateRoot.UncommittedEvents.Any(
                    eventWrapper => eventWrapper.Sequence % SnapshotRepository.SnapshotInterval == 0
                );
            if (snapshotRequired)
            {
                await SnapshotRepository.SaveSnapshot(aggregateRoot);
            }

            // Publish events if possible
            if (CanPublishEvents)
            {
                foreach (var uncommittedEvent in aggregateRoot.UncommittedEvents)
                {
                    await DomainEventBus.Publish(uncommittedEvent.Event);
                }
            }

            // Mark aggregate root as committed
            aggregateRoot.MarkAsCommitted();
        }

        public async Task<TAggregateRoot> GetById(Guid id)
        {
            TAggregateRoot aggregateRoot = null;
            IEnumerable<DomainEventWrapper> eventWrappers;

            if (CanProcessSnapshots)
            {
                var result = await SnapshotRepository.GetLatestSnapshot(id);
                var lastEventSequence = 0;
                if (result != null)
                {
                    aggregateRoot = result.Item1;
                    lastEventSequence = result.Item2;
                }
                eventWrappers = await GetAggregateEvents(id, lastEventSequence);
            }
            else
            {
                eventWrappers = await GetAggregateEvents(id);
            }

            if (aggregateRoot == null && !eventWrappers.Any())
            {
                return null;
            }

            if (aggregateRoot == null)
            {
                aggregateRoot = CreateInstanceFromPrivateConstructor();
            }

            aggregateRoot.Replay(eventWrappers);

            return aggregateRoot;
        }

        public async Task<IEnumerable<DomainEventWrapper>> GetAggregateEvents(Guid aggregateRootId)
        {
            IDocumentQuery<TEventSet> eventQuery = Client
                .CreateDocumentQuery<TEventSet>(_databaseCollectionUri)
                .Where(f => f.AggregateId == aggregateRootId)
                .AsDocumentQuery();

            FeedResponse<TEventSet> result = await eventQuery.ExecuteNextAsync<TEventSet>();

            return result.Select(GetDomainEventWrapperFromEvent);
        }

        public async Task<IEnumerable<DomainEventWrapper>> GetAggregateEvents(Guid aggregateRootId, int startSequenceNumber)
        {
            IDocumentQuery<TEventSet> eventQuery = Client
                .CreateDocumentQuery<TEventSet>(_databaseCollectionUri)
                .Where(eventSet => eventSet.AggregateId == aggregateRootId && eventSet.Sequence > startSequenceNumber)
                .AsDocumentQuery();

            FeedResponse<TEventSet> result = await eventQuery.ExecuteNextAsync<TEventSet>();

            return result.Select(GetDomainEventWrapperFromEvent);
        }

        public async Task<IEnumerable<DomainEventWrapper>> GetAggregateEvents<TDomainEvent>(Guid aggregateRootId)
            where TDomainEvent : IDomainEvent
        {
            string domainEventName = typeof(TDomainEvent).FullName;
            IDocumentQuery<TEventSet> eventQuery = Client
                .CreateDocumentQuery<TEventSet>(_databaseCollectionUri)
                .Where(eventSet => eventSet.AggregateId == aggregateRootId && eventSet.Name.Equals(domainEventName))
                .AsDocumentQuery();

            FeedResponse<TEventSet> result = await eventQuery.ExecuteNextAsync<TEventSet>();

            return result.Select(GetDomainEventWrapperFromEvent);
        }

        public async Task<IEnumerable<DomainEventWrapper>> GetAggregateEvents(Guid aggregateRootId, IEnumerable<Type> domainEventTypes)
        {
            HashSet<string> domainEventTypeNames = new HashSet<string>(domainEventTypes.Select(t => t.FullName));
            IDocumentQuery<TEventSet> eventQuery = Client
                .CreateDocumentQuery<TEventSet>(_databaseCollectionUri)
                .Where(eventSet => eventSet.AggregateId == aggregateRootId && domainEventTypeNames.Contains(eventSet.Name))
                .AsDocumentQuery();

            FeedResponse<TEventSet> result = await eventQuery.ExecuteNextAsync<TEventSet>();

            return result.Select(GetDomainEventWrapperFromEvent);
        }

        private DomainEventWrapper GetDomainEventWrapperFromEvent(TEventSet @event)
        {
            var domainEvent = _serializer.Deserialize<IDomainEvent>(@event.Payload);
            return new DomainEventWrapper(@event.EventId, @event.AggregateId, @event.Sequence, @event.AggregateVersion,
                domainEvent);
        }

        private static TAggregateRoot CreateInstanceFromPrivateConstructor()
        {
            var aggregateRootType = typeof(TAggregateRoot);
            var constructor = aggregateRootType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null,
                new Type[0], null);

            if (constructor == null)
                throw new InvalidOperationException($"{aggregateRootType.FullName} must have a constructor with no parameters.");

            return (TAggregateRoot)constructor.Invoke(new object[0]);
        }
    }
}
