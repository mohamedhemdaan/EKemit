using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EKemit.Core.Entities;

namespace EKemit.Core.Specifications
{
    public interface ISpecifications<T> where T : BaseEntity
    {
        //_dbContext.Products.Where(P => P.Id == id).Include(P => P.ProductBrand).Include(P => P.ProductType)

        // Signature for Property For Where Conditon Where(P => P.Id == id)
        public Expression<Func<T,bool>> Criteria { get; set; }

        // Signature for Property For list of include [Include(P => P.ProductBrand).Include(P => P.ProductType)]
        public List<Expression<Func<T,object>>> Includes { get; set; }


        // Signeture for Property For Order By  [OrderBy(P=>P.Name)]
        public Expression<Func<T,object>> OrderBy { get; set; }

        // Signeture for Property For OrderDesc By  [OrderByDesc(P=>P.Name)]
        public Expression<Func<T,object>> OrderByDescending { get; set; }

        //Skip 
        public int Skip { get; set; }
        //Take
        public int Take { get; set; }

        public bool IsPaginationEnabled { get; set; }
    }
}
