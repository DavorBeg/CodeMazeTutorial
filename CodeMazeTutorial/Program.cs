
using LoggerService;
using Microsoft.AspNetCore.HttpOverrides;
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

			builder.Services.AddControllers();

			var app = builder.Build();

			// Configure the HTTP request pipeline.

			if (app.Environment.IsDevelopment())
				app.UseDeveloperExceptionPage();
			else
				app.UseHsts();

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
