using LandmarkHunt.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace LandmarkHunt.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public DbSet<Location> Locations { get; set; } = null!;

    public DbSet<UserGuess> UserGuesses { get; set; } = null!;

    public DbSet<Challenge> Challenges { get; set; } = null!;

    public DbSet<ChallengeLocation> ChallengeLocations { get; set; } = null!;
    public DbSet<SessionChallenge> SessionChallenges { get; set; } = null!;
    public DbSet<Photo> Photos { get; set; } = null!;
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder
            .Entity<Location>()
            .HasOne(e => e.CreatorUser)
            .WithMany(e => e.Locations)
            .OnDelete(DeleteBehavior.ClientCascade);
        builder
           .Entity<SessionChallenge>()
           .HasOne(e => e.Player)
           .WithMany(e => e.Sessions)
           .OnDelete(DeleteBehavior.ClientCascade);
    }
}
