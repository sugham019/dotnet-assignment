using dotnet_assignment.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using dotnet_assignment.Model;

namespace dotnet_assignment.Pages
{
    public class AttendanceSummaryModel : PageModel
    {
        private readonly AppDbContext _context;

        public AttendanceSummaryModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Attendance> AttendanceRecords { get; set; } = new List<Attendance>();

        [BindProperty(SupportsGet = true)]
        public int? StudentId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateOnly? Date { get; set; }

        public List<Member> Students { get; set; }

        public async Task OnGetAsync()
        {
            Students = await _context.Members
                .Where(m => m.Role == "Student")
                .OrderBy(m => m.FullName)
                .ToListAsync();

            var query = _context.Attendances
                .Include(a => a.Member)
                .AsQueryable();

            if (StudentId.HasValue)
            {
                query = query.Where(a => a.MemberId == StudentId.Value);
            }

            if (Date.HasValue)
            {
                query = query.Where(a => a.Date == Date.Value);
            }
            AttendanceRecords = await query.OrderByDescending(a => a.Date).ToListAsync();
        }
    }
}