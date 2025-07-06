using DemoMVC.Controllers;
using DemoMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Data
{
    public class ApplicationDbcontext : DbContext
    {
        public ApplicationDbcontext(DbContextOptions<ApplicationDbcontext> options) : base(options)
        {}
        public DbSet<Person> Persons { get; set;}

        public static implicit operator ApplicationDbcontext(ApplicationDbContext v)
        {
            throw new NotImplementedException();
        }
    }
}


