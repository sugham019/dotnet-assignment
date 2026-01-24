using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dotnet_assignment.Data;
using System.Linq;

namespace dotnet_assignment.Pages;

public class LoginModel : PageModel
{
    private readonly AppDbContext _context;

    public LoginModel(AppDbContext context) => _context = context;

    [BindProperty]
    public string Email { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public string ErrorMessage { get; set; }

    public void OnGet() { }

    public IActionResult OnPost()
    {
        var user = _context.Members.FirstOrDefault(m => m.Email == Email && m.Password == Password);

        if (user != null)
        {
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserRole", user.Role);
            HttpContext.Session.SetInt32("UserId", user.Id);

            if (user.Role == "Teacher")
            {
                return RedirectToPage("/Attendance");
            }
            return RedirectToPage("/Index");
        }
        ErrorMessage = "Invalid login attempt.";
        return Page();
    }
}