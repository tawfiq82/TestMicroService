using System;
using DimensionData.Toolset.EventSourcing;
using DimensionData.Toolset.Validation;
using Newtonsoft.Json;
using TestMicroService.Toolset.Azure.DocumentDb.EventSourcing;

namespace TestMicroService.Toolset.Azure.DocumentDb
{
    /// <summary>
    ///     An abstract base class for domain events using Entity Framework data context.
    /// </summary>
    public abstract class DocumentDbDomainEvent
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DocumentDbDomainEvent" /> class.
        /// </summary>
        protected DocumentDbDomainEvent()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DocumentDbDomainEvent" /> class.
        /// </summary>
        /// <param name="eventWrapper">The domain eventWrapper.</param>
        protected DocumentDbDomainEvent(DomainEventWrapper eventWrapper)
        {
            Check.NotNull(eventWrapper, nameof(eventWrapper));

            var jsonObjectSerializer = new JsonObjectSerializer();

            EventId = eventWrapper.EventId;
            Name = eventWrapper.EventName;
            AggregateId = eventWrapper.AggregateId;
            Sequence = eventWrapper.Sequence;
            CreatedOnUtc = eventWrapper.CreatedOnUtc;
            CreatedBy = eventWrapper.CreatedBy;
            Payload = jsonObjectSerializer.Serialize(eventWrapper.Event);
            AggregateVersion = eventWrapper.AggregateVersion;
        }

        /// <summary>
        ///     Gets or sets the unique identifier of the eventWrapper.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public Guid EventId { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the unique identifier of the aggregate.
        /// </summary>
        public Guid AggregateId { get; set; }

        /// <summary>
        ///     Gets or sets the sequence.
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        ///     Gets the aggregate version.
        /// </summary>
        public int AggregateVersion { get; private set; }

        /// <summary>
        ///     Gets or sets the payload.
        /// </summary>
        public string Payload { get; set; }

        /// <summary>
        ///     Gets or sets the date and time when this instance was created (UTC).
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        ///     Gets or sets the name of the user who caused the event.
        /// </summary>
        public string CreatedBy { get; set; }
    }
}
