using DemoMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Student> Student { get; set; }
        public DbSet<Person> Person { get; set; }

        public DbSet<DaiLy> DaiLy { get; set; }
        public DbSet<HeThongPhanPhoi> HeThongPhanPhoi { get; set; }
    }
}