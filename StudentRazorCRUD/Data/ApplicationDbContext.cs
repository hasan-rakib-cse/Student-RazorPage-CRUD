using Microsoft.EntityFrameworkCore;
using StudentRazorCRUD.Models;

namespace StudentRazorCRUD.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {            
        }

        public DbSet<Student> Students { get; set; } = default!;
    }
}
