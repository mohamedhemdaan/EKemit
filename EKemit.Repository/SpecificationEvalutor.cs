using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EKemit.Core.Entities;
using EKemit.Core.Specifications;

namespace EKemit.Repository
{
    public static class SpecificationEvalutor<T> where T : BaseEntity
    {
        ///Func To build Quiery
        ///_dbContext.Set<T>().Where(P => P.Id == id).Include(P => P.ProductBrand).Include(P => P.ProductType)
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery,ISpecifications<T> Spec)
        {
            var Query = inputQuery;     //_dbContext.Products

            if(Spec.Criteria is not null) //Criteria = true && true 
            {
                Query = Query.Where(Spec.Criteria);
                //Query = _dbContext.Products.Where(true && true)
            }


            if (Spec.OrderBy is not null) //P=>P.Name --> "Name" 
            {
                Query = Query.OrderBy(Spec.OrderBy);
                //Query = _dbContext.Products.Where(true && true).OrderBy(P=>P.Name)
            }
            if (Spec.OrderByDescending is not null) //P=>P.Price --> "PriceDesc"
            {
                Query = Query.OrderByDescending(Spec.OrderByDescending);
                //Query = _dbContext.Set<Product>().OrderByDescending(P=>P.Price) in Sort = "PriceDesc"
            }

            if(Spec.IsPaginationEnabled)
            {

                Query = Query.Skip(Spec.Skip).Take(Spec.Take);
                //Query = _dbContext.Products.Where(true && true).OrderBy(P=>P.Name).Skip(0).Take(10)
            }


            //Query = _dbContext.Products.Where(true && true).OrderBy(P=>P.Name).Skip(0).Take(10)

            ///CurrentQuery = Query
            ///Includes
            /// 1. P => P.ProductBrand
            /// 2. P => P.ProductType

            Query = Spec.Includes.Aggregate(Query, (CurrentQuery, IncludeExpression) => CurrentQuery.Include(IncludeExpression));

            ///in a 1s iteration
            ///Query = CurrentQuery  =>
            ///_dbContext.Products.Where(true && true).OrderBy(P=>P.Name).Skip(0).Take(10).include(P => P.ProductBrand)
            ///
            ///_dbContext.Products.Where(true && true).OrderBy(P=>P.Name).Skip(0).Take(10).include(P => P.ProductBrand)
            ///                                                                           .include(P => P.ProductType)

            return Query;
            ///_dbContext.Products.Where(true && true).OrderBy(P=>P.Name).Skip(0).Take(10).include(P => P.ProductBrand)
            ///                                                                           .include(P => P.ProductType)
        }
    }
}
