namespace homework18.Models;

public class ToDo
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsDone { get; set; }
    public string UserId { get; set; }
    public required User User { get; set; }
}