using SharedNotes.Model;

namespace SharedNotes.Interfaces;

public interface INotesRepository
{
    Task<int> Commit();
    
    IQueryable<Note> GetAllNotesOrderByLastEditDesc();
    Task AddNoteAsync(Note note);
}