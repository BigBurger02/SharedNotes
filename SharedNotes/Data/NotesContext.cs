using Microsoft.EntityFrameworkCore;
using SharedNotes.Model;

namespace SharedNotes.Data;

public class NotesContext : DbContext
{
    private readonly IConfiguration _config;

    public NotesContext()
    {
    }
    
    public NotesContext(DbContextOptions<NotesContext> options, IConfiguration config) : base(options)
    {
        _config = config;
    }
    
    public virtual DbSet<Note> Notes { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_config.GetConnectionString("PostgresConnectionString"));
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    { 
        modelBuilder.UseIdentityAlwaysColumns();
    }
    
}