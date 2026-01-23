namespace dotnet_assignment.Model;

public class Attendance
{
    public int Id { get; set; }

    public int MemberId { get; set; }
    public Member Member { get; set; }

    public DateOnly Date { get; set; }

    public bool IsPresent { get; set; }

    public DateTime MarkedAt { get; set; } = DateTime.UtcNow;
}