using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EKemit.Core.Entities;
using EKemit.Core.Specifications;

namespace EKemit.Core.Repository.Contract
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        #region without Spec
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        #endregion

        #region With Spec
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> Spec);
        Task<T?> GetEntityWithSpecAsync(ISpecifications<T> Spec);

        Task<int> GetCountWithSpecAsync(ISpecifications<T> Spec);
        #endregion

        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);


    }
}
