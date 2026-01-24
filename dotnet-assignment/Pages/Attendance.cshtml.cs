using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dotnet_assignment.Data;
using dotnet_assignment.Model;

namespace dotnet_assignment.Pages;

public class AttendanceModel : PageModel
{
    private readonly AppDbContext _context;

    public AttendanceModel(AppDbContext context)
    {
        _context = context;
    }

 
    public List<Member> Students { get; set; } = new();

   
    [BindProperty]
    public DateOnly? AttendanceDate { get; set; }

    
    [BindProperty]
    public Dictionary<int, bool> AttendanceData { get; set; } = new();

  
    public bool IsTeacher =>
        HttpContext.Session.GetString("UserRole") == "Teacher";

    
    public IActionResult OnGet()
    {
        if (!IsTeacher)
            return RedirectToPage("/Index");

        Students = _context.Members
            .Where(m => m.Role == "Student")
            .ToList();

        return Page();
    }

    
    public IActionResult OnPost()
    {
        if (!IsTeacher)
            return RedirectToPage("/Index");

        Students = _context.Members
            .Where(m => m.Role == "Student")
            .ToList();

       
        DateOnly date = AttendanceDate ?? DateOnly.FromDateTime(DateTime.Today);

        foreach (var student in Students)
        {
            bool isPresent =
                AttendanceData.TryGetValue(student.Id, out bool value) && value;

            var existing = _context.Attendances
                .FirstOrDefault(a =>
                    a.MemberId == student.Id &&
                    a.Date == date);

            if (existing == null)
            {
                _context.Attendances.Add(new Attendance
                {
                    MemberId = student.Id,
                    Date = date,
                    IsPresent = isPresent,
                    MarkedAt = DateTime.UtcNow
                });
            }
            else
            {
                existing.IsPresent = isPresent;
                existing.MarkedAt = DateTime.UtcNow;
            }
        }

        _context.SaveChanges();
        TempData["SuccessMessage"] = "Attendance saved successfully!";
        return RedirectToPage("/Attendance");
    }
}
