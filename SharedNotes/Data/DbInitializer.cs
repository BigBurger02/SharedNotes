using SharedNotes.Model;

namespace SharedNotes.Data;

public static class DbInitializer
{
    public static void Initialize(NotesContext context)
    {
        context.Database.EnsureCreated();
        if (context.Notes.Any())
        {
            return;
        }

        var notes = new[]
        {
            new Note { Title = "First", Body = "Lorem ipsum dolor sit amet one", Created = DateTime.UtcNow, LastEdit = DateTime.UtcNow },
            new Note { Title = "Second", Body = "Lorem ipsum dolor sit amet two", Created = DateTime.UtcNow, LastEdit = DateTime.UtcNow },
            new Note { Title = "Third", Body = "Lorem ipsum dolor sit amet three", Created = DateTime.UtcNow, LastEdit = DateTime.UtcNow },
            new Note { Title = "Fourth", Body = "Lorem ipsum dolor sit amet four", Created = DateTime.UtcNow, LastEdit = DateTime.UtcNow },
            new Note { Title = "Fifth", Body = "Lorem ipsum dolor sit amet five", Created = DateTime.UtcNow, LastEdit = DateTime.UtcNow },
        };
        foreach (var item in notes)
        {
            context.Notes.Add(item);
        }
        context.SaveChanges();
    }
}