using System;
using System.Threading.Tasks;
using System.Net;
using System.Web.Http;
using DimensionData.Toolset.Cqrs;
using DimensionData.Toolset.Querying;
using TestMicroService.Commands.Contracts;
using TestMicroService.Queries.Contracts.Queries;
using TestMicroService.Queries.Contracts.Responses;
using TestMicroService.WebApi.Model;

namespace TestMicroService.WebApi.Controllers
{
	/// <summary>
	/// The API controller for managing test.
	/// </summary>
	[AllowAnonymous]
	[RoutePrefix("api/testmicroservice/1.0/tests")]
	public class TestController : ApiController
	{
		private readonly ICommandBus _commandBus;
		private readonly IQueryProcessor _queryProcessor;

		/// <summary>
		/// The public constructor.
		/// </summary>
		/// <param name="commandBus">Injected instance of <see cref="ICommandBus"/>.</param>
		/// <param name="queryProcessor">Injected instance of <see cref="IQueryProcessor"/>.</param>
		public TestController(ICommandBus commandBus, IQueryProcessor queryProcessor)
		{
			_commandBus = commandBus;
			_queryProcessor = queryProcessor;
		}

		/// <summary>
		/// Create a test.
		/// </summary>
		/// <param name="config">Test configuration.</param>
		/// <returns></returns>
		[HttpPost]
		[Route("")]
		public async Task<IHttpActionResult> CreateTest(TestConfig config)
		{
			await _commandBus.Send(new CreateTest(config.Name, config.Description, config.TestType));
			return StatusCode(HttpStatusCode.Created);
		}

		/// <summary>
		/// Update test.
		/// </summary>
		/// <param name="testId">Test unique identifier.</param>
		/// <param name="config">Test configuration.</param>
		/// <returns></returns>
		[HttpPut]
		[Route("{testId}")]
		public async Task<IHttpActionResult> UpdateTest(Guid testId, TestConfig config)
		{
			await _commandBus.Send(new UpdateTest(testId, config.Name, config.Description, config.TestType));
			return StatusCode(HttpStatusCode.Created);
		}

		/// <summary>
		/// Delete the test.
		/// </summary>
		/// <param name="testId">Test unique identifier.</param>
		/// <returns></returns>
		[HttpDelete]
		[Route("{testId}")]
		public async Task<IHttpActionResult> DeleteTest(Guid testId)
		{
			await _commandBus.Send(new DeleteTest(testId));
			return StatusCode(HttpStatusCode.Created);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("{testId}")]
		public async Task<GetTestResponse> GetTest(Guid testId)
		{
			var query = new GetTest(testId);
			return await _queryProcessor.ProcessQuery<GetTest, GetTestResponse>(query);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		//[Route("")]
		public async Task<QueryTestsResponse> QueryTests()
		{
			var query = new QueryTests(Request.GetQueryOptions());
			return await _queryProcessor.ProcessQuery<QueryTests, QueryTestsResponse>(query);
		}
	}
}
