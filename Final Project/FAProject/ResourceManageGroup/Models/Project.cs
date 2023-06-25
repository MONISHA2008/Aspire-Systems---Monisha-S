namespace ResourceManageGroup.Models;
using System.ComponentModel.DataAnnotations;

public class Project{
    public int project_id { get; set; }
    [RegularExpression(@"^[0-9a-zA-Z\s]+$", ErrorMessage = "Project name must contain only letters and spaces and numbers.")]
    public string? project_name { get; set; }
    [RegularExpression(@"^[0-9a-zA-Z\s]+$", ErrorMessage = "Project Description must contain only Numbers ,letters and spaces.")]
    public string? project_description { get; set; }
    [RegularExpression(@"^(\d{4})-(\d{2})-(\d{2})$", ErrorMessage = "Invalid date format.Date Format is YYYY-MM-DD")]
    public string? project_start_time{ get; set;}
    [RegularExpression(@"^(\d{4})-(\d{2})-(\d{2})$", ErrorMessage = "Invalid date format.Date Format is YYYY-MM-DD")]
    public string? project_end_time{ get; set;}
    public string? project_lead{ get; set;}
    public string? project_type { get; set; }
    public List<string> myList = new List<string>();
}