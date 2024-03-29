using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using SharedNotes.Components;
using SharedNotes.Data;
using SharedNotes.Interfaces;
using SharedNotes.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<NotesContext>();
builder.Services.AddScoped<INotesRepository, NotesRepository>();

builder.Services.AddSingleton<ElasticsearchClient>(
    new ElasticsearchClient(
        builder.Configuration.GetValue<string>("Elasticsearch:CloudId"), 
        new ApiKey(builder.Configuration.GetValue<string>("Elasticsearch:ApiKey"))
    ));
builder.Services.AddScoped<IElasticsearchService, ElasticsearchService>();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<NotesContext>();
            var elasticsearch = services.GetRequiredService<IElasticsearchService>();
            DbInitializer.Initialize(context, elasticsearch);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding the DB.");
        }
    }
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();