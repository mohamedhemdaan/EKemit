using AutoMapper;
using EKemit.APIs.DTOs;
using EKemit.APIs.Errors;
using EKemit.Core.Entities.Order_Aggregate;
using EKemit.Core.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EKemit.APIs.Controllers
{
    public class OrdersController : APIBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;


        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        //Create Order 
        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost] //Post : BaseUrl/api/Orders
        [Authorize]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var mappedAddress = _mapper.Map<AddressDto, Address>(orderDto.shipToAddress);
            var Order = await _orderService.CreateOrderAsync(buyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, mappedAddress);
            if (Order is null) return BadRequest(new ApiResponse(400, "There is a Problem With Your Project"));
            return Ok(_mapper.Map<OrderToReturnDto>(Order));
        }


        [ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet] //Get : BaseUrl/Api/Orders
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var BuyerEmaul = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await _orderService.GetOrdersForSpecificUserAsync(BuyerEmaul);
            if (Orders is null) return NotFound(new ApiResponse(404, "There are no Orders For This User"));
            var MappedOrders = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(Orders);
            return Ok(MappedOrders);
        }

        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [Authorize]
        [HttpGet("{id}")] //Get : BaseUrl/api/Orders/1
        public async Task<ActionResult<OrderToReturnDto>> GerOrderForUser(int id)
        {
            var BuyerEmaul = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderByIdForSpecificUserAsync(BuyerEmaul, id);
            if (order is null) return NotFound(new ApiResponse(404, $"There is No Order With Id = {id} For This User"));
            var MappedOrder = _mapper.Map<Order, OrderToReturnDto>(order);
            return Ok(MappedOrder); 
        }

        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var DeliveryMethods = await _orderService.GetDeliveryMethodsAsync();
            return Ok(DeliveryMethods);
        }


        [ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("AllOrders")] // New endpoint to get all orders
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            if (orders is null || orders.Count == 0) return NotFound(new ApiResponse(404, "No orders found"));
            var mappedOrders = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders);
            return Ok(mappedOrders);
        }
    }
}
