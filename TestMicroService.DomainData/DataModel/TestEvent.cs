using DimensionData.Toolset.EventSourcing;
using TestMicroService.Toolset.Azure.DocumentDb.EventSourcing;

namespace TestMicroService.DomainData.DataModel
{
	/// <summary>
	/// This defines the entity framework entity for EMS events in the database
	/// </summary>
	/// <seealso cref="DocumentDbDomainEvent" />
	public class TestEvent : DocumentDbDomainEvent
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentDbDomainEvent"/> class.
		/// </summary>
		/// <param name="eventWrapper">The domain eventWrapper.</param>
		public TestEvent(DomainEventWrapper eventWrapper)
			: base(eventWrapper)
		{
		}

		/// <summary>
		/// Prevents a default instance of the <see cref="TestEvent"/> class from being created.
		/// </summary>
		private TestEvent()
		{
		}
	}
}
