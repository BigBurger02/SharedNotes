using Microsoft.EntityFrameworkCore;
using SharedNotes.Data;
using SharedNotes.Interfaces;
using SharedNotes.Model;

namespace SharedNotes.Services;

public class NotesRepository : INotesRepository
{
    private NotesContext _context;

    public NotesRepository(NotesContext context)
    {
        _context = context;
    }

    public async Task<int> Commit()
    {
        return await _context.SaveChangesAsync();
    }

    public IQueryable<Note> GetAllNotesOrderByLastEditDesc()
    {
        return _context.Notes
            .OrderByDescending(l => l.LastEdit);
    }

    public async Task AddNoteAsync(Note note)
    {
        await _context.Notes
            .AddAsync(note);
    }
}