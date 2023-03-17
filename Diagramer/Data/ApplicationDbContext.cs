using Diagramer.Models;
using Diagramer.Models.Hub;
using Diagramer.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Diagramer.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid, IdentityUserClaim<Guid>
    , IdentityUserRole<Guid>,
    IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Models.Task> Tasks { get; set; }
    public DbSet<Diagram> Diagrams { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Room> Rooms { get; set; }

    public DbSet<HubConnection> Connections { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Answer>()
            .HasOne(p => p.User)
            .WithMany(b => b.Answers)
            .HasForeignKey(p => p.UserId);


        modelBuilder.Entity<Answer>()
            .HasOne(p => p.Task)
            .WithMany(b => b.Answers)
            .HasForeignKey(p => p.TaskId);
    }
}