using Microsoft.EntityFrameworkCore;
using tweetIngestion_service.Models;

namespace tweetIngestion_service.Data;

public class ApplicationDbContext:DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        
    }

  

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tweets>().HasKey(nameof(Models.Tweets.Id));
    }
    public DbSet<Tweets> Tweets { get; set; }
}