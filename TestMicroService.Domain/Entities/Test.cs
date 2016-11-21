using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

			ApplyChange(new TestCreated(name));
		}

		private void Apply(TestCreated @event)
		{
			Id = Guid.NewGuid();
			Name = @event.Name;
		}
	}
}
