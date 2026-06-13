using EKemit.Core.Entities.Order_Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKemit.Repository.Data.Configurations.Order_Config
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(O => O.Status)
                   .HasConversion(OStatus => OStatus.ToString(),
                                  OStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), OStatus));

            builder.Property(O => O.SubTotal)
                   .HasColumnType("decimal(12,2)");

            builder.OwnsOne(O => O.ShippingAddress, Sh => Sh.WithOwner());

            builder.HasOne(O => O.DeliveryMethod)
                   .WithMany()
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(O => O.Items).WithOne().OnDelete(DeleteBehavior.Cascade);

        }
    }
}
