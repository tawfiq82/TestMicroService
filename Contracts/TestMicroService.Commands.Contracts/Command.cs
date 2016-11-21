using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMicroService.Commands.Contracts
{
	using DimensionData.Toolset.Cqrs;

	/// <summary>
	/// Ths is the base class for all S4B commands
	/// </summary>
	/// <seealso cref="DimensionData.Toolset.Cqrs.ICommand" />
	public abstract class Command : ICommand
	{
	}
}
