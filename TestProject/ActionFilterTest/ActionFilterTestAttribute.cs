using Microsoft.AspNetCore.Mvc.Filters;

namespace TestProject.ActionFilterTest
{
	public class ActionFilterTestAttribute : IActionFilter
	{
		private readonly ILogger _logger;

		public ActionFilterTestAttribute(ILogger logger)
		{
			_logger = logger;
		}

		public void OnActionExecuted(ActionExecutedContext context)
		{
			_logger.LogInformation("Before method Run!");
		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
			_logger.LogInformation("After method Run!");
		}
	}
}
