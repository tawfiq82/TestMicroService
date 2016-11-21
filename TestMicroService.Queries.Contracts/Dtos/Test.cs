using System;

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
	}
}
