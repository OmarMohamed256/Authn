using Microsoft.EntityFrameworkCore;

namespace Authn.Data
{
    public class AuthDbContext : DbContext
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) 
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>( entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserId);
                entity.Property(e => e.Provider).HasMaxLength(250);
                entity.Property(e => e.NameIdentifier).HasMaxLength(250);
                entity.Property(e => e.Username).HasMaxLength(250);
                entity.Property(e => e.Password).HasMaxLength(250);
                entity.Property(e => e.Email).HasMaxLength(250);
                entity.Property(e => e.Firstname).HasMaxLength(250);
                entity.Property(e => e.Lastname).HasMaxLength(250);
                entity.Property(e => e.Mobile).HasMaxLength(250);
                entity.Property(e => e.Roles).HasMaxLength(250);

                // seed data
                entity.HasData(new AppUser
                {
                    Provider ="Cookies",
                    UserId = 1,
                    Email = "omar@gmail.com",
                    Username = "omar@gmail.com",
                    Password = "pizza",
                    Firstname = "omar",
                    Lastname = "mohamed",
                    Mobile = "0102598746",
                    Roles = "Admin"
                });
            });
        }

    }
}
