using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MigraineDiary.Web.Data.DbModels;

namespace MigraineDiary.Web.Data
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

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Headache> Headaches { get; set; }

        public DbSet<Medication> Medications { get; set; }

        public DbSet<HIT6Scale> HIT6Scales { get; set; }

        public DbSet<ZungScaleForAnxiety> ZungScalesForAnxiety { get; set;}
    }
}