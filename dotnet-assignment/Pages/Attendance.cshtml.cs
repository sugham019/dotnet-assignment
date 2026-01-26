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
    public List<Attendance> ExistingAttendance { get; set; } = new();

    [BindProperty]
    public DateOnly? AttendanceDate { get; set; }

    public bool IsTeacher =>
        HttpContext.Session.GetString("UserRole") == "Teacher";

    public IActionResult OnGet()
    {
        if (!IsTeacher)
            return RedirectToPage("/Index");

        Students = _context.Members
            .Where(m => m.Role == "Student")
            .ToList();

        DateOnly date = AttendanceDate ?? DateOnly.FromDateTime(DateTime.Today);

        ExistingAttendance = _context.Attendances
            .Where(a => a.Date == date)
            .ToList();

        return Page();
    }
    
    public IActionResult OnPostAutoSave(int memberId, bool isPresent, string date)
    {
        if (!IsTeacher)
            return new JsonResult(false);

        DateOnly attendanceDate = string.IsNullOrEmpty(date)
            ? DateOnly.FromDateTime(DateTime.Today)
            : DateOnly.Parse(date);

        var existing = _context.Attendances
            .FirstOrDefault(a => a.MemberId == memberId && a.Date == attendanceDate);

        if (existing == null)
        {
            _context.Attendances.Add(new Attendance
            {
                MemberId = memberId,
                Date = attendanceDate,
                IsPresent = isPresent,
                MarkedAt = DateTime.UtcNow
            });
        }
        else
        {
            existing.IsPresent = isPresent;
            existing.MarkedAt = DateTime.UtcNow;
        }

        _context.SaveChanges();
        return new JsonResult(true);
    }
    
    public IActionResult OnGetLoadAttendance(string date)
    {
        if (!IsTeacher)
            return new JsonResult(new List<object>());

        DateOnly attendanceDate = string.IsNullOrEmpty(date)
            ? DateOnly.FromDateTime(DateTime.Today)
            : DateOnly.Parse(date);

        var attendance = _context.Attendances
            .Where(a => a.Date == attendanceDate)
            .Select(a => new
            {
                memberId = a.MemberId,
                isPresent = a.IsPresent
            })
            .ToList();

        return new JsonResult(attendance);
    }
}