using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using ResourceManageGroup.Models;

namespace ResourceManageGroup.Controllers;
public class WebApiController : Controller
{
    private readonly ILogger<WebApiController> _logger;
    public WebApiController(ILogger<WebApiController> logger)
    {
        _logger = logger;
    }
    [HttpGet]
    public async Task<IActionResult> Index()
    {  
        HttpClientHandler clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        using (var client = new HttpClient(clientHandler))
        {
            client.BaseAddress = new Uri("http://localhost:5192/api/Sample");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.GetAsync("http://localhost:5192/api/Sample");
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result; 
                var employee = JsonConvert.DeserializeObject<List<WebApi>>(data);
                return View(employee);
            }
            else
            {
                return View("Error");
            }
        }
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}