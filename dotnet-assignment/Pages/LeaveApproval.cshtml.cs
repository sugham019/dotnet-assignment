using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using dotnet_assignment.Data;
using dotnet_assignment.Model;

namespace dotnet_assignment.Pages;

public class LeaveApprovalModel : PageModel
{
    private readonly AppDbContext _context;

    public LeaveApprovalModel(AppDbContext context)
    {
        _context = context;
    }

    public List<LeaveRequest> PendingLeaves { get; set; } = new();

    public string ErrorMessage { get; set; }
    public string SuccessMessage { get; set; }

    public IActionResult OnGet()
    {
        var role = HttpContext.Session.GetString("UserRole");
        if (role != "Teacher")
        {
            return RedirectToPage("/");
        }

        LoadPendingLeaves();
        return Page();
    }

    public IActionResult OnPostApprove(int id)
    {
        if (!IsTeacher()) return RedirectToPage("/");

        var leave = _context.LeaveRequests.Find(id);
        if (leave != null)
        {
            leave.LeaveStatus = "Approved";
            _context.SaveChanges();
            SuccessMessage = "Leave approved";
        }

        LoadPendingLeaves();
        return Page();
    }

    public IActionResult OnPostReject(int id)
    {
        if (!IsTeacher()) return RedirectToPage("/");

        var leave = _context.LeaveRequests.Find(id);
        if (leave != null)
        {
            leave.LeaveStatus = "Rejected";
            _context.SaveChanges();
            SuccessMessage = "Leave rejected";
        }

        LoadPendingLeaves();
        return Page();
    }

    private void LoadPendingLeaves()
    {
        PendingLeaves = _context.LeaveRequests
            .Include(l => l.Member)
            .Where(l => l.LeaveStatus == "Pending")
            .OrderBy(l => l.RequestedAt)
            .ToList();
    }

    private bool IsTeacher()
    {
        return HttpContext.Session.GetString("UserRole") == "Teacher";
    }
}
