using Diagramer.Models;
using Diagramer.Models.Hub;
using Diagramer.Models.Identity;
using Diagramer.Models.mxGraph;
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

    public DbSet<MxGraphModel> MxGraphModels { get; set; }
    public DbSet<MxCell> MxCells { get; set; }
    public DbSet<MxGeometry> MxGeometries { get; set; }
    public DbSet<MxArray> MxArrays { get; set; }
    public DbSet<MxPoint> MxPoints { get; set; }

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
        modelBuilder.Entity<MxCell>()
            .HasOne<MxGeometry>(c => c.MxGeometry)
            .WithOne(g => g.MxCell)
            .HasForeignKey<MxGeometry>(g => g.MxCellId)
            .IsRequired(false);
        modelBuilder.Entity<MxGeometry>()
            .HasOne<MxCell>(g => g.MxCell)
            .WithOne(c => c.MxGeometry)
            .HasForeignKey<MxCell>(c => c.MxGeometryId)
            .IsRequired(false);
        
        modelBuilder.Entity<Room>()
            .HasOne<MxGraphModel>(r => r.MxGraphModel)
            .WithOne(g => g.Room)
            .HasForeignKey<MxGraphModel>(g => g.RoomId)
            .IsRequired(false);
        modelBuilder.Entity<MxGraphModel>()
            .HasOne<Room>(g => g.Room)
            .WithOne(r => r.MxGraphModel)
            .HasForeignKey<Room>(r => r.MxGraphModelId)
            .IsRequired(false);
        
        modelBuilder.Entity<MxGraphModel>()
            .HasMany(g => g.Cells)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<MxCell>()
            .HasOne(c => c.MxGraphModel)
            .WithMany(m => m.Cells)
            .HasForeignKey(c => c.MxGraphModelId);

        modelBuilder.Entity<MxGraphModel>()
            .HasMany(m => m.Cells)
            .WithOne(c => c.MxGraphModel)
            .HasForeignKey(c => c.MxGraphModelId);
    }
}