using SharedNotes.Components;
using SharedNotes.Data;
using SharedNotes.Interfaces;
using SharedNotes.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<NotesContext>();
builder.Services.AddScoped<INotesRepository, NotesRepository>();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();