using System;

namespace TestMicroService.Commands.Contracts
{
	public class DeleteTest : Command
	{
		public Guid Id { get; internal set; }

		public DeleteTest(Guid id)
		{
			Id = id;
		}
	}
}
