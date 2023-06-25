using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ResourceManageGroup.Models;
using ResourceManageGroup.Data;

namespace ResourceManageGroup.Controllers;
//[Route("home")]
public class HomeController : Controller
{ 
    private readonly ApplicationDbContext _dbContext;
    public HomeController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    //[Route("error")]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
