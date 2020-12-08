using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Data
{
    public class SolarDBContext : IdentityDbContext 
    {
         public SolarDBContext(DbContextOptions options) : base(options)
         {

         }

         public virtual DbSet<Customer> Customers { get; set; }
         public virtual DbSet<CustomerAddress> CustomerAdresses { get; set; }
         public virtual DbSet<Product> Products{ get; set; }
         public virtual DbSet<ProductInvetory> ProductInvetories { get; set; }
         public virtual DbSet<ProductInventorySnapshot> ProductInventorySnapshots { get; set; }
         public virtual DbSet<SalesOrder> SalesOrders{ get; set; }
         public virtual DbSet<SalesOrderItem> SalesOrderItems { get; set; }
    }
}