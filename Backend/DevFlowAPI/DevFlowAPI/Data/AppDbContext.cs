using DevFlowAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DevFlowAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyMembership> CompanyMemberships { get; set; }
        public DbSet<Technology> Technologies { get; set; }
        public DbSet<UserTechnologyExperience> UserTechnologyExperiences { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<TaskAssignment> TaskAssignments { get; set; }
        public DbSet<ProjectMembership> ProjectMemberships { get; set; }
        public DbSet<Bug> Bugs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            base.OnModelCreating(modelBuilder);

            // Contstraints

            modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

            modelBuilder.Entity<Technology>()
            .HasIndex(t => t.Name)
            .IsUnique();

            modelBuilder.Entity<UserTechnologyExperience>()
            .ToTable(t => t.HasCheckConstraint(
            "CK_UserTechnologyExperience_ExperienceMonths",
            "[ExperienceMonths] >= 0"));

            modelBuilder.Entity<CompanyMembership>()
            .ToTable(t => t.HasCheckConstraint(
            "CK_CompanyMembership_DateRange",
            "[EndedAt] IS NULL OR [StartedAt] <= [EndedAt]"));

            modelBuilder.Entity<ProjectMembership>()
            .ToTable(t => t.HasCheckConstraint(
            "CK_ProjectMembership_DateRange",
            "[EndedAt] IS NULL OR [StartedAt] <= [EndedAt]"));


            // User -- Company --> N:M relationship -> CompanyMembership join entity (the join entity has attributes too, so two 1:N relationships are defined)

            modelBuilder.Entity<CompanyMembership>()
            .HasOne(cm => cm.User)
            .WithMany(u => u.CompanyMemberships)
            .HasForeignKey(cm => cm.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CompanyMembership>()
            .HasOne(cm => cm.Company)
            .WithMany(c => c.Memberships)
            .HasForeignKey(cm => cm.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CompanyMembership>()
            .HasIndex(cm => new { cm.UserId, cm.CompanyId })
            .IsUnique()
            .HasFilter("[EndedAt] IS NULL");

           

            // User -- Project --> N:M relationship --> PojectMember join entity (the join entity has attributes)

            modelBuilder.Entity<ProjectMembership>()
            .HasOne(pm => pm.Project)
            .WithMany(p => p.Memberships)
            .HasForeignKey(pm => pm.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectMembership>()
            .HasOne(pm => pm.User)
            .WithMany(u => u.ProjectMemberShips)
            .HasForeignKey(pm => pm.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectMembership>()
            .HasIndex(pm => new { pm.UserId, pm.ProjectId })
            .IsUnique()
            .HasFilter("[EndedAt] IS NULL");

            // User -- Technology --> N:M relationship --> DeveloperTechnologyExperience join entity (the join entity has attributes)

            modelBuilder.Entity<UserTechnologyExperience>()
            .HasOne(dt => dt.User)
            .WithMany(u => u.TechnologyExperiences)
            .HasForeignKey(dt => dt.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserTechnologyExperience>()
            .HasOne(dt => dt.Technology)
            .WithMany(t => t.UserTechnologyExperiences)
            .HasForeignKey(dt => dt.TechnologyId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserTechnologyExperience>()
            .HasIndex(ute => new { ute.UserId, ute.TechnologyId })
            .IsUnique();

            // User -- ProjectTask --> N:M relationship --> TaskAssignment join entity (the join entity has attributes)

            modelBuilder.Entity<TaskAssignment>()
            .HasOne(ta => ta.User)
            .WithMany(u => u.TaskAssignments)
            .HasForeignKey(ta => ta.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskAssignment>()
            .HasOne(ta => ta.ProjectTask)
            .WithMany(t => t.Assignments)
            .HasForeignKey(ta => ta.ProjectTaskId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskAssignment>()
            .HasIndex(ta => new { ta.UserId, ta.ProjectTaskId })
            .IsUnique();

            // User -- Bug --> 1:N relationship

            modelBuilder.Entity<Bug>()
            .HasOne(b => b.Reporter)
            .WithMany(u => u.ReportedBugs)
            .HasForeignKey(b => b.ReporterUserId)
            .OnDelete(DeleteBehavior.Restrict);

            // User -- Bug --> 1:N relationsip

            modelBuilder.Entity<Bug>()
            .HasOne(b => b.AssignedUser)
            .WithMany(u => u.AssignedBugs)
            .HasForeignKey(b => b.AssignedUserId)
            .OnDelete(DeleteBehavior.Restrict); 

            // Technology -- Project --> N:M relationship (the join entity doesn't have additional attributes)

            modelBuilder.Entity<Project>()
            .HasMany(p => p.Technologies)
            .WithMany(t => t.Projects)
            .UsingEntity(j => j.ToTable("ProjectTechnologies"));

            // Project -- ProjectTask --> 1:N relationship

            modelBuilder.Entity<ProjectTask>()
            .HasOne(t => t.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

            // ProjectTask -- Bug --> 1:N relationship, but partial from the N side

            modelBuilder.Entity<Bug>()
           .HasOne(b => b.ProjectTask)
           .WithMany(t => t.Bugs)
           .HasForeignKey(b => b.ProjectTaskId)
           .OnDelete(DeleteBehavior.Restrict);
            
            // Company -- Projects --> 1:N relationship

            modelBuilder.Entity<Project>()
            .HasOne(p => p.Company)
            .WithMany(c => c.Projects)
            .HasForeignKey(p => p.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);













        }
    }
}
