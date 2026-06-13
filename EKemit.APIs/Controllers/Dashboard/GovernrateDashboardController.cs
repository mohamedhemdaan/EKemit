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
   
    public class GovernrateDashboardController : APIBaseController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GovernrateDashboardController(IMapper mapper,IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Governrate>> GetGovernrateById(int id)
        {
            var governrate = await _unitOfWork.Repository<Governrate>().GetByIdAsync(id);
            if (governrate is null) return NotFound(new ApiResponse(404));
            var MappedGovernrate = _mapper.Map<Governrate, GovernrateDashToReturnDto>(governrate);
            return Ok(MappedGovernrate);
        }


        [HttpPost]
        public async Task<ActionResult<Governrate>> CreateGovernrate([FromForm] GovernrateDashToReturnDto governrateDashToReturnDto)
        {
            if (governrateDashToReturnDto.Image is not null)
                governrateDashToReturnDto.PictureUrl = PictureSettings.UploadFile(governrateDashToReturnDto.Image, "governrates");
            else
                governrateDashToReturnDto.PictureUrl = "images/governrates/hat-react2.png";

            var governrate = _mapper.Map<GovernrateDashToReturnDto, Governrate>(governrateDashToReturnDto);

            _unitOfWork.Repository<Governrate>().Add(governrate);
            await _unitOfWork.CompleteAsync();

            return Ok(governrate);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Governrate>> UpdateGovernrate([FromRoute] int id, [FromForm] GovernrateDashToReturnDto governrateDashToReturnDto)
        {
            if (id != governrateDashToReturnDto.Id)
                return BadRequest(new ApiResponse(404));

            if (governrateDashToReturnDto.Image is not null)
            {
                if (governrateDashToReturnDto.PictureUrl is not null)
                {
                    PictureSettings.DeleteFile(governrateDashToReturnDto.PictureUrl);
                    governrateDashToReturnDto.PictureUrl = PictureSettings.UploadFile(governrateDashToReturnDto.Image, "governrates");
                }
                else
                {
                    governrateDashToReturnDto.PictureUrl = PictureSettings.UploadFile(governrateDashToReturnDto.Image, "governrates");

                }
            }

            try
            {
                var governrate = _mapper.Map<GovernrateDashToReturnDto, Governrate>(governrateDashToReturnDto);
                _unitOfWork.Repository<Governrate>().Update(governrate);
                await _unitOfWork.CompleteAsync();
                return Ok(governrate);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(500, ex.Message));
            }

        }


        [HttpPost("{id}")]
        public async Task<ActionResult<int>> DeleteGovernrate([FromRoute] int id, [FromForm] GovernrateDashToReturnDto governrateDashToReturnDto)
        {
            if (id != governrateDashToReturnDto.Id)
                return BadRequest(new ApiResponse(404));

            try
            {

                var governrate = await _unitOfWork.Repository<Governrate>().GetByIdAsync(id);
                if (governrate.PictureUrl is not null)
                    PictureSettings.DeleteFile(governrate.PictureUrl);

                _unitOfWork.Repository<Governrate>().Delete(governrate);

                return await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }



        }
    }
}
