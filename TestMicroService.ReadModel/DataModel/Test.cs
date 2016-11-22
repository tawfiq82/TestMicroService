using System;

namespace TestMicroService.ReadModel.DataModel
{
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	using TestMicroService.Shared.Contracts.Enums;

	[Table("Client", Schema = ReadModelDataContext.Schema)]
	internal class Test
	{
		[Key]
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public TestType TestType { get; set; }

		public bool IsDeleted { get; set; }
	}
}
