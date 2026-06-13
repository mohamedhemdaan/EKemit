using EKemit.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKemit.Repository.Data.Configurations
{
    public class GovernrateConfigurations : IEntityTypeConfiguration<Governrate>
    {
        public void Configure(EntityTypeBuilder<Governrate> builder) //builder => Governrate
        {
            builder.Property(G => G.Name).IsRequired();
        }
    }
}
