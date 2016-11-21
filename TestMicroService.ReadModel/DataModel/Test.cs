using System;

namespace TestMicroService.ReadModel.DataModel
{
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("Client", Schema = ReadModelDataContext.Schema)]
	internal class Test
	{
		[Key]
		public Guid Id { get; set; }

		public string Name { get; set; }
	}
}
