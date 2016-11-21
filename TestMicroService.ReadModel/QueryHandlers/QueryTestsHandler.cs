using System.Threading.Tasks;

namespace TestMicroService.ReadModel.QueryHandlers
{
	using AutoMapper;

	using DimensionData.Toolset.Cqrs;
	using DimensionData.Toolset.Querying;
	using DimensionData.Toolset.Validation;

	using TestMicroService.Queries.Contracts.Queries;
	using TestMicroService.Queries.Contracts.Responses;

	internal sealed class QueryTestsHandler : IQueryHandler<QueryTests, QueryTestsResponse>
	{
		private readonly ReadModelDataContext _modelContext;

		public QueryTestsHandler(ReadModelDataContext modelContext)
		{
			_modelContext = modelContext;
		}

		/// <summary>
		/// Read the query fron the read table
		/// </summary>
		/// <param name="query">
		/// The ListLocation quesry
		/// </param>
		/// <returns>
		/// The ListLocations response
		/// </returns>
		public async Task<QueryTestsResponse> Read(QueryTests query)
		{
			Check.NotNull(query, nameof(query));

			var queryResult = await _modelContext.Tests
										.ExecuteQueryOptionsAsync(query.Options);

			return Mapper.Map<QueryTestsResponse>(queryResult);
		}
	}
}
