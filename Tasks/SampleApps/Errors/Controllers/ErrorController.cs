using Microsoft.AspNetCore.Mvc;
using Errors.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace Errors.Controllers
{
    public class ErrorController : Controller
    {

        public IActionResult Error()
        {
            var errorModel = new ErrorModel
            {
                ErrorCode = 500,
                ErrorMessage = "An error occurred. Please try again later."
            };
            return View(errorModel);
        }

        public IActionResult ServerError()
        {

            return View();
        }

        [HttpGet("/Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            // Handle different status codes and customize error messages or logic
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the resource you requested could not be found.";
                    break;
                case 500:
                    ViewBag.ErrorMessage = "Oops! Something went wrong on the server.";
                    break;
                default:
                    ViewBag.ErrorMessage = "An error occurred.";
                    break;
            }

            ViewBag.StatusCode = statusCode;
            ViewBag.OriginalPath = statusCodeResult?.OriginalPath;
            ViewBag.OriginalQueryString = statusCodeResult?.OriginalQueryString;

            return View();
        }
    }
}