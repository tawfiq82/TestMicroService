using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMicroService.WebApi.App_Start
{
	using System.IO;
	using System.Reflection;

	using DimensionData.Toolset.WebApi;
	using DimensionData.Toolset.WebApi.Swagger;

	using Swashbuckle.Application;

	public static class SwaggerConfig
	{
		public static OwinHttpConfigurationContext UseSwagger(this OwinHttpConfigurationContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}

			context
				.HttpConfiguration
				.EnableSwagger(c =>
				{
					c.SingleApiVersion("1.0", Startup.ServiceDescription);
					c.Schemes(new[] { "http", "https" });

					c.DescribeAllEnumsAsStrings();
					c.UseFullTypeNameInSchemaIds();
					c.OperationFilter<SwaggerBearerAuthorizationOperationFilter>();

					var baseDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
					var commentsFileName = Assembly.GetExecutingAssembly().GetName().Name + ".XML";
					var commentsFile = Path.Combine(baseDirectory, commentsFileName);
					c.IncludeXmlComments(commentsFile);

					// In accordance with the built in JsonSerializer, Swashbuckle will, by default, describe enums as integers.
					// You can change the serializer behavior by configuring the StringToEnumConverter globally or for a given
					// enum type. Swashbuckle will honor this change out-of-the-box. However, if you use a different
					// approach to serialize enums as strings, you can also force Swashbuckle to describe them as strings.
					// 
					c.DescribeAllEnumsAsStrings();

					//c.DocumentFilter(() => new ResourceDescriptionDocumentFilter(DimensionData.S4B.WebApi.Documentation.DocumentationResource.Overview, Assembly.GetExecutingAssembly()));
					c.DocumentFilter(() => new ResourceDescriptionDocumentFilter(DimensionData.Toolset.WebApi.Swagger.DocumentationResource.GeneralUsage));
					c.DocumentFilter(() => new ResourceDescriptionDocumentFilter(DimensionData.Toolset.WebApi.Swagger.DocumentationResource.Paging));
					c.DocumentFilter(() => new ResourceDescriptionDocumentFilter(DimensionData.Toolset.WebApi.Swagger.DocumentationResource.Ordering));
					c.DocumentFilter(() => new ResourceDescriptionDocumentFilter(DimensionData.Toolset.WebApi.Swagger.DocumentationResource.Filtering));
				})
				.EnableSwaggerUi(c =>
				{
					// By default, swagger-ui will validate specs against swagger.io's online validator and display the result
					// in a badge at the bottom of the page. Use these options to set a different validator URL or to disable the
					// feature entirely.
					//c.SetValidatorUrl("http://localhost/validator");
					c.DisableValidator();

					// Use this option to control how the Operation listing is displayed.
					// It can be set to "None" (default), "List" (shows operations for each resource),
					// or "Full" (fully expanded: shows operations and their details).
					//
					c.DocExpansion(DocExpansion.List);
				});


			return context;
		}
	}
}
