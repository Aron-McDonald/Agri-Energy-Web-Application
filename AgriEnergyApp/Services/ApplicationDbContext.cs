using AgriEnergyApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace AgriEnergyApp.Services
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; } // Add this DbSet for products


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Add any additional configurations or constraints for your entities

            var employee = new IdentityRole("employee");
            employee.NormalizedName = "employee";

            var farmer = new IdentityRole("farmer");
            farmer.NormalizedName = "farmer";

            builder.Entity<IdentityRole>().HasData(employee, farmer);
        }
    }
}
