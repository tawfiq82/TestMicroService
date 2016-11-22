using System;
using System.Threading.Tasks;

namespace TestMicroService.ReadModel
{
	using System.Data.Entity;
	using System.Diagnostics.CodeAnalysis;

	using DimensionData.Toolset.Configuration;
	using DimensionData.Toolset.Configuration.Sections;
	using DimensionData.Toolset.Ef;

	using TestMicroService.ReadModel.DataModel;

	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Instantiated by IOC container.")]
	internal sealed class ReadModelDataContext : DataContext
	{
		internal const string Schema = "ReadModel";
		private const string ConnectionStringPath = "test/database";

		public ReadModelDataContext(ISecureConfigurationStore configurationStore)
			: base(GetConnectionString(configurationStore))
		{
		}

		public DbSet<Test> Tests { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Test>()
				.ToTable("Test", Schema);
		}

		private static string GetConnectionString(ISecureConfigurationStore configurationStore)
		{
			var section = Task.Run(async () => await configurationStore.GetSection<ConnectionStringSection>(ConnectionStringPath)).Result;
			if (string.IsNullOrEmpty(section?.ConnectionString))
			{
				throw new Exception($"Database configuration not found at '{ConnectionStringPath}'.");
			}

			return section.ConnectionString;
		}
	}
}
