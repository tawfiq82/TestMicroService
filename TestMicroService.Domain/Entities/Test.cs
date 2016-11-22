using System;
using DimensionData.Toolset.EventSourcing;
using DimensionData.Toolset.Validation;
using MicroService.Events.Contracts;

namespace TestMicroService.Domain.Entities
{
	public class Test : EventSourcedAggregateRoot
	{
		/// <summary>
		///     The name of the organization
		/// </summary>
		public string Name { get; private set; }

		public bool IsDeleted { get; private set; }

		private Test()
		{

		}
		public Test(string name) : this()
		{
			Check.NotNullOrWhiteSpace(name, nameof(name));

			ApplyChange(new TestCreated(Guid.NewGuid(), name));
		}

		public void Update(Guid testId, string name)
		{
			Check.NotNullOrWhiteSpace(name, nameof(name));
			ApplyChange(new TestUpdated(testId, name));
		}

		public void Delete(Guid testId)
		{
			ApplyChange(new TestDeleted(testId));
		}

		private void Apply(TestCreated @event)
		{
			Id = @event.Id;
			Name = @event.Name;
		}

		private void Apply(TestUpdated @event)
		{
			Name = @event.Name;
		}

		private void Apply(TestDeleted @event)
		{
			IsDeleted = @event.IsDeleted;
		}
	}
}
