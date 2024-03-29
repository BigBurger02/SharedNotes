using Microsoft.EntityFrameworkCore;
using SharedNotes.Data;
using SharedNotes.Model;
using SharedNotes.Services;

namespace SharedNotes.Tests.Services;

public class NotesRepositoryTests
{
    [Fact]
    public void AddNote_ToEmptyRepo_Success()
    {
        var mockDbSet = new Mock<DbSet<Note>>();
        
        var mockContext = new Mock<NotesContext>();
        mockContext
            .Setup(s => s.Notes)
            .Returns(mockDbSet.Object);
        
        var repository = new NotesRepository(mockContext.Object);
        repository.AddNote(new Note()
        {
            Title = "One",
            Body = "First note",
            Created = DateTime.UtcNow,
            LastEdit = DateTime.UtcNow,
        });
        repository.Commit();
        
        mockDbSet.Verify(m => m.Add(It.IsAny<Note>()), Times.Once());
        mockContext.Verify(m => m.SaveChanges(), Times.Once());
    }
    
   
}