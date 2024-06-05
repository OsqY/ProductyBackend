using Microsoft.EntityFrameworkCore;

namespace Producty.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<AppUser>()
                .HasMany(e => e.Todos)
                .WithOne(u => u.User)
                .HasForeignKey("UserId");

            modelBuilder.Entity<AppUser>().Property(u => u.Id).ValueGeneratedOnAdd();
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Todo> Todos { get; set; }
    }
}
