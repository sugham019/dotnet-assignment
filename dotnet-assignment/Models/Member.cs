namespace dotnet_assignment.Data;

public class Member
{
    public int Id { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public string Role { get; set; }

    public ICollection<Attendance> Attendances { get; set; }
    public ICollection<LeaveRequest> LeaveRequests { get; set; }
}