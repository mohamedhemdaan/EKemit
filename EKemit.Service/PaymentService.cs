using EKemit.Core;
using EKemit.Core.Entities;
using EKemit.Core.Entities.Basket;
using EKemit.Core.Entities.Order_Aggregate;
using EKemit.Core.Repository.Contract;
using EKemit.Core.Services.Contract;
using EKemit.Core.Specifications.Order_Specs;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = EKemit.Core.Entities.Product;

namespace EKemit.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration,IBasketRepository basketRepository , IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:Secretkey"];
            //Get Basket 
            var Basket = await _basketRepository.GetBasketAsync(BasketId);
            if (Basket is null) return null;
            var ShippingPrice = 0M;
            if(Basket.DeliveryMethodId.HasValue)
            {
                var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(Basket.DeliveryMethodId.Value);
                Basket.ShippingPrice = DeliveryMethod.Cost;
                ShippingPrice = DeliveryMethod.Cost;
            }
            //Total= Subtotal + DM.Cost
            if(Basket.Items.Count>0)
            {
                foreach(var item in Basket.Items)
                {
                    var Product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    if (item.Price != Product.Price)
                        item.Price = Product.Price;
                }
            }
            var SubTotal = Basket.Items.Sum(item => item.Price * item.Quantity);

            //Create Payment Intent

            var Service = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if(string.IsNullOrEmpty(Basket.PaymentIntentId)) //Create
            {
                var Options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(SubTotal * 100 + ShippingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() {"card"}
                };
                paymentIntent = await Service.CreateAsync(Options); 
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var Options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(SubTotal * 100 + ShippingPrice * 100)
                };
                paymentIntent = await Service.UpdateAsync(Basket.PaymentIntentId, Options);
                //Basket.PaymentIntentId = paymentIntent.Id;
                //Basket.ClientSecret = paymentIntent.ClientSecret; 
            }

            await _basketRepository.UpdateBasketAsync(Basket);

            return Basket;
            
        }

        public async Task<Order> UpdatePaymentIntentToSucceddOrFailed(string PaymentIntentId, bool flag)
        {
            var spec = new OrderWithPaymentIntentSpec(PaymentIntentId);
            var Order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            if (flag)
            {
                Order.Status = OrderStatus.PaymentReceived;
            }
            else
            {
                Order.Status = OrderStatus.PaymentFailed;
            }

            _unitOfWork.Repository<Order>().Update(Order);
            await _unitOfWork.CompleteAsync();
            return Order;  
        }
    }
}
