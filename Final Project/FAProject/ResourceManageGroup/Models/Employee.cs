namespace ResourceManageGroup.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
public class Employee
{   
    [Key]
    public string? EmployeeId { get; set; }
    [StringLength(maximumLength: 20, ErrorMessage = "Employee name must be between {2} and {1} characters.", MinimumLength = 2)]
    [RegularExpression(@"^[A-Z][a-zA-Z]*(?:\s[A-Z][a-zA-Z]*)*$", ErrorMessage = "Employee name must contain only letters and spaces.")]
    public string? EmployeeName { get; set; }
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
    public string? EmployeeEmail { get; set; }
    [RegularExpression(@"^\+?[6-9][0-9]{9,11}$", ErrorMessage = "Invalid phone number format.")]
    public string? EmployeeNumber { get; set; }
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[a-zA-Z\d@$!%*?&]{8,}$", ErrorMessage = "Password must contain at least 8 characters, one uppercase letter, one lowercase letter, one digit, and one special character.")]
    public string? EmployeePassword { get; set; }
    //[Compare("EmployeePassword", ErrorMessage = "The password and confirmation password do not match.")]
    public string? EmployeeConfirmPassword { get; set; }
    public DateTime DateOfBirth{get; set;}
    public string? EmployeeVacationStartTime { get; set; }
    //[VacationDate(ErrorMessage = "Invalid vacation dates.")]
    public string? EmployeeVacationEndTime { get; set; }
    [RegularExpression(@"^[0-9a-zA-Z\s]+$", ErrorMessage = "Employee Reason must contain only Numbers, letters, and spaces.")]
    public string? EmployeeVacationReason { get; set; }
    public byte[]? EmployeeImage { get; set; }
    // The Values Will Be Assigned from HR- Manager Part 
    public string? EmployeeWorkingStatus { get; set; }
    public string? EmployeeVacationStatus { get; set; }
    public string? EmployeeTechnology { get; set; }
    public string? EmployeeTrainerName { get; set; }
    public string? EmployeeTrainingStartTime { get; set; }
    public string? EmployeeTrainingEndTime { get; set; }
    // The Values Will Be Assigned from Project - Manager Part
    public string? EmployeeProject { get; set; }
    
}
