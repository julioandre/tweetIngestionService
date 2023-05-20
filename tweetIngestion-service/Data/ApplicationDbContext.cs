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
        modelBuilder.Entity<TweetEntity>().HasKey(nameof(TweetEntity.Id));
    }
    public DbSet<TweetEntity> Tweets { get; set; }
}