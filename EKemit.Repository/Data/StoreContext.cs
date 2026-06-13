using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EKemit.Core.Entities;
using EKemit.Repository.Data.Configurations;
using EKemit.Core.Entities.Order_Aggregate;

namespace EKemit.Repository.Data
{
    public class StoreContext : DbContext
    {

        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new ProductConfigurations());
            //modelBuilder.ApplyConfiguration(new ProductBrandConfigurations());
            //modelBuilder.ApplyConfiguration(new ProductTypeConfigurations());

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<Governrate> Governrates { get; set; }
         
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        public DbSet<ContactUs> ContactUsMessages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        public void UpdateProductRatings()
        {
            var products = Products.Include(p => p.Ratings).ToList();

            foreach (var product in products)
            {
                if (product.Ratings.Any())
                {
                    var averageRating = product.Ratings.Average(r => r.Value);
                    product.Rating = Math.Round(averageRating, 2);
                }
                else
                {
                    product.Rating = 0;
                }
            }

            SaveChanges();
        }




    }
}
