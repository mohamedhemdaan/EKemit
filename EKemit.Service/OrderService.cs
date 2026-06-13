using EKemit.Core;
using EKemit.Core.Entities;
using EKemit.Core.Entities.Order_Aggregate;
using EKemit.Core.Repository.Contract;
using EKemit.Core.Services.Contract;
using EKemit.Core.Specifications;
using EKemit.Core.Specifications.Order_Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKemit.Service
{
    public class OrderService : IOrderService
    {
        //private readonly IGenericRepository<Product> _productRepo;
        //private readonly IGenericRepository<DeliveryMethod> _deliverMethodRepo;
        //private readonly IGenericRepository<Order> _orderRepo;

        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepo,IUnitOfWork unitOfWork,IPaymentService paymentService)
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int DeliveryMethodId, Address shippingAddress)
        {
            //1.Get Basket From Basket Repo
            var Basket = await _basketRepo.GetBasketAsync(basketId);
            //2.Get Selected Items at Basket From Product Repo 
            //create OrderItems 
            var OrderItems = new List<OrderItem>(); 
            if(Basket?.Items.Count > 0)
            {
                foreach(var item in Basket.Items)
                {
                    //1s info --> Id of BasketItem
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var prdocutItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);
                    var OrderItem = new OrderItem(prdocutItemOrdered,product.Price,item.Quantity);
                    OrderItems.Add(OrderItem);
                }
            }

            //3.Calculate SubTotal Sum of (Price * Quantity) for each Orderitem in OrderItems 
            var SubTotal = OrderItems.Sum(OrdItem => OrdItem.Price * OrdItem.Quantity);

            //4.Get Delivery Method From DeliveryMethod Repo
            var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);


            //5.Create Order

            var Spec = new OrderWithPaymentIntentSpec(Basket.PaymentIntentId);
            var existingOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(Spec);
            if(existingOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            }


            var Order = new Order(buyerEmail, shippingAddress, DeliveryMethod, OrderItems, SubTotal, Basket.PaymentIntentId);





            //6.Add Order Locally
            _unitOfWork.Repository<Order>().Add(Order);

            //7.Save Order To Database[ToDo]
            var Result = await _unitOfWork.CompleteAsync();

            if (Result <= 0) return null;

            return Order;


        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var DeliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return DeliveryMethods;
        }

        public async Task<Order> GetOrderByIdForSpecificUserAsync(string buyerEmail, int orderId)
        {
            var spec = new OrderSpecifications(buyerEmail, orderId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            return order;   
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecifications(buyerEmail);
            var Orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return Orders;
        }

        public async Task<IReadOnlyList<Order>> GetAllOrdersAsync() 
        {
            var spec = new OrderSpecifications();
            return await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
        }
    }
}
