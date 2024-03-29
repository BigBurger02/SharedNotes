using System.Collections;
using Microsoft.EntityFrameworkCore;
using SharedNotes.Data;
using SharedNotes.Interfaces;
using SharedNotes.Model;

namespace SharedNotes.Services;

public class NotesRepository : INotesRepository
{
    private readonly NotesContext _context;

    public NotesRepository(NotesContext context)
    {
        _context = context;
    }
    
    public int Commit()
    {
        return _context.SaveChanges();
    }
    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public IEnumerable<Note> GetAllNotesOrderByLastEditDesc()
    {
        return _context.Notes
            .OrderByDescending(l => l.LastEdit)
            .AsEnumerable();
    }

    public void AddNote(Note note)
    {
        _context.Notes
            .Add(note);
    }
    public async Task AddNoteAsync(Note note)
    {
        await _context.Notes
            .AddAsync(note);
    }
}