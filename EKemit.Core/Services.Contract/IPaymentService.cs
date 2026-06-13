using EKemit.Core.Entities.Basket;
using EKemit.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKemit.Core.Services.Contract
{
    public interface IPaymentService
    {
        //Fun To Create Or Update PaymentIntent

        Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId);
        Task<Order> UpdatePaymentIntentToSucceddOrFailed(string PaymentIntentId, bool flag);
    }
}
