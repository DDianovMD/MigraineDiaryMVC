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

            modelBuilder.Entity<Article>()
                .HasOne(x => x.Creator)
                .WithMany(x => x.Articles)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);

            const string ADMIN_ID = "ff3d52a7-7288-42aa-9955-6c4c4ad4caed";
            const string ROLE_ID = "2d75eec2-411b-43d0-acb7-5ae4bf74555f";

            string path = Path.Combine(AppContext.BaseDirectory, "adminpassword.txt");
            string password = File.ReadAllText(path);

            PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();

            modelBuilder.Entity<ApplicationUser>()
                .HasData(new ApplicationUser
                {
                    Id = ADMIN_ID,
                    UserName = "Admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@migrainediary.com",
                    NormalizedEmail = "ADMIN@MIGRAINEDIARY.COM",
                    EmailConfirmed = true,
                    PasswordHash = passwordHasher.HashPassword(null, password),
                    LockoutEnabled = true,
                    LockoutEnd = null,
                    AccessFailedCount = 0,
                    TwoFactorEnabled = false,
                    IsDeleted = false,
                });

            modelBuilder.Entity<IdentityRole>()
               .HasData(new IdentityRole
               {
                   Id = ROLE_ID,
                   Name = "Admin",
                   NormalizedName = "ADMIN"
               });

            modelBuilder.Entity<IdentityUserRole<string>>()
                .HasData(new IdentityUserRole<string>()
                {
                    RoleId = ROLE_ID,
                    UserId = ADMIN_ID,
                });
        }

        public DbSet<Headache> Headaches { get; set; }

        public DbSet<Medication> Medications { get; set; }

        public DbSet<HIT6Scale> HIT6Scales { get; set; }

        public DbSet<ZungScaleForAnxiety> ZungScalesForAnxiety { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Message> Messages { get; set; }
    }
}