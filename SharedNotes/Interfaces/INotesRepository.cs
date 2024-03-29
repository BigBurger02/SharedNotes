using SharedNotes.Model;

namespace SharedNotes.Interfaces;

public interface INotesRepository
{
    int Commit();
    Task<int> CommitAsync();
    
    IEnumerable<Note> GetAllNotesOrderByLastEditDesc();
    
    void AddNote(Note note);
    Task AddNoteAsync(Note note);

    void ReloadNote(Note note);
    Task ReloadNoteAsync(Note note);
}