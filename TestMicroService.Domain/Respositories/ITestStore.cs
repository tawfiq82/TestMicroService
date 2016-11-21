using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMicroService.Domain.Respositories
{
	using DimensionData.Toolset.EventSourcing;

	using TestMicroService.Domain.Entities;

	public interface ITestStore : IEventStore<Test>
	{
	}
}
