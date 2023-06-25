using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ResourceManageGroup.Models;
using ResourceManageGroup.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
namespace ResourceManageGroup.Controllers;
public class ManagerController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IConfiguration _configuration;
    public ManagerController(ApplicationDbContext dbContext,IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }
    [HttpGet]
    [AllowAnonymous]
    public IActionResult RegisterM()
    {
        return View();
    }
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> RegisterM(Manager manager,ValidationUser validation)
    {
        if(ModelState.IsValid){
            try{
                int emailcount = 0;
                int numcount = 0;
                if(_dbContext.Manager_Details!=null){
                emailcount = _dbContext.Manager_Details.Where(c => c.ManagerEmail == manager.ManagerEmail).Count();
                numcount = _dbContext.Manager_Details.Where(c => c.ManagerNumber == manager.ManagerNumber).Count();
                if(manager.ManagerEmail!=null  && manager.ManagerNumber!=null && manager.ManagerPassword!=null){
                                if(emailcount>0){
                                    ViewData["Message"]="The User Already Exists !!!!!";
                                }
                                else if(numcount>0){
                                    ViewData["Message"]="The User Already Exists !!!!!";
                                }
                                else{
                                    _dbContext.Add(manager);
                                    await _dbContext.SaveChangesAsync();
                                    ViewData["Message"]="Succesfully Registered , Log in to Work";
                                }
                            }
                }           
            }
            catch(Exception exception){
                ModelState.AddModelError(String.Empty,$"Something Went Wrong{exception.Message}"); 
            }
        }
        ModelState.AddModelError(String.Empty,$"Something Went Wrong Invalid Model"); 
        return View(manager);
    }
    [HttpGet]
    [AllowAnonymous]
    public IActionResult forgotPasswordM()
    {
        return View();
    }
    [HttpGet]
    [AllowAnonymous]
    public IActionResult VerifyUserM()
    {
        return View();
    }
    [HttpPost]
    [AllowAnonymous]
    public IActionResult VerifyUserM(ValidationUser validation,Manager manager)
    {
        string ? myData = TempData["verificationcode"] as String;
        string ? email = TempData["email"] as String;
        if(myData==validation.VerifyCode){
            manager.ManagerEmail=email;
            HttpContext.Session.SetObjectAsJson("users", manager);
            return RedirectToAction("ProjectListM", "Manager");
        }
        else{
            ViewBag.message="OTP is Invalid";
        }
        return View();
    }
    [HttpPost]
    [AllowAnonymous]
    public IActionResult forgotPasswordM(Manager manager,ValidationUser validation)
    {
        if(ModelState.IsValid){
            try{
                int count =0;
                if(_dbContext.Manager_Details!=null){
                count = _dbContext.Manager_Details.Where(c => c.ManagerEmail == manager.ManagerEmail).Count();
                if(manager.ManagerEmail!=null){
                    if(count>0){
                        try
                        {
                            Random random = new Random();
                            string code = random.Next(100000, 999999).ToString();
                            TempData["verificationcode"] = code;
                            TempData["email"] = manager.ManagerEmail;
                            string from, pass, messageBody;
                            MailMessage message = new MailMessage();
                            from = "kishorevijaykumar26@gmail.com";
                            pass = "iepcxotvxhfxjbjc";
                            messageBody = "Your Verification Code is " + code;
                            if(manager.ManagerEmail != null)
                            {
                                message.To.Add(new MailAddress(manager.ManagerEmail));
                            }
                            else
                            {
                                throw new Exception("User email is null or invalid.");
                            }
                            message.From = new MailAddress(from);
                            message.Body = messageBody;
                            message.Subject = "Otp To Login ";
                            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                            smtp.EnableSsl = true;
                            smtp.Port = 587;
                            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtp.UseDefaultCredentials = false;
                            smtp.EnableSsl = true;
                            smtp.Credentials = new NetworkCredential(from, pass);                  
                            smtp.Send(message);
                            ViewData["Message"] = "Verification Code sent successfully";
                            return RedirectToAction("VerifyUserM", "Manager");
                        }
                        catch (Exception ex)
                        {
                            // Handle any errors that occur while sending the email
                            ViewData["Message"] = "Error sending verification code: " + ex.Message;
                        }
                    }
                }
                else{
                    ViewData["Message"]="Check Your E-Mail";
                }
                }
            }
            catch(Exception exception){
                ModelState.AddModelError(String.Empty,$"Something Went Wrong{exception.Message}"); 
            }
        }
        return View(manager);
    }
    [HttpGet]
    [AllowAnonymous]
    public IActionResult LoginM(Manager manager)
    {
        var cookieValue = Request.Cookies["MyMAuthCookie"];
        if (cookieValue != null)
        {
            var decodedCookie = cookieValue.Split(":");
            manager.ManagerEmail = decodedCookie[0];
            manager.ManagerPassword = decodedCookie[1];
            ViewBag.email=manager.ManagerEmail;
            ViewBag.password=manager.ManagerPassword;
        }
        return View();
    }
    [HttpPost]
    [AllowAnonymous]
    public IActionResult LoginM(Manager manager,ValidationUser validation)
    {
        if(ModelState.IsValid){
            try{
                /*int count =0;
                string? dbpassword=" ";
                if(_dbContext.Manager_Details!=null){
                    count = _dbContext.Manager_Details.Where(c => c.ManagerEmail == manager.ManagerEmail).Count();
                    dbpassword = _dbContext.Manager_Details.Where(c => c.ManagerEmail == manager.ManagerEmail).Select(c => c.ManagerPassword).FirstOrDefault();
                    if(manager.ManagerEmail!=null && manager.ManagerPassword!=null){
                        if(count>0){
                            if(dbpassword==manager.ManagerPassword)
                            {*/
                            if(_dbContext.Manager_Details!=null){
                                var authenticatedManager = _dbContext.Manager_Details.FirstOrDefault(c => c.ManagerEmail == manager.ManagerEmail);
                                if (authenticatedManager != null && authenticatedManager.ManagerPassword == manager.ManagerPassword){
                                    TempData["email"]=manager.ManagerEmail;
                                    HttpContext.Session.SetObjectAsJson("users", manager);
                                    //var token = GenerateJwtToken(manager.ManagerEmail);
                                    //Console.WriteLine(token);
                                    var cookieValue = $"{manager.ManagerEmail}:{manager.ManagerPassword}";
                                    var cookieOptions = new CookieOptions
                                    {
                                        Expires = DateTimeOffset.Now.AddDays(30),
                                        HttpOnly = true
                                    };
                                    Response.Cookies.Append("MyMAuthCookie", cookieValue, cookieOptions);
                                    var url = Url.Action("ProjectListM", "Manager");
                                    if (url != null){
                                        return Redirect(url);
                                    }
                                }
                                else
                                {
                                    ViewData["Message"] = "Invalid email or password.";
                                }
                            }
                            /*}
                            else{
                                ViewData["Message"]="Check Your Password";
                            }
                        }
                    }
                    else{
                        ViewData["Message"]="Check Your E-Mail";
                    }
                }*/
            }
            catch(Exception exception){
                ModelState.AddModelError(String.Empty,$"Something Went Wrong{exception.Message}"); 
            }
        }
        return View(manager);
    }
    
    [HttpGet]
    public async Task<IActionResult> ProjectListM()
    {
        if(_dbContext.Project_Details!=null){
            var projects = await _dbContext.Project_Details.ToListAsync();
            return View(projects);
        }
        return View();
    }
    [HttpGet]
    
    public async Task<IActionResult> ManageProjectM(int id)
    { 
        string ? myData = TempData["email"] as string;
        if(myData!=null)
        TempData.Keep(myData);
        if(_dbContext.Project_Details!=null){
            var projects = await _dbContext.Project_Details.Where(x => x.project_id == id ).FirstOrDefaultAsync();
            if(projects!=null){
                TempData["myData"] = projects.project_name;
                return View(projects);
            }
        }
        return View();
    }
    [HttpGet]
    
    public IActionResult AddProjectM()
    {
        return View();
    }
    [HttpPost]
    
    public async Task<IActionResult> AddProjectM(Project project)
    {
        if(ModelState.IsValid){
            try{
                string ? myData = TempData["email"] as string;
                if(myData!=null)
                TempData.Keep(myData);
                int namecount = 0;
                if(_dbContext.Project_Details!=null){
                    namecount = _dbContext.Project_Details.Where(c => c.project_name == project.project_name).Count();
                    if(namecount>0){
                        ViewData["Message"]="The Project Name Already Exists !!!!!";
                    }
                    else{
                        
                        project.project_lead="Not Assigned";
                        project.project_type="Not Assigned";
                        _dbContext.Add(project);
                        await _dbContext.SaveChangesAsync();
                        var url = Url.Action("ProjectListM", "Manager");
                        if (url != null){
                            return Redirect(url);
                        }
                    }
                }
            }
            catch(Exception exception){
                ModelState.AddModelError(String.Empty,$"Something Went Wrong{exception.Message}"); 
            }
        }
        ModelState.AddModelError(String.Empty,$"Something Went Wrong Invalid Model");
        return View(project);
    }
    [HttpGet]
    
    public async Task<IActionResult> UpdateProjectM(int id,Project project)
    {
        project.myList.AddRange(new string[] {"Web Application","Mobile Application","Desktop Appliaion"});
        ViewBag.list = project.myList;
        if(_dbContext.Project_Details!=null){
            var projects = await _dbContext.Project_Details.Where(x => x.project_id == id ).FirstOrDefaultAsync();
            return View(projects);
        }
        return View();
    }
    [HttpPost]
    
    public async Task<IActionResult> UpdateProjectM(Project project)
    {
        if(ModelState.IsValid){
            try{
                if(_dbContext.Project_Details!=null){
                    var projects = await _dbContext.Project_Details.Where(x => x.project_id == project.project_id ).FirstOrDefaultAsync();
                    if(projects != null){
                        projects.project_name= project.project_name;
                        projects.project_description= project.project_description;
                        projects.project_type=project.project_type;
                        projects.project_start_time=project.project_start_time;
                        projects.project_end_time=project.project_end_time;
                    }
                    await _dbContext.SaveChangesAsync();
                    var url = Url.Action("ManageProjectM", "Manager",new { id = project.project_id });
                    if (url != null){
                        return Redirect(url);
                    }
                }
            }
            catch(Exception exception){
                ModelState.AddModelError(String.Empty,$"Something Went Wrong{exception.Message}"); 
            }
        }
        ModelState.AddModelError(String.Empty,$"Something Went Wrong Invalid Model");
        return View(project);
    }
    [HttpGet]
    
    public async Task<IActionResult> DeleteProjectM(int id)
    {
        if(_dbContext.Project_Details!=null){
            var projects = await _dbContext.Project_Details.Where(x => x.project_id == id ).FirstOrDefaultAsync();
            return View(projects);
        }
        return View();
    }
    [HttpPost]
    
    public async Task<IActionResult> DeleteProjectM(Project project)
    { 
        if(ModelState.IsValid){
            try{
                if(_dbContext.Project_Details!=null){
                    var projects = _dbContext.Project_Details.FirstOrDefault(p => p.project_id == project.project_id);
                    if(projects != null){
                        _dbContext.Remove(projects);
                        await _dbContext.SaveChangesAsync();
                        return RedirectToAction("ProjectListM");
                    }
                }
            }
            catch(Exception exception){
                ModelState.AddModelError(String.Empty,$"Something Went Wrong{exception.Message}"); 
            }
        }
        ModelState.AddModelError(String.Empty,$"Something Went Wrong Invalid Model"); 
        return View(project);
    }
    [HttpGet]
    
    public async Task<IActionResult> ManagePeopleM()
    {
        string ? myData = TempData["myData"] as String;
        TempData.Keep("myData");
        if(_dbContext.Employee_Details!=null){
            var employees = await _dbContext.Employee_Details.Where(e => e.EmployeeProject == myData).ToListAsync();
            return View(employees);
        }
        return View();
    }
    [HttpGet]
    
    public async Task<IActionResult> AddPeopleM()
    {
        string ? myData = TempData["myData"] as String;
        TempData.Keep("myData");
        if(_dbContext.Employee_Details!=null){
            var employees = await _dbContext.Employee_Details.Where(e => e.EmployeeWorkingStatus == "Bench").ToListAsync();
            return View(employees);
        }
        return View();
    }
    [HttpGet]
    
    public async Task<IActionResult> AddEmployeeM(string id)
    {
        string ? myData = TempData["myData"] as String;
        TempData.Keep("myData");
        if(_dbContext.Employee_Details!=null){
            var employees = await _dbContext.Employee_Details.Where(x => x.EmployeeId == id ).FirstOrDefaultAsync();
            return View(employees);
        }
        return View();
    }
    public IActionResult GetImage(string id)
    {
        if(_dbContext.Employee_Details!=null){
            var employee = _dbContext.Employee_Details.FirstOrDefault(e => e.EmployeeId == id);
            if (employee != null && employee.EmployeeImage != null)
            {
                return File(employee.EmployeeImage, "image/jpeg"); 
            }
        }
        return NotFound();
    }
    [HttpPost]
    
    public async Task<IActionResult> AddEmployeeM(Employee employee)
    {
        if(ModelState.IsValid){
            try{
                string ? myData = TempData["myData"] as String;
                TempData.Keep("myData");
                if(_dbContext.Employee_Details!=null){
                    var employees = _dbContext.Employee_Details.FirstOrDefault(p => p.EmployeeId == employee.EmployeeId);
                    if(employees != null){
                        employees.EmployeeProject=myData;
                        employees.EmployeeWorkingStatus="InProject";
                        await _dbContext.SaveChangesAsync();
                        var url = Url.Action("ManagePeopleM", "Manager");
                        if (url != null){
                            return Redirect(url);
                        }
                    }
                }
            }
            catch(Exception exception){
                ModelState.AddModelError(String.Empty,$"Something Went Wrong{exception.Message}"); 
            }
        }
        ModelState.AddModelError(String.Empty,$"Something Went Wrong Invalid Model"); 
        return View(employee);
    }

    [HttpGet]
    
    public async Task<IActionResult> RemovePeopleM(string id,Employee employee)
    {
        if(_dbContext.Employee_Details!=null){
            var employees = await _dbContext.Employee_Details.Where(x => x.EmployeeId == id ).FirstOrDefaultAsync();
            return View(employees);
        }
        return View();
    }
    [HttpPost]
    
    public async Task<IActionResult> RemovePeopleM(Employee employee)
    { 
        if(ModelState.IsValid){
            try{
                if(_dbContext.Employee_Details!=null){
                    var employees = _dbContext.Employee_Details.FirstOrDefault(p => p.EmployeeId == employee.EmployeeId);
                    if(employees != null){
                        employees.EmployeeWorkingStatus="Bench";
                        employees.EmployeeProject="Not Assigned"; 
                    }
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("ManagePeopleM");
                }
            }
            
            catch(Exception exception){
                ModelState.AddModelError(String.Empty,$"Something Went Wrong{exception.Message}"); 
            }
        }
        ModelState.AddModelError(String.Empty,$"Something Went Wrong Invalid Model"); 
        return View(employee);
    }
    [HttpGet]
    
    public async Task<IActionResult> AssignLeaderM(string id,Employee employee)
    {
        if(_dbContext.Employee_Details!=null){
            var employees = await _dbContext.Employee_Details.Where(x => x.EmployeeId == id ).FirstOrDefaultAsync();
            return View(employees);
        }
        return View();
    }
    [HttpPost]
    
    public async Task<IActionResult> AssignLeaderM(Employee employee,Project project)
    { 
        if(ModelState.IsValid){
            try{
                if(_dbContext.Employee_Details!=null&&_dbContext.Project_Details!=null){
                    var projects = _dbContext.Project_Details.FirstOrDefault();
                    string ? name = _dbContext.Employee_Details.Where(c => c.EmployeeId == employee.EmployeeId).Select(c => c.EmployeeName).FirstOrDefault();
                    if(projects!=null){
                        projects.project_lead=name;
                    }
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("ManagePeopleM");
                }
            }
            
            catch(Exception exception){
                ModelState.AddModelError(String.Empty,$"Something Went Wrong{exception.Message}"); 
            }
        }
        ModelState.AddModelError(String.Empty,$"Something Went Wrong Invalid Model"); 
        return View(employee);
    }
    
    public ActionResult LogoutM()
    {
        return RedirectToAction("LoginM");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    /*private string GenerateJwtToken(string email)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, email)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpiryInDays"]));
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: expires,
            signingCredentials: credentials
        );
        return token.ToString();
    }*/
}