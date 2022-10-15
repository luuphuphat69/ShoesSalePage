using ShoesSalePage.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ShoesSalePage.Data
{
    public class ShoesDbContext: DbContext
    {
        public ShoesDbContext(): base("ShoesDbContext") // Specify the connection string
        {
        }
        public DbSet<ShoesModel> Shoes { get; set; }    // Create database
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}