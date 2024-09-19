using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace CompanyEmployees.Presentation.ActionFilters
{
	public sealed class ValidationFilterAttribute : IActionFilter
	{
		public void OnActionExecuted(ActionExecutedContext context)
		{
			throw new NotImplementedException();
		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
			var action = context.RouteData.Values["action"];
			var controller = context.RouteData.Values["controller"];

			var param = context.ActionArguments.SingleOrDefault(x => (x.Value?.ToString() ?? string.Empty).Contains("Dto")).Value;
			if(param == null)
			{
				context.Result = new BadRequestObjectResult($"Objest is null. Controller: {controller}, Action: {action}");
				return;
			}

			if(!context.ModelState.IsValid)
			{
				context.Result = new UnprocessableEntityObjectResult(context.ModelState);
			}
		}
	}
}
