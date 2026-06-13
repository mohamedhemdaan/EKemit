using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EKemit.Core.Entities;

namespace EKemit.Repository.Data.Configurations
{
    internal class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(P => P.ProductBrand)
                   .WithMany()
                   .HasForeignKey(P => P.ProductBrandId);

            builder.HasOne(P => P.ProductType)
                   .WithMany(pt => pt.Products)
                   .HasForeignKey(P => P.ProductTypeId);

            builder.HasOne(P => P.Governrate)
                   .WithMany()
                   .HasForeignKey(P => P.GovernrateId);

            builder.Property(P => P.Name).IsRequired()
                   .HasMaxLength(100);
            builder.Property(P => P.Description).IsRequired();
            builder.Property(P => P.PictureUrl).IsRequired();
            builder.Property(P => P.Price).HasColumnType("decimal(18,2)");
        }
    }
}
