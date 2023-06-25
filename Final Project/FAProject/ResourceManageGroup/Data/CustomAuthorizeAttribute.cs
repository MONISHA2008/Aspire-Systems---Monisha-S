using Microsoft.EntityFrameworkCore;
using ResourceManageGroup.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace ResourceManageGroup.Data
{
public class CustomAuthorizeRAttribute : TypeFilterAttribute
{
    public CustomAuthorizeRAttribute() : base(typeof(CustomAuthorizeRFilter))
    {
    }
}
public class CustomAuthorizeMAttribute : TypeFilterAttribute
{
    public CustomAuthorizeMAttribute() : base(typeof(CustomAuthorizeMFilter))
    {
    }
}
public class CustomAuthorizeEAttribute : TypeFilterAttribute
{
    public CustomAuthorizeEAttribute() : base(typeof(CustomAuthorizeEFilter))
    {
    }
}
public class CustomAuthorizeRFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.Session.GetObjectFromJson<Recruiter>("users");
        if (user == null)
        {
            Console.WriteLine("Error: users is null");
            context.Result = new RedirectToActionResult("LoginR", "Recruiter", null);
            return;
        }
        Console.WriteLine($"users: {JsonConvert.SerializeObject(user)}");
    }
}
public class CustomAuthorizeMFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.Session.GetObjectFromJson<Manager>("users");
        if (user == null)
        {
            Console.WriteLine("Error: users is null");
            context.Result = new RedirectToActionResult("LoginM", "Manager", null);
            return;
        }
        Console.WriteLine($"users: {JsonConvert.SerializeObject(user)}");
    }
}

public class CustomAuthorizeEFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.Session.GetObjectFromJson<Employee>("users");
        if (user == null)
        {
            Console.WriteLine("Error: users is null");
            context.Result = new RedirectToActionResult("LoginE", "Employee", null);
            return;
        }
        Console.WriteLine($"users: {JsonConvert.SerializeObject(user)}");
    }
}
}