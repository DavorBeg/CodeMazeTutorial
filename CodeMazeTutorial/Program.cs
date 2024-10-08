
using Application.Companies.Behaviors;
using AspNetCoreRateLimit;
using CodeMazeTutorial.Extensions;
using CodeMazeTutorial.Utility;
using CompanyEmployees.Presentation.ActionFilters;
using Contracts;
using FluentValidation;
using LoggerService;
using MediatR;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Configuration;
using Service.DataShaping;
using Shared.DataTransferObjects.EmployeeDtos;

namespace CodeMazeTutorial
{

    // Last page: 320 (Ducment api with swagger)
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(builder.Configuration)
				.CreateLogger();

			builder.Services.ConfigureCors();
			builder.Services.ConfigureResponseCaching();
			builder.Services.ConfigureHttpCacheHeaders();
			builder.Services.ConfigureIISIntegration();
			builder.Services.ConfigureLoggerService();
			builder.Services.ConfigureRepositoryManager();
			builder.Services.ConfigureServiceManager();
			builder.Services.ConfigureSqlContext(builder.Configuration);
			builder.Services.AddActionFiltersServices();
			builder.Services.ConfigureVersioning();
			builder.Services.AddJwtConfiguration(builder.Configuration);
			builder.Services.ConfigureSwagger();

			builder.Services.AddMemoryCache();
			builder.Services.ConfigureRateLimitingOptions();
			builder.Services.AddHttpContextAccessor();

			builder.Services.AddAuthentication();
			builder.Services.ConfigureIdentity();
			builder.Services.ConfigureJWT(builder.Configuration);

			builder.Services.AddScoped<ValidateMediaTypeAttribute>();

			builder.Services.AddAutoMapper(typeof(Program));
			builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

			builder.Services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();
			builder.Services.AddScoped<IEmployeeLinks, EmployeeLinks>();
			
			builder.Services.Configure<ApiBehaviorOptions>(options =>
			{
				options.SuppressModelStateInvalidFilter = true;
			});

			builder.Services.AddControllers(config =>
			{
				config.RespectBrowserAcceptHeader = true;
				config.ReturnHttpNotAcceptable = true;
				config.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());				config.CacheProfiles.Add("120SecondsDuration", new CacheProfile { Duration = 120 });
			})
			.AddXmlDataContractSerializerFormatters()
			.AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);

			builder.Services.AddValidatorsFromAssembly(typeof(Application.AssemblyReference).Assembly);	
			builder.Services.AddCustomMediaTypes();
			builder.Services.AddMediatR(config =>
			{
				config.RegisterServicesFromAssembly(typeof(Application.AssemblyReference).Assembly);
			});

			var app = builder.Build();
			app.UseIpRateLimiting();
			app.UseCors("CorsPolicy");

			app.UseSwagger();
			app.UseSwaggerUI(opt =>
			{
				opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Code Maze API v1");
				opt.SwaggerEndpoint("/swagger/v2/swagger.json", "Code Maze API v2");
			});

			app.UseResponseCaching();
			app.UseHttpCacheHeaders();

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

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
