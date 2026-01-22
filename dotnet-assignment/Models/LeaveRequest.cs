namespace dotnet_assignment.Data;

public class LeaveRequest
{
    public int Id { get; set; }

    public int MemberId { get; set; }
    public Member Member { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public string Reason { get; set; }

    public string LeaveStatus { get; set; } // Either pending, approved or rejected

    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
}