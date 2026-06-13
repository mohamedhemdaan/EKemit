using EKemit.Core;
using EKemit.Core.Entities;
using EKemit.Core.Services.Contract;
using EKemit.Core.Specifications.Product_Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKemit.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams Params)
        {
            var Spec = new ProductWithBrandAndTypeAndGovernrateSpecifications(Params);
            var Products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(Spec);
            return Products;
        }
        public async Task<Product?> GetProductAsync(int id)
        {
            var Spec = new ProductWithBrandAndTypeAndGovernrateSpecifications(id);
            var Product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(Spec);
            return Product;
        }
        public async Task<int> GetCountAsync(ProductSpecParams Params)
        {
            var CountSpec = new ProductWithSecificationForCountAsync(Params);
            var Count = await _unitOfWork.Repository<Product>().GetCountWithSpecAsync(CountSpec);
            return Count;
        }

        public async Task<IReadOnlyList<ProductType>> GetTypesAsync()
        {
            var spec = new ProductTypesWithProductsSpecification();

            return await _unitOfWork.Repository<ProductType>().GetAllWithSpecAsync(spec);
        }
        public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
        {
            var Brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Brands;
        }

        public async Task<IReadOnlyList<Governrate>> GetGovernratesAsync()
        {
            var Governrates = await _unitOfWork.Repository<Governrate>().GetAllAsync();
            return Governrates;

        }


    }
}
