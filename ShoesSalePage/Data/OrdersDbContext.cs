using ShoesSalePage.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ShoesSalePage.Data
{
    public class OrdersDbContext: DbContext
    {
        public OrdersDbContext() : base("OrderDbContext")
        {
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Database.SetInitializer<demoEntities>(null);
            modelBuilder.Entity<Order>().ToTable("OrdersDbTable");
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}