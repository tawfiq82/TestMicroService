using System;

namespace MicroService.Events.Contracts
{
	public class TestDeleted : DomainEvent
	{
		public TestDeleted(Guid id)
		{
			Id = id;
			IsDeleted = true;
		}

		public bool IsDeleted { get; set; }
	}
}
