using Microsoft.EntityFrameworkCore;
using tcc_web_api.Models;

namespace tcc_web_api.Data {
    public class TCCDbContext : DbContext{

        public TCCDbContext(DbContextOptions<TCCDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Manager> Managers { get; set; }
    }
}
