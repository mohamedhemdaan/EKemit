using EKemit.APIs.DTOs;
using EKemit.APIs.Errors;
using EKemit.Core;
using EKemit.Core.Entities;
using EKemit.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EKemit.APIs.Controllers
{
  
    public class ContactUsController : APIBaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContactUsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        [HttpPost]
        public async Task<ActionResult> ContactUs([FromForm] ContactUsDto contactUsDto)
        {
            if (contactUsDto == null || !ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(400, "Invalid contact information."));
            }

            var contactUs = new ContactUs
            {
                Name = contactUsDto.Name,
                Email = contactUsDto.Email,
                PhoneNumber = contactUsDto.PhoneNumber,
                Subject = contactUsDto.Subject,
                Message = contactUsDto.Message
            };


            _unitOfWork.Repository<ContactUs>().Add(contactUs);
            await _unitOfWork.CompleteAsync();

            return Ok(new ApiResponse(200, "Contact message received successfully."));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> GetContactMessages()
        {
            //var contactMessages = await _storeContext.ContactUsMessages.ToListAsync();
            var contactMessages = await _unitOfWork.Repository<ContactUs>().GetAllAsync();
                 
            return Ok(contactMessages);
        }

    }
}

