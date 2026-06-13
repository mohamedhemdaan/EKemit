using EKemit.Core.Entities.Order_Aggregate;

namespace EKemit.APIs.DTOs
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; } //PricuteUrlResolve
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}