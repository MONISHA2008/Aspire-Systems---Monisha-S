using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ResourceManageGroup.Models;
using ResourceManageGroup.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;
using System.Net.Mail;

namespace ResourceManageGroup.Controllers;
public class RecruiterController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    public RecruiterController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    [HttpGet]
    public IActionResult RegisterR()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> RegisterR(Recruiter recruiter, ValidationUser validation)
    {
        if (ModelState.IsValid)
        {
            try
            {
                int emailcount = 0;
                int numcount = 0;
                if (_dbContext.Recruiter_Details != null)
                {
                    emailcount = _dbContext.Recruiter_Details.Where(c => c.RecruiterEmail == recruiter.RecruiterEmail).Count();
                    numcount = _dbContext.Recruiter_Details.Where(c => c.RecruiterNumber == recruiter.RecruiterNumber).Count();
                    if (recruiter.RecruiterEmail != null && recruiter.RecruiterNumber != null && recruiter.RecruiterPassword != null)
                                    if (emailcount > 0)
                                    {
                                        ViewData["Message"] = "The User Already Exists !!!!!";
                                    }
                                    else if (numcount > 0)
                                    {
                                        ViewData["Message"] = "The User Already Exists !!!!!";
                                    }
                                    else
                                    {
                                        _dbContext.Add(recruiter);
                                        await _dbContext.SaveChangesAsync();
                                        ViewData["Message"] = "Succesfully Registered , Log in to Work";
                                    }
                                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(String.Empty, $"Something Went Wrong{exception.Message}");
            }
        }
        ModelState.AddModelError(String.Empty, $"Something Went Wrong Invalid Model");
        return View(recruiter);
    }
    [HttpGet]
    public IActionResult forgotPasswordR()
    {
        return View();
    }
    [HttpGet]
    public IActionResult VerifyUserR()
    {
        return View();
    }
    [HttpPost]
    public IActionResult VerifyUserR(ValidationUser validation, Recruiter recruiter)
    {
        string? myData = TempData["verificationcode"] as String;
        string? email = TempData["email"] as String;
        if (myData == validation.VerifyCode)
        {
            recruiter.RecruiterEmail = email;
            HttpContext.Session.SetObjectAsJson("users", recruiter);
            return RedirectToAction("EmployeeListR", "Recruiter");
        }
        else
        {
            ViewBag.message = "OTP is Invalid";
        }
        return View();
    }
    [HttpPost]
    public IActionResult forgotPasswordR(Recruiter recruiter, ValidationUser validation)
    {
        if (ModelState.IsValid)
        {
            try
            {
                int count = 0;
                if (_dbContext.Recruiter_Details != null)
                {
                    count = _dbContext.Recruiter_Details.Where(c => c.RecruiterEmail == recruiter.RecruiterEmail).Count();
                    if (recruiter.RecruiterEmail != null){
                        if (count > 0)
                        {
                            try
                            {
                                Random random = new Random();
                                string code = random.Next(100000, 999999).ToString();
                                TempData["verificationcode"] = code;
                                TempData["email"] = recruiter.RecruiterEmail;
                                string from, pass, messageBody;
                                MailMessage message = new MailMessage();
                                from = "kishorevijaykumar26@gmail.com";
                                pass = "iepcxotvxhfxjbjc";
                                messageBody = "Your Verification Code is " + code;
                                if (recruiter.RecruiterEmail != null)
                                {
                                    message.To.Add(new MailAddress(recruiter.RecruiterEmail));
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
                                return RedirectToAction("VerifyUserR", "Recruiter");
                            }
                            catch (Exception ex)
                            {
                                // Handle any errors that occur while sending the email
                                ViewData["Message"] = "Error sending verification code: " + ex.Message;
                            }
                        }
                        else
                        {
                            ViewData["Message"] = "Check Your E-Mail";
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(String.Empty, $"Something Went Wrong{exception.Message}");
            }
        }
        return View(recruiter);
    }
    [HttpGet]
    public IActionResult LoginR(Recruiter recruiter)
    {
        var cookieValue = Request.Cookies["MyRAuthCookie"];
        if (cookieValue != null)
        {
            var decodedCookie = cookieValue.Split(":");
            recruiter.RecruiterEmail = decodedCookie[0];
            recruiter.RecruiterPassword = decodedCookie[1];
            ViewBag.email = recruiter.RecruiterEmail;
            ViewBag.password = recruiter.RecruiterPassword;
        }
        return View();
    }
    [HttpPost]
    public IActionResult LoginR(Recruiter recruiter, ValidationUser validation)
    {
        if (ModelState.IsValid)
        {
            try
            {
                int count = 0;
                string? dbpassword = " ";
                if (_dbContext.Recruiter_Details != null)
                {
                    count = _dbContext.Recruiter_Details.Where(c => c.RecruiterEmail == recruiter.RecruiterEmail).Count();
                    dbpassword = _dbContext.Recruiter_Details.Where(c => c.RecruiterEmail == recruiter.RecruiterEmail).Select(c => c.RecruiterPassword).FirstOrDefault();
                    if (recruiter.RecruiterEmail != null && recruiter.RecruiterPassword != null){
                        if (count > 0)
                        {
                            if (dbpassword == recruiter.RecruiterPassword)
                            {
                                HttpContext.Session.SetObjectAsJson("users", recruiter);
                                //Cookies 
                                var cookieValue = $"{recruiter.RecruiterEmail}:{recruiter.RecruiterPassword}";
                                var cookieOptions = new CookieOptions
                                {
                                    Expires = DateTimeOffset.Now.AddDays(30),
                                    HttpOnly = true
                                };
                                Response.Cookies.Append("MyRAuthCookie", cookieValue, cookieOptions);
                                return RedirectToAction("EmployeeListR", "Recruiter");
                            }
                            else
                            {
                                ViewData["Message"] = "Check Your Password";
                            }
                        }
                        else
                        {
                            ViewData["Message"] = "Check Your E-Mail";
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(String.Empty, $"Something Went Wrong{exception.Message}");
            }
        }
        return View(recruiter);
    }
    [HttpGet]
    [CustomAuthorizeR]
    public async Task<IActionResult> EmployeeListR()
    {
        if (_dbContext.Employee_Details != null)
        {
            var employees = await _dbContext.Employee_Details.ToListAsync();
            return View(employees);
        }
        return View();
    }
    [HttpGet]
    [CustomAuthorizeR]
    public async Task<IActionResult> VacationReqListR()
    {
        if (_dbContext.Employee_Details != null)
        {
            var employees = await _dbContext.Employee_Details.Where(e => e.EmployeeVacationStatus == "Not Yet" || e.EmployeeVacationStatus == "Approved").ToListAsync();
            return View(employees);
        }
        return View();
    }
    public IActionResult GetImage(string id)
    {
        if (_dbContext.Employee_Details != null)
        {
            var employee = _dbContext.Employee_Details.FirstOrDefault(e => e.EmployeeId == id);
            if (employee != null && employee.EmployeeImage != null)
            {
                return File(employee.EmployeeImage, "image/jpeg"); 
            }
        }
        return NotFound();
    }
    [HttpGet]
    [CustomAuthorizeR]
    public async Task<IActionResult> ManageEmployeeR(string id)
    {
        if (_dbContext.Employee_Details != null)
        {
            var employees = await _dbContext.Employee_Details.Where(x => x.EmployeeId == id).FirstOrDefaultAsync();
            return View(employees);
        }
        return View();
    }
    [HttpGet]
    [CustomAuthorizeR]
    public async Task<IActionResult> ManageRequestR(string id, Employee employee)
    {

        if (_dbContext.Employee_Details != null)
        {
            var employees = await _dbContext.Employee_Details.Where(x => x.EmployeeId == id).FirstOrDefaultAsync();
            if (employees != null)
            {
                TempData["reqemail"] = employees.EmployeeEmail;
            }
            TempData.Keep();
            return View(employees);
        }
        return View();
    }
    [HttpGet]
    [CustomAuthorizeR]
    public async Task<IActionResult> ApproveReqR(string id, Employee employee)
    {
        if (_dbContext.Employee_Details != null)
        {
            var employees = await _dbContext.Employee_Details.Where(x => x.EmployeeId == id).FirstOrDefaultAsync();
            {
                return View(employees);
            }
        }
        return View();
    }
    [HttpPost]
    [CustomAuthorizeR]
    public async Task<IActionResult> ApproveReqR(Employee employee)
    {
        if (ModelState.IsValid)
        {
            try
            {
                string? myData = TempData["reqemail"] as string;
                if (myData != null)
                {
                    string temp_email = myData;
                    string from, pass, messageBody;
                    MailMessage message = new MailMessage();
                    from = "kishorevijaykumar26@gmail.com";
                    pass = "iepcxotvxhfxjbjc";
                    messageBody = "Your Vacation have been accepted by Human Resource Manager";
                    if (temp_email != null)
                    {
                        message.To.Add(new MailAddress(temp_email));
                    }
                    else
                    {
                        throw new Exception("User email is null or invalid.");
                    }
                    message.From = new MailAddress(from);
                    message.Body = messageBody;
                    message.Subject = "Vacation Details ";
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                    smtp.EnableSsl = true;
                    smtp.Port = 587;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential(from, pass);
                    smtp.Send(message);
                }
                if (_dbContext.Employee_Details != null)
                {
                    var employees = _dbContext.Employee_Details.FirstOrDefault(p => p.EmployeeId == employee.EmployeeId);
                    if (employees != null)
                    {
                        employees.EmployeeVacationStatus = "Approved";
                    }
                    await _dbContext.SaveChangesAsync();
                }
                var url = Url.Action("VacationReqListR", "Recruiter", new { id = employee.EmployeeId });
                if (url != null)
                {
                    return Redirect(url);
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(String.Empty, $"Something Went Wrong{exception.Message}");
            }
        }
        ModelState.AddModelError(String.Empty, $"Something Went Wrong Invalid Model");
        return View(employee);
    }

    [HttpGet]
    [CustomAuthorizeR]
    public async Task<IActionResult> RejectReqR(string id, Employee employee)
    {
        if (_dbContext.Employee_Details != null)
        {
            var employees = await _dbContext.Employee_Details.Where(x => x.EmployeeId == id).FirstOrDefaultAsync();
            return View(employees);
        }
        return View();
    }
    [HttpPost]
    [CustomAuthorizeR]
    public async Task<IActionResult> RejectReqR(Employee employee)
    {
        if (ModelState.IsValid)
        {
            try
            {
                string? myData = TempData["reqemail"] as string;
                if (myData != null)
                {
                    string temp_email = myData;
                    string from, pass, messageBody;
                    MailMessage message = new MailMessage();
                    from = "kishorevijaykumar26@gmail.com";
                    pass = "iepcxotvxhfxjbjc";
                    messageBody = "Your Vacation have been Rejected by Humar Resource Manager";
                    if (temp_email != null)
                    {
                        message.To.Add(new MailAddress(temp_email));
                    }
                    else
                    {
                        throw new Exception("User email is null or invalid.");
                    }
                    message.From = new MailAddress(from);
                    message.Body = messageBody;
                    message.Subject = "Vacation Details ";
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                    smtp.EnableSsl = true;
                    smtp.Port = 587;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential(from, pass);
                    smtp.Send(message);
                }
                if (_dbContext.Employee_Details != null)
                {
                    var employees = _dbContext.Employee_Details.FirstOrDefault(p => p.EmployeeId == employee.EmployeeId);
                    if (employees != null)
                    {
                        employees.EmployeeVacationReason = "Not Assigned";
                        employees.EmployeeVacationStatus = "Not Assigned";
                        employees.EmployeeVacationStartTime = "Not Assigned";
                        employees.EmployeeVacationEndTime = "Not Assigned";
                    }
                    await _dbContext.SaveChangesAsync();
                    var url = Url.Action("VacationReqListR", "Recruiter", new { id = employee.EmployeeId });
                    if (url != null)
                    {
                        return Redirect(url);
                    }
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(String.Empty, $"Something Went Wrong{exception.Message}");
            }
        }
        ModelState.AddModelError(String.Empty, $"Something Went Wrong Invalid Model");
        return View(employee);
    }
    [HttpGet]
    [CustomAuthorizeR]
    public async Task<IActionResult> AssignDomainR(string id, Employee employee,ValidationUser validationuser)
    {
        employee.EmployeeTechnology = null;
        validationuser.MyList.AddRange(new string[] { "Java", "C#", "Angular", "React" });
        ViewBag.list = validationuser.MyList;
        if (_dbContext.Employee_Details != null)
        {
            var employees = await _dbContext.Employee_Details.Where(x => x.EmployeeId == id).FirstOrDefaultAsync();
            return View(employees);
        }
        return View();
    }
    [HttpPost]
    [CustomAuthorizeR]
    public async Task<IActionResult> AssignDomainR(Employee employee)
    {
        if (ModelState.IsValid)
        {
            try
            {
                if (_dbContext.Employee_Details != null)
                {
                    var employees = _dbContext.Employee_Details.FirstOrDefault(p => p.EmployeeId == employee.EmployeeId);
                    if (employees != null)
                    {
                        employees.EmployeeTechnology = employee.EmployeeTechnology;
                    }
                    await _dbContext.SaveChangesAsync();
                    var url = Url.Action("ManageEmployeeR", "Recruiter", new { id = employee.EmployeeId });
                    if (url != null)
                    {
                        return Redirect(url);
                    }
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(String.Empty, $"Something Went Wrong{exception.Message}");
            }
        }
        ModelState.AddModelError(String.Empty, $"Something Went Wrong Invalid Model");
        return View(employee);
    }
    [HttpGet]
    [CustomAuthorizeR]
    public async Task<IActionResult> AssignWorkR(string id, Employee employee)
    {
        if (_dbContext.Employee_Details != null)
        {
            var employees = await _dbContext.Employee_Details.Where(x => x.EmployeeId == id).FirstOrDefaultAsync();
            return View(employees);
        }
        return View();
    }
    [HttpPost]
    [CustomAuthorizeR]
    public async Task<IActionResult> AssignWorkR(Employee employee)
    {
        if (ModelState.IsValid)
        {
            try
            {
                if (_dbContext.Employee_Details != null)
                {
                    var employees = await _dbContext.Employee_Details.Where(x => x.EmployeeId == employee.EmployeeId).FirstOrDefaultAsync();
                    if (employees != null)
                    {
                        employees.EmployeeWorkingStatus = "Bench";
                        employees.EmployeeTrainerName = "Not Assigned";
                        employees.EmployeeTrainingStartTime = "Not Assigned";
                        employees.EmployeeTrainingEndTime = "Not Assigned";
                    }
                    await _dbContext.SaveChangesAsync();
                    var url = Url.Action("ManageEmployeeR", "Recruiter", new { id = employee.EmployeeId });
                    if (url != null)
                    {
                        return Redirect(url);
                    }
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(String.Empty, $"Something Went Wrong{exception.Message}");
            }
        }
        ModelState.AddModelError(String.Empty, $"Something Went Wrong Invalid Model");
        return View(employee);
    }
    [HttpGet]
    [CustomAuthorizeR]
    public async Task<IActionResult> AssignTrainingR(string id, Employee employee,ValidationUser validationuser)
    {
        employee.EmployeeTrainerName = null;
        validationuser.MyList.AddRange(new string[] { "Sabapathi", "Savitha", "Silpa", "Jaya", "Anitha", "Saraswathi" });
        ViewBag.list = validationuser.MyList;
        if (_dbContext.Employee_Details != null)
        {
            var employees = await _dbContext.Employee_Details.Where(x => x.EmployeeId == id).FirstOrDefaultAsync();
            return View(employees);
        }
        return View();
    }
    [HttpPost]
    [CustomAuthorizeR]
    public async Task<IActionResult> AssignTrainingR(Employee employee)
    {
        if (ModelState.IsValid)
        {
            try
            {
                if (_dbContext.Employee_Details != null)
                {
                    var employees = await _dbContext.Employee_Details.Where(x => x.EmployeeId == employee.EmployeeId).FirstOrDefaultAsync();
                    if (employees != null)
                    {
                        employees.EmployeeWorkingStatus = "Training";
                        employees.EmployeeTrainerName = employee.EmployeeTrainerName;
                        employees.EmployeeTrainingStartTime = employee.EmployeeTrainingStartTime;
                        employees.EmployeeTrainingEndTime = employee.EmployeeTrainingStartTime;
                    }
                    await _dbContext.SaveChangesAsync();
                    var url = Url.Action("ManageEmployeeR", "Recruiter", new { id = employee.EmployeeId });
                    if (url != null)
                    {
                        return Redirect(url);
                    }
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(String.Empty, $"Something Went Wrong{exception.Message}");
            }
        }
        ModelState.AddModelError(String.Empty, $"Something Went Wrong Invalid Model");
        return View(employee);
    }
    [HttpGet]
    [CustomAuthorizeR]
    public async Task<IActionResult> RemoveEmployeeR(string id, Employee employee)
    {
        if (_dbContext.Employee_Details != null)
        {
            var employees = await _dbContext.Employee_Details.Where(x => x.EmployeeId == id).FirstOrDefaultAsync();
            return View(employees);
        }
        return View();
    }
    [HttpPost]
    [CustomAuthorizeR]
    public async Task<IActionResult> RemoveEmployeeR(Employee employee)
    {
        if (ModelState.IsValid)
        {
            try
            {
                if (_dbContext.Employee_Details != null)
                {
                    var employees = _dbContext.Employee_Details.FirstOrDefault(p => p.EmployeeId == employee.EmployeeId);
                    if (employees != null)
                    {
                        _dbContext.Remove(employees);
                        await _dbContext.SaveChangesAsync();
                        return RedirectToAction("EmployeeListR");
                    }
                }
            }

            catch (Exception exception)
            {
                ModelState.AddModelError(String.Empty, $"Something Went Wrong{exception.Message}");
            }
        }
        ModelState.AddModelError(String.Empty, $"Something Went Wrong Invalid Model");
        return View(employee);
    }
    [CustomAuthorizeR]
    public IActionResult LogoutR()
    {
        return RedirectToAction("LoginR", "Recruiter");
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}