using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMicroService.Queries.Contracts.Responses
{
	using DimensionData.Toolset.Cqrs;

	using TestMicroService.Queries.Contracts.Dtos;

	public class QueryTestsResponse : PagedQueryResponse<Test>
	{
	}
}
