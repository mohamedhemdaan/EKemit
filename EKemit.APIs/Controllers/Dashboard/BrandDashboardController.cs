using AutoMapper;
using EKemit.APIs.DTOs.Dashbord_DTOs;
using EKemit.APIs.Errors;
using EKemit.APIs.Helpers;
using EKemit.Core;
using EKemit.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EKemit.APIs.Controllers.Dashboard
{
   
    public class BrandDashboardController : APIBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BrandDashboardController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //Get Brand

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductBrand>> GetBrandById(int id)
        {
            var brand = await _unitOfWork.Repository<ProductBrand>().GetByIdAsync(id);
            return Ok(brand);
        }

        [HttpPost]
        public async Task<ActionResult<ProductBrand>> CreateBrand([FromForm] BrandOrTypeDto brandOrTypeDto)
        {
            var brand = _mapper.Map<BrandOrTypeDto, ProductBrand>(brandOrTypeDto);
            _unitOfWork.Repository<ProductBrand>().Add(brand);
            await _unitOfWork.CompleteAsync();
            return Ok(brand);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductBrand>> UpdateBrand([FromRoute]int id, [FromForm] BrandOrTypeDto brandOrTypeDto)
        {
            if (id != brandOrTypeDto.Id)
                return BadRequest(new ApiResponse(404));
            try
            {
                var brand = _mapper.Map<BrandOrTypeDto, ProductBrand>(brandOrTypeDto);
                _unitOfWork.Repository<ProductBrand>().Update(brand);
                await _unitOfWork.CompleteAsync();

                return Ok(brand);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(500, ex.Message));

            }

        }
        [HttpPost("{id}")]
        public async Task<ActionResult<int>> DeleteBrand([FromRoute] int id, [FromForm] BrandOrTypeDto brandOrTypeDto)
        {
            if (id != brandOrTypeDto.Id)
                return BadRequest(new ApiResponse(404));

            try
            {
                var brand = await _unitOfWork.Repository<ProductBrand>().GetByIdAsync(id);
                _unitOfWork.Repository<ProductBrand>().Delete(brand);
                return await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));

            }


        }


    }
}
