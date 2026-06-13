using EKemit.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKemit.Core.Specifications.Product_Specs
{
    public class ProductTypesWithProductsSpecification
    : BaseSpecifications<ProductType>
    {
        public ProductTypesWithProductsSpecification()
            : base(pt => pt.Products.Any())
        {
        }
    }
}
