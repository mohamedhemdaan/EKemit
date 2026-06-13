using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using EKemit.Core.Entities;

namespace EKemit.Core.Specifications.Product_Specs
{
    public class ProductWithSecificationForCountAsync : BaseSpecifications<Product>
    {
        public ProductWithSecificationForCountAsync(ProductSpecParams Params)
            : base(P =>
            (String.IsNullOrEmpty(Params.Search) || P.Name.ToLower().Contains(Params.Search))
            &&
            // !(true)=> false || ProductBrandId == 1 true
            (!Params.BrandId.HasValue || P.ProductBrandId == Params.BrandId) //True
            &&
            //!(true)=> fales  || P.ProductTypeId == 1  true
            (!Params.TypeId.HasValue || P.ProductTypeId == Params.TypeId) //True 
            &&
            (!Params.GovernrateId.HasValue || P.GovernrateId == Params.GovernrateId)

            )

        {

        }
    }
}
