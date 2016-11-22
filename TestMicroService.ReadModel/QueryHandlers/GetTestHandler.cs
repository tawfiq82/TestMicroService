using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TestMicroService.Queries.Contracts.Queries;
using DimensionData.Toolset.Cqrs;
using DimensionData.Toolset.Querying;
using DimensionData.Toolset.Validation;

using TestMicroService.Queries.Contracts.Responses;

namespace TestMicroService.ReadModel.QueryHandlers
{
	using System.Data.Entity;
	using System.IO;

	using TestMicroService.Queries.Contracts.Dtos;
	using TestMicroService.ReadModel.Exceptions;

	internal sealed class GetTestHandler : IQueryHandler<GetTest, GetTestResponse>
	{
		private readonly ReadModelDataContext _modelContext;

		public GetTestHandler(ReadModelDataContext modelContext)
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
		public async Task<GetTestResponse> Read(GetTest query)
		{
			Check.NotNull(query, nameof(query));

			var test = await _modelContext.Tests.FirstOrDefaultAsync(x => x.Id == query.TestId);

			if (test == null)
				throw new TestNotFoundException(query.TestId);

			return new GetTestResponse()
			{
				Test = new Test()
				{
					Id = test.Id,
					Name = test.Name,
					Description = test.Description,
					TestType = test.TestType,
					IsDeleted = test.IsDeleted
				}
			};
		}
	}
}
