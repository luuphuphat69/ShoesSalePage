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
        public DbSet<ShoesModel> Shoes { get; set; }
        /*public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }*/
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}