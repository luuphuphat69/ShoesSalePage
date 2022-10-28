using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using ShoesSalePage.Models;

namespace ShoesSalePage.Data
{
    public class UsersDbContext:DbContext
    {
        public UsersDbContext(): base("UsersDbContext")
        {
        }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Database.SetInitializer<demoEntities>(null);
            modelBuilder.Entity<User>().ToTable("UsersDbTable");
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}