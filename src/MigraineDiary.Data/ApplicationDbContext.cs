using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MigraineDiary.Data.Common;
using MigraineDiary.Data.DbModels;

namespace MigraineDiary.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Headache>()
                .HasOne(x => x.Patient)
                .WithMany(x => x.Headaches)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HIT6Scale>()
                .HasOne(x => x.Patient)
                .WithMany(x => x.HIT6Scales)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ZungScaleForAnxiety>()
                .HasOne(x => x.Patient)
                .WithMany(x => x.ZungScalesForAnxiety)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Article>()
                .HasOne(x => x.Creator)
                .WithMany(x => x.Articles)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ClinicalTrial>()
                .HasMany(x => x.Practicioners)
                .WithOne(x => x.ClinicalTrial)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClinicalTrial>()
                .HasOne(x => x.Creator)
                .WithMany(x => x.ClinicalTrials)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);

            TestDataSeeder.SeedUsers(modelBuilder);
            TestDataSeeder.SeedRoles(modelBuilder);
            TestDataSeeder.AssignRoles(modelBuilder);
            TestDataSeeder.SeedData(modelBuilder);
        }

        public DbSet<Headache> Headaches { get; set; }

        public DbSet<Medication> Medications { get; set; }

        public DbSet<HIT6Scale> HIT6Scales { get; set; }

        public DbSet<ZungScaleForAnxiety> ZungScalesForAnxiety { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Practicioner> Practicioners { get; set; }

        public DbSet<ClinicalTrial> ClinicalTrials { get; set; }
    }
}