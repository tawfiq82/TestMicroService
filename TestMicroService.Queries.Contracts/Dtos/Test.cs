using System;
using TestMicroService.Shared.Contracts.Enums;

namespace TestMicroService.Queries.Contracts.Dtos
{
	public class Test
	{
		/// <summary>
		/// Gets or sets the identifier of the client.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the client.
		/// </summary>
		public string Name { get; set; }


		public string Description { get; set; }

		public TestType TestType { get; set; }

		public bool IsDeleted { get; set; }
	}
}
