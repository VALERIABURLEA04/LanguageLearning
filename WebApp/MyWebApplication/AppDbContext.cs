using Microsoft.EntityFrameworkCore;
using MyWebApplication.Models;
using System.Collections.Generic;

namespace MyWebApplication
{
  

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<car> Cars { get; set; }
    }
}
