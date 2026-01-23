using Microsoft.AspNetCore.Mvc.RazorPages;
using  dotnet_assignment.Data;
using dotnet_assignment.Model;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_assignment.Pages;

public class AttendanceModel : PageModel
{
    private readonly AppDbContext _context;

    public AttendanceModel(AppDbContext context)
    {
        _context = context;
    }
    
    public List<Member> Students { get; set; } 
    
    [BindProperty]
    public DateTime ? AttendanceDate { get; set; }
    
    [BindProperty]
    public Dictionary<int, bool> AttendanceData { get; set; }

    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("Role") != "Teacher")
        {
            return RedirectToPage("/Index");
        }
        
        Students = _context.Members
            .Where(m=> m.Role == "Student")
            .ToList();
        return Page();
    }

    public IActionResult OnPost()
    {
        if (HttpContext.Session.GetString("Role") != "Teacher")
        {
            return RedirectToPage("/Index");
        }
        
        DateOnly date = AttendanceDate == null
            ? DateOnly.FromDateTime(DateTime.Today)
            : DateOnly.FromDateTime(AttendanceDate.Value);

        foreach (var item  in AttendanceData)
        {
            Attendance attendance = new Attendance
            {
                MemberId = item.Key,
                Date = date,
                IsPresent = item.Value
            };
            
            _context.Attendances.Add(attendance);
        }
        
        _context.SaveChanges();
        
        return RedirectToPage("/Index");
        
    }
}