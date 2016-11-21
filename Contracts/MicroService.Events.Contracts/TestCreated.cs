using System;

namespace MicroService.Events.Contracts
{
	public class TestCreated : DomainEvent
	{

		public TestCreated(Guid id, string name)
		{
			Id = id;
			Name = name;
		}

		public string Name { get; set; }
	}
}
