using JwtTemplate.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtTemplate.Presistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.IsAdmin)
                .HasDefaultValue(0);
        }
    }
}