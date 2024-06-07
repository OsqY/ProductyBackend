using Microsoft.EntityFrameworkCore;

namespace Producty.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>().Property(u => u.Id).ValueGeneratedOnAdd();

            modelBuilder
                .Entity<AppUser>()
                .HasMany(u => u.Expenses)
                .WithOne(e => e.User)
                .HasForeignKey("UserId");

            modelBuilder
                .Entity<AppUser>()
                .HasMany(u => u.Incomes)
                .WithOne(i => i.User)
                .HasForeignKey("UserId");

            modelBuilder
                .Entity<AppUser>()
                .HasMany(u => u.JournalEntries)
                .WithOne(j => j.User)
                .HasForeignKey("UserId");

            modelBuilder
                .Entity<AppUser>()
                .HasMany(u => u.Todos)
                .WithOne(u => u.User)
                .HasForeignKey("UserId");
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<JournalEntry> JournalEntries { get; set; }
        public DbSet<Todo> Todos { get; set; }
    }
}
