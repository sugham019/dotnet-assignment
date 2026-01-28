using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dotnet_assignment.Data;
using dotnet_assignment.Model;

namespace dotnet_assignment.Pages;

public class LeaveRequestModel : PageModel
{
    private readonly AppDbContext _context;

    public LeaveRequestModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public DateOnly StartDate { get; set; }

    [BindProperty]
    public DateOnly EndDate { get; set; }

    [BindProperty]
    public string Reason { get; set; }

    public List<LeaveRequest> LeaveRequests { get; set; } = new();

    public string ErrorMessage { get; set; }
    public string SuccessMessage { get; set; }

    public IActionResult OnGet()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToPage("/Login");
        }

        LoadLeaveRequests(userId.Value);
        return Page();
    }

    public IActionResult OnPost()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToPage("/Login");
        }

        if (EndDate < StartDate)
        {
            ErrorMessage = "End date cannot be earlier than start date.";
            LoadLeaveRequests(userId.Value);
            return Page();
        }

        var leaveRequest = new LeaveRequest
        {
            MemberId = userId.Value,
            StartDate = StartDate,
            EndDate = EndDate,
            Reason = Reason,
            LeaveStatus = "Pending",
            RequestedAt = DateTime.UtcNow
        };

        _context.LeaveRequests.Add(leaveRequest);
        _context.SaveChanges();

        SuccessMessage = "Leave request submitted successfully.";

        LoadLeaveRequests(userId.Value);
        ModelState.Clear();

        return Page();
    }

    private void LoadLeaveRequests(int memberId)
    {
        LeaveRequests = _context.LeaveRequests
            .Where(l => l.MemberId == memberId)
            .OrderByDescending(l => l.RequestedAt)
            .ToList();
    }
}
