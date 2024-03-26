namespace SharedNotes.Model;

public class Note
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastEdit { get; set; }
}