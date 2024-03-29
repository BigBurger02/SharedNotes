using Bogus;
using SharedNotes.Model;

namespace SharedNotes.Tests;

public class DataGenerator
{
    public List<Note> Notes;

    public void GenerateBogusData()
    {
        Notes = GetNoteGenerator().Generate(10);
    }

    private Faker<Note> GetNoteGenerator()
    {
        return new Faker<Note>()
            .RuleFor(f => f.Id, d => d.IndexFaker)
            .RuleFor(f => f.Title, d => d.Lorem.Word())
            .RuleFor(f => f.Body, d => d.Lorem.Text())
            .RuleFor(f => f.Created, d => d.Date.Past())
            .RuleFor(f => f.LastEdit, d => d.Date.Past());
    }
}