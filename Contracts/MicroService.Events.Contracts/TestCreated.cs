using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Events.Contracts
{
    public class TestCreated : DomainEvent
	{
		
		public TestCreated( string name)
		{
			Name = name;
		}
		
		public string Name { get; set; }
    }
}
