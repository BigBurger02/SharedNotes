using Microsoft.EntityFrameworkCore;
using SharedNotes.Data;
using SharedNotes.Model;
using SharedNotes.Services;

namespace SharedNotes.Tests.Services;

public class NotesRepositoryTests : RepositoryTestsInit
{
    public NotesRepositoryTests()
    {
        Init();
    }
    
    [Fact]
    public void Commit_Empty_Success()
    {
        var repository = new NotesRepository(mockContext.Object);

        repository.Commit();
        
        mockContext.Verify(m => m.SaveChanges(), Times.Once());
    }
    
    [Fact]
    public void Commit_Success()
    {
        var repository = new NotesRepository(mockContext.Object);
        
        var dataGenerator = new DataGenerator();
        dataGenerator.GenerateBogusData();

        foreach (var item in dataGenerator.Notes)
        {
            repository.AddNote(item);
        }
        repository.Commit();
        
        mockNotesDbSet.Verify(m => m.Add(It.IsAny<Note>()), Times.Exactly(10));
        mockContext.Verify(m => m.SaveChanges(), Times.Once());
    }
    
    [Fact]
    public void AddNote_ToEmptyRepo_Success()
    {
        var repository = new NotesRepository(mockContext.Object);
        
        repository.AddNote(new Note()
        {
            Title = "One",
            Body = "First note",
            Created = DateTime.UtcNow,
            LastEdit = DateTime.UtcNow,
        });
        
        mockNotesDbSet.Verify(m => m.Add(It.IsAny<Note>()), Times.Once());
    }

    [Fact]
    public void AddNote_TenNotes_Success()
    {
        var repository = new NotesRepository(mockContext.Object);
        
        var dataGenerator = new DataGenerator();
        dataGenerator.GenerateBogusData();

        foreach (var item in dataGenerator.Notes)
        {
            repository.AddNote(item);
        }
        
        mockNotesDbSet.Verify(m => m.Add(It.IsAny<Note>()), Times.Exactly(10));
    }
}