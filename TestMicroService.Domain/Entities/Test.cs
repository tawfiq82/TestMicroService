using System;

namespace TestMicroService.Domain.Entities
{
	using DimensionData.Toolset.EventSourcing;
	using DimensionData.Toolset.Validation;

	using MicroService.Events.Contracts;

	public class Test : EventSourcedAggregateRoot
	{

		/// <summary>
		///     The name of the organization
		/// </summary>
		public string Name { get; private set; }

		private Test()
		{

		}
		public Test(string name) : this()
		{
			Check.NotNullOrWhiteSpace(name, nameof(name));

			ApplyChange(new TestCreated(Guid.NewGuid(), name));
		}

		private void Apply(TestCreated @event)
		{
			Id = @event.Id;
			Name = @event.Name;
		}
	}
}
