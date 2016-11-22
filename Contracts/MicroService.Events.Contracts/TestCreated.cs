using System;

namespace MicroService.Events.Contracts
{
	using TestMicroService.Shared.Contracts.Enums;

	public class TestCreated : DomainEvent
	{
		public TestCreated(Guid id, string name, string description, TestType testType)
		{
			Id = id;
			Name = name;
			Description = description;
			TestType = testType;
		}

		public string Name { get; set; }

		public string Description { get; internal set; }

		public TestType TestType { get; internal set; }
	}
}
