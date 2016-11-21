using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMicroService.Commands.Contracts
{
	public class CreateTest : Command
	{
		public string Name { get; internal set; }

		public CreateTest(string name)
		{
			Name = name;
		}
	}
}
