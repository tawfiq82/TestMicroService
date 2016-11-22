using System;
using DimensionData.Toolset.EventSourcing;
using DimensionData.Toolset.Validation;
using MicroService.Events.Contracts;

namespace TestMicroService.Domain.Entities
{
	using TestMicroService.Shared.Contracts.Enums;

	public class Test : EventSourcedAggregateRoot
	{
		/// <summary>
		///     The name of the organization
		/// </summary>
		public string Name { get; private set; }

		public bool IsDeleted { get; private set; }

		public string Description { get; private set; }

		public TestType TestType { get; private set; }

		private Test()
		{

		}
		public Test(string name, string description, TestType testType) : this()
		{
			Check.NotNullOrWhiteSpace(name, nameof(name));

			ApplyChange(new TestCreated(Guid.NewGuid(), name, description, testType));
		}

		public void Update(Guid testId, string name, string description, TestType testType)
		{
			Check.NotNullOrWhiteSpace(name, nameof(name));
			ApplyChange(new TestUpdated(testId, name, description, testType));
		}

		public void Delete(Guid testId)
		{
			ApplyChange(new TestDeleted(testId));
		}

		private void Apply(TestCreated @event)
		{
			Id = @event.Id;
			Name = @event.Name;
			Description = @event.Description;
			TestType = @event.TestType;
		}

		private void Apply(TestUpdated @event)
		{
			Name = @event.Name;
			Description = @event.Description;
			TestType = @event.TestType;
		}

		private void Apply(TestDeleted @event)
		{
			IsDeleted = @event.IsDeleted;
		}
	}
}
