using System;

namespace MicroService.Events.Contracts
{
	using DimensionData.Toolset.Domain;

	public abstract class DomainEvent : IDomainEvent
	{
		protected DomainEvent()
		{
			CreatedOnUtc = DateTime.UtcNow;
		}

		/// <summary>
		/// Gets the UC time when this event was created.
		/// </summary>
		/// <value>
		/// The created on UTC.
		/// </value>
		public DateTime CreatedOnUtc { get; }

		/// <summary>
		/// The unique identifier for the domain object
		/// </summary>
		/// <value>
		/// The unique identifier
		/// </value>
		public Guid Id { get; set; }
	}
}
