using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKemit.Core.Entities.Order_Aggregate
{
    public class Order : BaseEntity
    {
        public Order()
        {
            
        }
        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod,
                     ICollection<OrderItem> items, decimal subTotal,string paymentIntentId )
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;  //Date of Creation Order
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Address ShippingAddress { get; set; } //object[Composite Attribute]   Not Navigitional property  

        //public int DeliveryMethodId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; } // Navigational Property [ M Order -- 1 DeliveryMethod ]

        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>(); //Navigational property[M]

        public decimal SubTotal { get; set; } // price of Product * Quantity (1000 * 2)

        //[NotMapped]
        //public decimal Total { get => SubTotal + DeliveryMethod.Cost; }     //SubTotal + Delivery cost
        public decimal GetTotal() => SubTotal + DeliveryMethod.Cost;

        public string PaymentIntentId { get; set; } 
    }
}
