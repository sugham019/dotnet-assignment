namespace dotnet_assignment.Model;

public class Member
{
    public int Id { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public string Role { get; set; }
    
    public string Password { get; set; }
    
    public ICollection<Attendance>? Attendances { get; set; } = new List<Attendance>();
    public ICollection<LeaveRequest>? LeaveRequests { get; set; } = new List<LeaveRequest>();
}