using AutoMapper;
using EKemit.APIs.DTOs;
using EKemit.APIs.Errors;
using EKemit.Core.Entities.Basket;
using EKemit.Core.Repository.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EKemit.APIs.Controllers
{
    public class BasketController : APIBaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository,IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        //Get Basket
        [HttpGet] //GET :  BaseUrl/api/Basket?id=
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket is null ? new CustomerBasket(id) : basket);  // if null , it will generate new empty basket
        }


        //Create Or Update Basket
        [HttpPost] //POST : BaseUrl/api/Basket
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var mappedBasket = _mapper.Map<CustomerBasketDto,CustomerBasket>(basket);

            var createdOrUpatedBasket = await _basketRepository.UpdateBasketAsync(mappedBasket); 
            if (createdOrUpatedBasket is null) return BadRequest(new ApiResponse(400));
            return Ok(createdOrUpatedBasket); 
        }
        //Delete Basket
        [HttpDelete] //DELETE : BaseUrl/api/Basket?id=
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            return await _basketRepository.DeleteBasketAsync(id);
        }



    }
}
