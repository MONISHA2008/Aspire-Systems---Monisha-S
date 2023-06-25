using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ResourceManageGroup.Models;
using ResourceManageGroup.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.ComponentModel.DataAnnotations;
namespace ResourceManageGroup.Controllers;
public class EmployeeController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    public EmployeeController(ApplicationDbContext dbContext,IWebHostEnvironment hostEnvironment)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult RegisterE()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> RegisterE(Employee employee,ValidationUser validation)
    {
        if(ModelState.IsValid){
            try{
                int emailcount = 0;
                int numcount = 0;
                if(_dbContext.Employee_Details!=null){
                    emailcount = _dbContext.Employee_Details.Where(c => c.EmployeeEmail == employee.EmployeeEmail).Count();
                    numcount = _dbContext.Employee_Details.Where(c => c.EmployeeNumber == employee.EmployeeNumber).Count();
                    if(employee.EmployeeEmail!=null &&  employee.EmployeeNumber!=null && employee.EmployeePassword!=null){
                        
                                    if(emailcount>0){
                                        ViewData["Message"]="The User Already Exists !!!!!";
                                    }
                                    else if(numcount>0){
                                        ViewData["Message"]="The User Already Exists !!!!!";
                                    }
                                    else{
                                                employee.EmployeeTechnology = "Not Assigned";
                                                employee.EmployeeProject = "Not Assigned";
                                                employee.EmployeeWorkingStatus = "Not Assigned" ;
                                                employee.EmployeeTrainingStartTime = "Not Assigned" ;
                                                employee.EmployeeTrainingEndTime = "Not Assigned" ;
                                                employee.EmployeeTrainerName = "Not Assigned" ;
                                                employee.EmployeeVacationStartTime = "Not Assigned" ;
                                                employee.EmployeeVacationEndTime = "Not Assigned" ;
                                                employee.EmployeeVacationStatus =  "Not Assigned" ;
                                                employee.EmployeeImage = null;  
                                                employee.EmployeeVacationReason = "Not Assigned";                                                                    ViewData["Message"]="Succesfully Registered , Log in to Work";
                                        ViewData["Message"]="Succesfully Registered , Log in to Work";
                                        _dbContext.Add(employee);
                                        await _dbContext.SaveChangesAsync();
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
    public IActionResult forgotPasswordE()
    {
        return View();
    }
    [HttpGet]
    public IActionResult VerifyUserE()
    {
        return View();
    }
    [HttpPost]
    public IActionResult VerifyUserE(ValidationUser validation,Employee employee)
    {
        string ? myData = TempData["verificationcode"] as String;
        string ? email = TempData["email"] as String;
        if(myData==validation.VerifyCode){
            employee.EmployeeEmail=email;
            HttpContext.Session.SetObjectAsJson("users", employee);
            return RedirectToAction("EmployeeViewE", "Employee");
        }
        else{
            ViewBag.message="OTP is Invalid";
        }
        return View();
    }
    [HttpPost]
    public IActionResult forgotPasswordE(Employee employee,ValidationUser validation)
    {
        if(ModelState.IsValid){
            try{
                int count =0;
                if(_dbContext.Employee_Details!=null){
                    count = _dbContext.Employee_Details.Where(c => c.EmployeeEmail == employee.EmployeeEmail).Count();
                    if(employee.EmployeeEmail!=null){
                        if(count>0){
                            try
                            {
                                Random random = new Random();
                                string code = random.Next(100000, 999999).ToString();
                                TempData["verificationcode"] = code;
                                TempData["email"] = employee.EmployeeEmail;
                                string from, pass, messageBody;
                                MailMessage message = new MailMessage();
                                from = "kishorevijaykumar26@gmail.com";
                                pass = "iepcxotvxhfxjbjc";
                                messageBody = "Your Verification Code is " + code;
                                if(employee.EmployeeEmail != null)
                                {
                                    message.To.Add(new MailAddress(employee.EmployeeEmail));
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
                                smtp.Credentials = new NetworkCredential(from, pass);                  
                                smtp.Send(message);
                                ViewData["Message"] = "Verification Code sent successfully";
                                return RedirectToAction("VerifyUserE", "Employee");
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
        return View(employee);
    }
    [HttpGet]
    public IActionResult LoginE(Employee employee)
    {
        var cookieValue = Request.Cookies["MyEAuthCookie"];
        if (cookieValue != null)
        {
            var decodedCookie = cookieValue.Split(":");
            employee.EmployeeEmail = decodedCookie[0];
            employee.EmployeePassword = decodedCookie[1];
            ViewBag.email=employee.EmployeeEmail;
            ViewBag.password=employee.EmployeePassword;
        }
        return View();
    }
    [HttpPost]
    public IActionResult LoginE(Employee employee,ValidationUser validation)
    {
        ModelState.Remove("EmployeeVacationStartTime");
        ModelState.Remove("EmployeeVacationEndTime");
        ModelState.Remove("EmployeeTrainingStartTime");
        ModelState.Remove("EmployeeTrainingEndTime");
        ModelState.Remove("EmployeeAge");
        ModelState.Remove("EmployeeConfirmPassword");
        if(ModelState.IsValid){
            try{
                int count =0;
                string? dbpassword=" ";
                if(_dbContext.Employee_Details!=null){
                count = _dbContext.Employee_Details.Where(c => c.EmployeeEmail == employee.EmployeeEmail).Count();
                dbpassword = _dbContext.Employee_Details.Where(c => c.EmployeeEmail == employee.EmployeeEmail).Select(c => c.EmployeePassword).FirstOrDefault();
                if(employee.EmployeeEmail!=null && employee.EmployeePassword!=null)
                if(count>0){
                        if(dbpassword==employee.EmployeePassword)
                        {
                            HttpContext.Session.SetObjectAsJson("users", employee);
                            //Cookies 
                            var cookieValue = $"{employee.EmployeeEmail}:{employee.EmployeePassword}";
                            var cookieOptions = new CookieOptions
                            {
                                Expires = DateTimeOffset.Now.AddDays(30),
                                HttpOnly = true
                            };
                            Response.Cookies.Append("MyEAuthCookie", cookieValue, cookieOptions);
                            string empId;
                            if(_dbContext.Employee_Details!=null){
                            empId = _dbContext.Employee_Details.Where(c => c.EmployeeEmail == employee.EmployeeEmail).Select(c => c.EmployeeId).FirstOrDefault();
                            TempData["EmpId"]=Convert.ToString(empId);
                            Console.WriteLine(empId);
                            var url = Url.Action("EmployeeViewE","Employee",1);
                            if (url != null){
                                return Redirect(url);
                            }
                            }
                        }
                        else{
                            ViewData["Message"]="Check Your Password";
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
        return View(employee);
    }
    [HttpGet]
    [CustomAuthorizeE]
    public async Task<IActionResult> EmployeeViewE(Employee employee)
    {
        string ? myData = TempData["EmpId"] as String;
        TempData.Keep("EmpId");
        employee.EmployeeId=myData;
        if(_dbContext.Employee_Details!=null){
            var employees = await _dbContext.Employee_Details.Where(x => x.EmployeeId == employee.EmployeeId ).FirstOrDefaultAsync();
            return View(employees);
        }
        return View();
    }
    [HttpGet]
    [CustomAuthorizeE]
    public async Task<IActionResult> VacationRequestE(Employee employee)
    {
        string ? myData = TempData["EmpId"] as String;
        TempData.Keep("EmpId");
        employee.EmployeeId=myData;
        if(_dbContext.Employee_Details!=null){
            var employees = await _dbContext.Employee_Details.Where(x => x.EmployeeId == employee.EmployeeId ).FirstOrDefaultAsync();
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
    [HttpGet]
    [CustomAuthorizeE]
    public async Task<IActionResult> EditEmployeeE(Employee employee)
    {
        string ? myData = TempData["EmpId"] as String;
        TempData.Keep("EmpId");
        employee.EmployeeId=myData;
        if(_dbContext.Employee_Details!=null){
            var employees = await _dbContext.Employee_Details.Where(x => x.EmployeeId == employee.EmployeeId ).FirstOrDefaultAsync();
            return View(employees);
        }
        return View();
    }
    [HttpPost]
    [CustomAuthorizeE]
    public async Task<IActionResult> EditEmployeeE(Employee employee, IFormFile image1)
    {
        string ? myData = TempData["EmpId"] as string;
        TempData.Keep("EmpId");
        employee.EmployeeId = myData;
        ModelState.Remove("EmployeeVacationStartTime");
        ModelState.Remove("EmployeeVacationEndTime");
        ModelState.Remove("EmployeeTrainingStartTime");
        ModelState.Remove("EmployeeTrainingEndTime");
        ModelState.Remove("EmployeeAge");
        ModelState.Remove("EmployeeConfirmPassword");
        if (ModelState.IsValid)
        {
            try
            {
                if(_dbContext.Employee_Details!=null){
                    var employees = await _dbContext.Employee_Details.FindAsync(employee.EmployeeId);
                    if (employees != null)
                    {
                        employees.EmployeeName = employee.EmployeeName;
                        employees.EmployeeNumber = employee.EmployeeNumber;
                        employees.EmployeeEmail = employee.EmployeeEmail;
                        
                        if (image1 != null && image1.Length > 0)
                        { 
                            using (var memoryStream = new MemoryStream())
                            {
                                await image1.CopyToAsync(memoryStream);
                                employees.EmployeeImage = memoryStream.ToArray();
                            }
                        }
                        await _dbContext.SaveChangesAsync();
                        var url = Url.Action("EmployeeViewE", "Employee", new { id = employee.EmployeeId });
                        if (url != null)
                        {
                            return Redirect(url);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Employee not found");
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, $"Something went wrong: {exception.Message}");
            }
        }

        return View(employee);
    }
    [HttpPost]
    [CustomAuthorizeE]
    public async Task<IActionResult> VacationRequestE(Employee employee,int a)
    {
        string ? myData = TempData["EmpId"] as string;
        TempData.Keep("EmpId");
        employee.EmployeeId = myData;
        ModelState.Remove("EmployeeTrainingStartTime");
        ModelState.Remove("EmployeeTrainingEndTime");
        ModelState.Remove("EmployeeAge");
        ModelState.Remove("EmployeeConfirmPassword");
        if (ModelState.IsValid)
        {
            try
            {
                if(_dbContext.Employee_Details!=null){
                    var employees = await _dbContext.Employee_Details.FindAsync(employee.EmployeeId);
                    if (employees != null)
                    {
                        employees.EmployeeVacationStatus = "Not Yet";
                        employees.EmployeeVacationEndTime =employee.EmployeeVacationEndTime ;
                        employees.EmployeeVacationStartTime = employee.EmployeeVacationStartTime;
                        employees.EmployeeVacationReason=employee.EmployeeVacationReason;
                        await _dbContext.SaveChangesAsync();
                        var url = Url.Action("EmployeeViewE", "Employee", new { id = employee.EmployeeId });
                        if (url != null)
                        {
                            return Redirect(url);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Employee not found");
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, $"Something went wrong: {exception.Message}");
            }
        }
        return View(employee);
    }
    [CustomAuthorizeE]
    public ActionResult LogoutE()
    {
        return RedirectToAction("LoginE", "Employee");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

