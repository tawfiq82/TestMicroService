namespace TestMicroService.Queries.Contracts.Queries
{
	using DimensionData.Toolset.Cqrs;
	using DimensionData.Toolset.Querying;

	using TestMicroService.Queries.Contracts.Responses;

	public class QueryTests : IQuery<QueryTestsResponse>
	{
		/// <summary>
		/// Private constructor 
		/// </summary>
		private QueryTests()
		{
		}

		/// <summary>
		/// The public constructor
		/// </summary>
		/// <param name="options">The optional paging, sorting and filtering Options</param>
		public QueryTests(QueryOptions options)
			: this()
		{
			Options = options;
		}

		/// <summary>
		/// Gets or sets the optional paging, sorting and filtering Options.
		/// </summary>
		public QueryOptions Options { get; private set; }
	}
}
