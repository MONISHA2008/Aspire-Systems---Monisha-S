namespace ResourceManageGroup.Models;
using System;
using System.ComponentModel.DataAnnotations;

public class ValidationUser{
    public string ? VerifyCode { get; set; }
    public List<string> MyList { get; set; } = new List<string>();
}

public class VacationDateAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        try{
            
            var startTimeProperty = validationContext.ObjectType.GetProperty("EmployeeVacationStartTime");
            var endTimeProperty = validationContext.ObjectType.GetProperty("EmployeeVacationEndTime");
            if (startTimeProperty == null || endTimeProperty == null)
                throw new ArgumentException("Invalid property names");
            var startTimeValue = startTimeProperty.GetValue(validationContext.ObjectInstance) as string;
            var endTimeValue = endTimeProperty.GetValue(validationContext.ObjectInstance) as string;
            if (!DateTime.TryParse(startTimeValue, out DateTime startTime) || !DateTime.TryParse(endTimeValue, out DateTime endTime))
            {
                return new ValidationResult("Invalid date format. Date format should be YYYY-MM-DD.");
            }
            if (startTime > endTime)
            {
                return new ValidationResult("Start date should be earlier than the end date.");
            }
            if(endTime.AddMonths(-3) > startTime)
            {
                return new ValidationResult("Vacation is valid only for 3 months.");
            }
            return ValidationResult.Success;
        }
        catch (Exception exception)
        {
            Console.WriteLine("An exception occurred: " + exception.Message);
            return new ValidationResult("An error occurred during validation.");
        }
    }
}
