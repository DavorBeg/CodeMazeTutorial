
using CodeMazeTutorial.Extensions;
using Contracts;
using LoggerService;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Configuration;

namespace CodeMazeTutorial
{

    // Last page: 40 (CompanyConfiguration class)
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(builder.Configuration)
				.CreateLogger();

			builder.Services.ConfigureCors();
			builder.Services.ConfigureIISIntegration();
			builder.Services.ConfigureLoggerService();
			builder.Services.ConfigureRepositoryManager();
			builder.Services.ConfigureServiceManager();
			builder.Services.ConfigureSqlContext(builder.Configuration);
			builder.Services.AddAutoMapper(typeof(Program));

			builder.Services.Configure<ApiBehaviorOptions>(options =>
			{
				options.SuppressModelStateInvalidFilter = true;
			});

			builder.Services.AddControllers(config =>
			{
				config.RespectBrowserAcceptHeader = true;
				config.ReturnHttpNotAcceptable = true;
			})
			.AddXmlDataContractSerializerFormatters()
			.AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);

			builder.Services.AddControllers()
				.AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);

			var app = builder.Build();

			var logger = app.Services.GetRequiredService<ILoggerManager>();
			app.ConfigureExceptionHandler(logger);

			// Configure the HTTP request pipeline.
			if(app.Environment.IsProduction())
			{
				app.UseHsts();
			}

            app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseForwardedHeaders(new ForwardedHeadersOptions()
			{
				ForwardedHeaders = ForwardedHeaders.All
			});

			app.UseAuthorization();
			app.MapControllers();

			app.Run();
		}
	}
}
