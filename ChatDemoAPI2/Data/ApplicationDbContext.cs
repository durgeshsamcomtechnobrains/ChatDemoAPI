using ChatDemoAPI2.Model;
using Microsoft.EntityFrameworkCore;

namespace ChatDemoAPI2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<RegisterModel> registerUsers { get; set; }

    }
}
