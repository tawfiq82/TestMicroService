using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMicroService.WebApi.Controllers
{
	using System.Net;
	using System.Web.Http;

	using DimensionData.Toolset.Cqrs;
	using DimensionData.Toolset.Querying;
	using DimensionData.Toolset.WebApi.Swagger;

	using Swashbuckle.Swagger.Annotations;

	using TestMicroService.Commands.Contracts;
	using TestMicroService.Queries.Contracts.Queries;
	using TestMicroService.Queries.Contracts.Responses;
	using TestMicroService.WebApi.Model;

	[RoutePrefix("api/testmicroservice/1.0/tests")]
	public class TestController : ApiController
	{
		private readonly ICommandBus _commandBus;
		private readonly IQueryProcessor _queryProcessor;

		public TestController(ICommandBus commandBus, IQueryProcessor queryProcessor)
		{
			_commandBus = commandBus;
			_queryProcessor = queryProcessor;
		}

		[HttpPost]
		public async Task<IHttpActionResult> CreateTest(TestConfig config)
		{
			await _commandBus.Send(new CreateTest(config.Name));
			return StatusCode(HttpStatusCode.Created);
		}

		[HttpGet]
		[Route("")]
		public async Task<QueryTestsResponse> QueryTests()
		{
			var query = new QueryTests(Request.GetQueryOptions());
			return await _queryProcessor.ProcessQuery<QueryTests, QueryTestsResponse>(query);
		}
	}
}
