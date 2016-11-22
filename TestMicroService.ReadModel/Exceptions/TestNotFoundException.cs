using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMicroService.ReadModel.Exceptions
{
	public class TestNotFoundException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TestNotFoundException"/> class.
		/// </summary>
		/// <param name="testId">The test identifier.</param>
		public TestNotFoundException(Guid testId)
			: base($"Test with the Id ('{testId}') is not found.")
		{
		}
	}
}
