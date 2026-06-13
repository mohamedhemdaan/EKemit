using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EKemit.Core.Entities;

namespace EKemit.Core.Specifications
{
    public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; } = null;
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; } = null;
        public Expression<Func<T, object>> OrderByDescending { get; set; } = null;
        public int Skip { get; set; } 
        public int Take { get; set; } 
        public bool IsPaginationEnabled { get; set; }

        //Get All
        public BaseSpecifications()
        {
            //Cruteria = null;  
        }

        //Get by Id , Get By BrandId , Get By TypeId ,Get By Govenrate
        public BaseSpecifications(Expression<Func<T, bool>> criteriaExpression) 
        {
            Criteria = criteriaExpression;   //true&&true

        }

        public void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression; //P => P.Name
        }
        public void AddOrderByDesc(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }

        public void ApplyPagination( int skip , int take)
        {
            IsPaginationEnabled = true;
            Skip = skip;
            Take = take;
        }
    }
}
