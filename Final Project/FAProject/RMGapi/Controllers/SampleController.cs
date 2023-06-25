using Microsoft.AspNetCore.Mvc;
using ResourceManageGroup.Models;
using ResourceManageGroup.Data;
using Microsoft.EntityFrameworkCore;

namespace ResourceManageGroup.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SampleController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    public SampleController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    } 

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Sample>>> GetDetails()
    {
        return await _dbContext.Sample_Details.ToListAsync();
    }
       
}