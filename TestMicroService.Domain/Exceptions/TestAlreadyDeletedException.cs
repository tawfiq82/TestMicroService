using System;

namespace TestMicroService.Domain.Exceptions
{
	public class TestAlreadyDeletedException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TestAlreadyDeletedException"/> class.
		/// </summary>
		/// <param name="testId">The test identifier.</param>
		public TestAlreadyDeletedException(Guid testId)
            : base($"Test with the Id ('{testId}') already deleted.")
        {
		}
	}
}
