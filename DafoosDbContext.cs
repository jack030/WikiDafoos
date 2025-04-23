
using Microsoft.EntityFrameworkCore;
using WikiDafoos.Models;
namespace WikiDafoos
{
    public class DafoosDbContext : DbContext
    {
        public DafoosDbContext(DbContextOptions<DafoosDbContext> options) : base(options)
        {

        }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }    
    }
}
