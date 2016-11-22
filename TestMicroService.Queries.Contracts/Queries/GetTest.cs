using System;
using DimensionData.Toolset.Cqrs;
using TestMicroService.Queries.Contracts.Responses;


namespace TestMicroService.Queries.Contracts.Queries
{
	public class GetTest : IQuery<GetTestResponse>
	{
		/// <summary>
		/// Private constructor 
		/// </summary>
		private GetTest()
		{
		}

		/// <summary>
		/// The public constructor
		/// </summary>
		/// <param name="testId">The optional paging, sorting and filtering Options</param>
		public GetTest(Guid testId)
			: this()
		{
			TestId = testId;
		}

		/// <summary>
		/// Gets or sets the optional paging, sorting and filtering Options.
		/// </summary>
		public Guid TestId { get; private set; }
	}
}
