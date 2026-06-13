using AutoMapper;
using EKemit.APIs.DTOs;
using EKemit.APIs.Errors;
using EKemit.Core.Entities.Basket;
using EKemit.Core.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace EKemit.APIs.Controllers
{


    public class PaymentsController : APIBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        const string endpointSecret = "whsec_b661c056c1ae9c51e46259aeb97b2a92d3706e662a922cb546016649405c7864";

        public PaymentsController(IPaymentService paymentService, IMapper mapper)
        {
            _paymentService = paymentService;
            _mapper = mapper;
        }

        //Create Or update  PaymentIntentID
        [Authorize]
        [ProducesResponseType(typeof(CustomerBasketDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var customerBasket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (customerBasket is null) return BadRequest(new ApiResponse(400, "There is a Problem with your Basket"));
            var MappedBasket = _mapper.Map<CustomerBasket, CustomerBasketDto>(customerBasket);
            return Ok(MappedBasket);
        }


        [HttpPost("webhook")] //BaseUrl/api/Payments/webhook
        public async Task<IActionResult> StripteWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], endpointSecret);

            var paymentItent = stripeEvent.Data.Object as PaymentIntent;
            // Handle the event
            if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
            {
                await _paymentService.UpdatePaymentIntentToSucceddOrFailed(paymentItent.Id, false);
            }
            else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
            {
                await _paymentService.UpdatePaymentIntentToSucceddOrFailed(paymentItent.Id, true);

            }

            return Ok();
        }





    }
}
