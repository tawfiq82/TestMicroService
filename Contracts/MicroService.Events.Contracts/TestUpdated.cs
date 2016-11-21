using System;

namespace MicroService.Events.Contracts
{
	public class TestUpdated : DomainEvent
	{
		public TestUpdated(Guid id, string name)
		{
			Id = id;
			Name = name;
		}

		public string Name { get; set; }
	}
}
