using AutoMapper;
using EKemit.APIs.DTOs;
using EKemit.APIs.DTOs.Dashbord_DTOs;
using EKemit.APIs.Errors;
using EKemit.APIs.Helpers;
using EKemit.Core.Entities;
using EKemit.Core.Repository.Contract;
using EKemit.Core.Services.Contract;
using EKemit.Core.Specifications.Product_Specs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EKemit.APIs.Controllers
{

    public class ProductsController : APIBaseController
    {
        
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }


        //Get All Products
        [CachedAttribute(240)]
        //[Authorize/*(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)*/]
        [HttpGet]   //BaseUrl /api/Products
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams Params)
        {

            var Products = await _productService.GetProductsAsync(Params);


            var Count = await _productService.GetCountAsync(Params);

            var MappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(Products);

            ///var RetunedObject = new Pagination<ProductToReturnDto>()
            ///{
            ///    PageSize = Params.PageSize,
            ///    PageIndex = Params.Pageindex,
            ///    Data = MappedProducts
            ///};

            return Ok(new Pagination<ProductToReturnDto>(Params.PageSize, Params.Pageindex, MappedProducts, Count));

        }

        //Get Product by Id
        [CachedAttribute(240)]
        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(ProductToReturnDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int Id)
        {
            var Product = await _productService.GetProductAsync(Id);
            if (Product is null)
                return NotFound(new ApiResponse(404));

            var MappedProduct = _mapper.Map<Product, ProductToReturnDto>(Product);
            return Ok(MappedProduct);
        }

        //Get All Types
        //BaseUrl/api/Products/Types
        [CachedAttribute(240)]
        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var Types = await _productService.GetTypesAsync();
            return Ok(Types);
        }

        //Get All Brands
        //BaseUrl/api/Products/Brands
        [CachedAttribute(240)]
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var Brands = await _productService.GetBrandsAsync();
            return Ok(Brands);
        }

        //Get All Governtrates
        //BaseUrl/api/Products/Governrates
        [CachedAttribute(240)]
        [HttpGet("Governrates")]
        public async Task<ActionResult<IReadOnlyList<Governrate>>> GetGovernrates()
        {
            var Governrates = await _productService.GetGovernratesAsync();
            var MappedGovernrates = _mapper.Map<IReadOnlyList<Governrate>, IReadOnlyList<GovernrateDashToReturnDto>>(Governrates);
            return Ok(MappedGovernrates);
        }
    }
}
