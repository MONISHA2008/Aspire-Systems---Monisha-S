using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ActionFilters.Data
{
    public class CustomActionFilter : Attribute, IActionFilter, IExceptionFilter
    {
        private readonly string _name;

        public CustomActionFilter()
        {
        }

        public CustomActionFilter(string name)
        {
            this._name = name;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine($"OnActionExecuted - {_name}");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"OnActionExecuting - {_name}");
        }
        private readonly IHostEnvironment _hostEnvironment;

        public CustomActionFilter(IHostEnvironment hostEnvironment) =>
        _hostEnvironment = hostEnvironment;
      

        public void OnException(ExceptionContext context)
        {
            if (!_hostEnvironment.IsDevelopment())
            {
                // Don't display exception details unless running in Development.
                return;
            }

            context.Result = new ContentResult
            {
                Content = context.Exception.ToString()
            };
        }
    }
}