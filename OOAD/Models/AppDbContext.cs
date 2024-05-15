using Microsoft.EntityFrameworkCore;

namespace OOAD.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<CalenderApointment> Calenders { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
