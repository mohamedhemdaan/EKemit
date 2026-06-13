using AutoMapper;
using EKemit.APIs.DTOs.Dashbord_DTOs;
using EKemit.APIs.Errors;
using EKemit.Core.Entities;
using EKemit.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EKemit.APIs.Controllers.Dashboard
{
    
    public class TypeDashboardController : APIBaseController
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TypeDashboardController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //Get Type

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductType>> GetTypeById(int id)
        {
            var Type = await _unitOfWork.Repository<ProductType>().GetByIdAsync(id);
            return Ok(Type);
        }

        [HttpPost]
        public async Task<ActionResult<ProductType>> CreateType([FromForm] BrandOrTypeDto brandOrTypeDto)
        {
            var Type = _mapper.Map<BrandOrTypeDto, ProductType>(brandOrTypeDto);
            _unitOfWork.Repository<ProductType>().Add(Type);
            await _unitOfWork.CompleteAsync();
            return Ok(Type);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductType>> UpdateType([FromRoute] int id, [FromForm] BrandOrTypeDto brandOrTypeDto)
        {
            if (id != brandOrTypeDto.Id)
                return BadRequest(new ApiResponse(404));
            try
            {
                var Type = _mapper.Map<BrandOrTypeDto, ProductType>(brandOrTypeDto);
                _unitOfWork.Repository<ProductType>().Update(Type);
                await _unitOfWork.CompleteAsync();

                return Ok(Type);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(500, ex.Message));

            }

        }


        [HttpPost("{id}")]
        public async Task<ActionResult<int>> DeleteType([FromRoute] int id, [FromForm] BrandOrTypeDto brandOrTypeDto)
        {
            if (id != brandOrTypeDto.Id)
                return BadRequest(new ApiResponse(404));

            try
            {
                var Type = await _unitOfWork.Repository<ProductType>().GetByIdAsync(id);
                _unitOfWork.Repository<ProductType>().Delete(Type);
                return await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));

            }


        }

    }
}
