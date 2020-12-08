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
    }
}