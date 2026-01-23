using dotnet_assignment.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using dotnet_assignment.Model;

namespace dotnet_assignment.Pages;

public class RegisterModel : PageModel
{
    private readonly AppDbContext _context;

    public RegisterModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Member Input { get; set; }

    public string Message { get; set; }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        ModelState.Remove("Input.Attendances");
        ModelState.Remove("Input.LeaveRequests");

        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Members.Add(Input);
        _context.SaveChanges();

        Message = "Registration successful!";
        
        Input = new Member(); 
        ModelState.Clear();

        return Page();
    }
}