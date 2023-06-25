using ResourceManageGroup.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
namespace ResourceManageGroup.Data
{
public class CustomActionFilter : Attribute, IResultFilter
{
    /*public void OnActionExecuting(ActionExecutingContext context){
        Console.WriteLine("Action Name : "+context.ActionDescriptor.DisplayName);
        var recruiter = (Recruiter)context.ActionArguments["recruiter"];
        if(recruiter!=null){
        if (recruiter.RecruiterPassword!=null && recruiter.RecruiterEmail!=null)
        {
            Console.WriteLine("Email : "+recruiter.RecruiterEmail+", Password : "+recruiter.RecruiterPassword);
            Console.WriteLine("Email & Password are not null in OnActionExecuting");
        }
        else{
            context.Result = new RedirectToActionResult("LoginR", "Recruiter", null);
        }
        }
        else{
            context.Result = new RedirectToActionResult("LoginR", "Recruiter", null);
        }
    } 
    public void OnActionExecuted(ActionExecutedContext context){
        Console.WriteLine("Action Name : "+context.ActionDescriptor.DisplayName);
        var exception = context.HttpContext.Items["Exception"] as Exception;
        if (exception != null)
        {
            Console.WriteLine($"Exception occurred: {exception.Message}");
        }
        else{
            Console.WriteLine("No Exception occurred in OnActionExecuted");
        }
    }*/
    public void OnResultExecuting(ResultExecutingContext context){
        Console.WriteLine("Action Name : "+context.ActionDescriptor.DisplayName);
        Console.WriteLine("OnResultExecuting");
    } 
    public void OnResultExecuted(ResultExecutedContext context){
        Console.WriteLine("Action Name : "+context.ActionDescriptor.DisplayName);
        Console.WriteLine("OnResultExecuted");
    }
}
}