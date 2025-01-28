namespace homework17.Models;

public class Subject
{
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Grade> Grades { get; set; }
}