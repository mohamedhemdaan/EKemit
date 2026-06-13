using System.ComponentModel.DataAnnotations;

namespace EKemit.APIs.DTOs
{
    public class OrderDto
    {
        [Required]
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; } //int => Requierd
        public AddressDto shipToAddress { get; set; }
    }
}
