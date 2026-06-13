using EKemit.Core.Entities;
using EKemit.Core.Specifications.Product_Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKemit.Core.Services.Contract
{
    public interface IProductService
    {
        Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams Params);
        Task<Product?> GetProductAsync(int id);
        Task<IReadOnlyList<ProductType>> GetTypesAsync();

        Task<IReadOnlyList<ProductBrand>> GetBrandsAsync();

        Task<IReadOnlyList<Governrate>> GetGovernratesAsync();

        Task<int> GetCountAsync(ProductSpecParams Params);
    }
}
