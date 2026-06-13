using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EKemit.Core.Entities;
using EKemit.Core.Repository;
using EKemit.Core.Specifications;
using EKemit.Repository.Data;
using EKemit.Core.Repository.Contract;

namespace EKemit.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region without Spec
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            //if(typeof(T) == typeof(Product))
            //{
            //    return (IReadOnlyList<T>)await _dbContext.Products.Include(P => P.ProductBrand).Include(P => P.ProductType).ToListAsync();

            //}
            return await _dbContext.Set<T>().ToListAsync();
        }
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
            //return await _dbContext.Products.Where(P => P.Id == id).Include(P => P.ProductBrand).Include(P => P.ProductType).FirstOrDefaultAsync();
        }
        #endregion

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApplySpecifications(Spec).ToListAsync();
            ///_dbContext.Products.Where(true && true).OrderBy(P=>P.Name).Skip(0).Take(10).include(P => P.ProductBrand)
            ///                                                                           .include(P => P.ProductType)
            ///                                                                           .ToListAsync();
        }


        public async Task<T?> GetEntityWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApplySpecifications(Spec).FirstOrDefaultAsync();
            //_dbContext.Set<Product>().Where(P => P.Id == 1).include(P => P.ProductBrand)
            //                                               .include(P => P.ProductType).FirstOrDefaultAsync();

        }
        private IQueryable<T> ApplySpecifications(ISpecifications<T> Spec)
        {
            return SpecificationEvalutor<T>.GetQuery(_dbContext.Set<T>(), Spec);
        }

        public async Task<int> GetCountWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApplySpecifications(Spec).CountAsync();
        }

        ///Loaclly
        public void Add(T entity)  //marked ModelStat (Added) 
            => _dbContext.Set<T>().Add(entity);

        public void Update(T entity) //marked ModelStat (Modified) 
            => _dbContext.Set<T>().Update(entity);

        public void Delete(T entity) //marked ModelStat (Deleted) 
            => _dbContext.Set<T>().Remove(entity);
    }

}
