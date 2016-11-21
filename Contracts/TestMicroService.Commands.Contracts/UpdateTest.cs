using System;

namespace TestMicroService.Commands.Contracts
{
	public class UpdateTest : Command
	{
		public Guid Id { get; internal set; }

		public string Name { get; internal set; }

		public UpdateTest(Guid id, string name)
		{
			Id = id;
			Name = name;
		}
	}
}
