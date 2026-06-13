using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EKemit.Core.Entities;

namespace EKemit.Core.Specifications.Product_Specs
{
    public class ProductWithBrandAndTypeAndGovernrateSpecifications : BaseSpecifications<Product>
    {   
        //This Constrcutor will be used for Creating an object That will be used To Get All Products
        public ProductWithBrandAndTypeAndGovernrateSpecifications(ProductSpecParams Params)
            :base(P =>
            (string.IsNullOrEmpty(Params.Search) || P.Name.ToLower().Contains(Params.Search) )
            &&
            // !(true)=> false || ProductBrandId == 1 true
            (!Params.BrandId.HasValue || P.ProductBrandId == Params.BrandId) //True
            &&
            //!(true)=> fales  || P.ProductTypeId == 1  true
            (!Params.TypeId.HasValue  || P.ProductTypeId == Params.TypeId) //True 
            &&
            (!Params.GovernrateId.HasValue || P.GovernrateId ==Params.GovernrateId)
            )
            
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);
            Includes.Add(P => P.Governrate);

            if (!string.IsNullOrEmpty(Params.Sort))
            {
                switch(Params.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name); 
                        break;

                }
            }
            else
                AddOrderBy(P => P.Name);
            //Products = 18
            //PageSize = 10
            //PageIndex = 1  

            //Skip=>40   = 10 * 4
            //Take=>10
                            // ( 10 * (1-1) =>   0  ,  10)     
            ApplyPagination(Params.PageSize * (Params.Pageindex - 1) , Params.PageSize);

        }
        //This Constrcutor will be used for Creating an object That will be used To Get a Specific Product with Id
        public ProductWithBrandAndTypeAndGovernrateSpecifications(int id):base(P => P.Id == id) //P => P.Id == 1
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);
            Includes.Add(P => P.Governrate);
            Includes.Add(P => P.Comments);
        }

    }
}
