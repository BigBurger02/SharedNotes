using SharedNotes.Interfaces;
using SharedNotes.Model;

namespace SharedNotes.Data;

public static class DbInitializer
{
    public static async void Initialize(NotesContext context, IElasticsearchService elasticsearch)
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
            new Note { Title = "Third", Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. three", Created = DateTime.UtcNow, LastEdit = DateTime.UtcNow },
            new Note { Title = "Fourth", Body = "Lorem ipsum dolor sit amet four", Created = DateTime.UtcNow, LastEdit = DateTime.UtcNow },
            new Note { Title = "Fifth", Body = "Lorem ipsum dolor sit amet five", Created = DateTime.UtcNow, LastEdit = DateTime.UtcNow },
        };
        foreach (var item in notes)
        {
            context.Notes.Add(item);
        }
        context.SaveChanges();

        await elasticsearch.DeleteIndexIfExistsAsync("notes-index");
        await elasticsearch.CreateIndexIfNotExistsAsync("notes-index");
        await elasticsearch.IndexDocsAsync(notes, "notes-index");
    }
}