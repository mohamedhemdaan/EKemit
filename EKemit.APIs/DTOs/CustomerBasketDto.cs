using EKemit.Core.Entities.Basket;
using System.ComponentModel.DataAnnotations;

namespace EKemit.APIs.DTOs
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; } = null!; //Guid  
        public List<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();
        public int? DeliveryMethodId { get; set; }
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        public decimal ShippingPrice { get; set; }


    }
}
