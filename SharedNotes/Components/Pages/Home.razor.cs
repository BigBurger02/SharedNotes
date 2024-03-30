using Microsoft.AspNetCore.Components;
using SharedNotes.Model;

namespace SharedNotes.Components.Pages;

public partial class Home
{
	private List<Note> Notes = null!; // Notes from DB
	private List<Note> ShowNotes = null!; // Notes that will be shown on the page
	private List<Note> FoundNotes = new List<Note>();  // Notes found by ElasticSearch
	
    private int NoteBeingViewed = 0; // 0 - none
    [SupplyParameterFromForm]
    private Note? NoteBeingEdited { get; set; } = null; // null - none
    
    // ElasticSearch
    private string NotesIndexName = "notes-index";
    private string SearchString = String.Empty;
    private bool Searching = false;
    
    
    
    protected override async Task OnInitializedAsync()
    {
    	Notes = Repository.GetAllNotesOrderByLastEditDesc()
    		.ToList();
    }

    private void EditNote(int id)
    {
    	if (id == 0) // new note
    	{
    		NoteBeingEdited = new Note()
    		{
			    Id = 0,
    			Created = DateTime.UtcNow,
    			LastEdit = DateTime.UtcNow,
    		};

    		return;
    	}
	    
    	NoteBeingEdited = Notes.Find(i => i.Id == id);
    }

    private async Task SaveNoteAsync()
    {
	    NoteBeingEdited.LastEdit = DateTime.UtcNow;
	    
    	if (NoteBeingEdited.Id == 0) // new note
    	{
    		await Repository.AddNoteAsync(NoteBeingEdited);
    		await Repository.CommitAsync();

    		Notes.Add(NoteBeingEdited);
    	}
    	else
    	{
    		await Repository.CommitAsync();
    	}

    	await Elasticsearch.IndexDocAsync(NoteBeingEdited, NotesIndexName);
	    
    	NoteBeingEdited = null;

    	Notes = Notes
    		.OrderByDescending(l => l.LastEdit)
    		.ToList();
    }

    private void CancelNote()
    {
	    Repository.ReloadNote(NoteBeingEdited);
	    NoteBeingEdited = null;
    }
    private async Task CancelNoteAsync()
    {
    	await Repository.ReloadNoteAsync(NoteBeingEdited);
    	NoteBeingEdited = null;
    }

    private async Task SearchAsync()
    {
    	if (string.IsNullOrWhiteSpace(SearchString))
    	{
    		FoundNotes = new List<Note>();
    		return;
    	}

    	Searching = true;
    	var found = await Elasticsearch.GetNotesAsync(SearchString, NotesIndexName, 0, 100);
    	FoundNotes = found.ToList();
    	Searching = false;
    }
}
