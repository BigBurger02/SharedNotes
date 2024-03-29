using Microsoft.EntityFrameworkCore;
using SharedNotes.Data;
using SharedNotes.Model;

namespace SharedNotes.Tests.Services;

public class RepositoryTestsInit
{
    protected static Mock<NotesContext> mockContext;
    protected static Mock<DbSet<Note>> mockNotesDbSet;

    public static void Init()
    {
        SetupNotes();
        mockContext = new Mock<NotesContext>();
        mockContext
            .Setup(s => s.Notes)
            .Returns(mockNotesDbSet.Object);
    }

    private static void SetupNotes()
    {
        mockNotesDbSet = new Mock<DbSet<Note>>();
    }
}