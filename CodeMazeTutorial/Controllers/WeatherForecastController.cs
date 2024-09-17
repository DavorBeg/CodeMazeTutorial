using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CodeMazeTutorial.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILoggerManager _logger;
		public WeatherForecastController(ILoggerManager logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public IEnumerable<string> Get()
		{
			_logger.LogDebug("Log debug");
			_logger.LogInfo("Log info");
			_logger.LogWarn("Log warning");
			_logger.LogError("Log error");
			
			return ["a", "b"];
		}
	}
}
