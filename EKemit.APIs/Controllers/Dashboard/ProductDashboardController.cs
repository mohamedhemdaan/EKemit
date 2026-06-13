using AutoMapper;
using EKemit.APIs.DTOs;
using EKemit.APIs.DTOs.Dashbord_DTOs;
using EKemit.APIs.Errors;
using EKemit.APIs.Helpers;
using EKemit.Core;
using EKemit.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EKemit.APIs.Controllers.Dashboard
{

    public class ProductDashboardController : APIBaseController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductDashboardController(IMapper mapper,IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        //Get Products  Get : {{BaseUrl}}/api/Products

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromForm]ProductDashDto productDashDto)
        {
            if (productDashDto.Image is not null)
                productDashDto.PictureUrl = PictureSettings.UploadFile(productDashDto.Image, "products");
            else
                productDashDto.PictureUrl = "images/products/hat-react2.png";

            var product = _mapper.Map<ProductDashDto, Product>(productDashDto);

            _unitOfWork.Repository<Product>().Add(product);
            await _unitOfWork.CompleteAsync();

            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> UpdateProduct([FromRoute]int id,[FromForm] ProductDashDto productDashDto)
        {
            if (id != productDashDto.Id)
                return BadRequest(new ApiResponse(404));

            if (productDashDto.Image is not null)
            {
                if (productDashDto.PictureUrl is not null)
                {
                    PictureSettings.DeleteFile(productDashDto.PictureUrl);
                    productDashDto.PictureUrl = PictureSettings.UploadFile(productDashDto.Image, "products");
                }
                else
                {
                    productDashDto.PictureUrl = PictureSettings.UploadFile(productDashDto.Image, "products");

                }
            }

            try
            {
                var product = _mapper.Map<ProductDashDto, Product>(productDashDto);
                _unitOfWork.Repository<Product>().Update(product);
                await _unitOfWork.CompleteAsync();
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(500,ex.Message));
            }

        }


        [HttpPost("{id}")]
        public async Task<ActionResult<int>> DeleteProduct([FromRoute]int id,[FromForm] ProductDashDto productDashDto)
        {
            if (id != productDashDto.Id)
                return BadRequest(new ApiResponse(404));

            try
            {

                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
                if (product.PictureUrl is not null)
                    PictureSettings.DeleteFile(product.PictureUrl);

                _unitOfWork.Repository<Product>().Delete(product);

                return await _unitOfWork.CompleteAsync();
            }
            catch(Exception ex)
            {
                return BadRequest(new ApiResponse(400,ex.Message));
            }



        }

    }
}
