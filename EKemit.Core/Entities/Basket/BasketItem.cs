namespace EKemit.Core.Entities.Basket
{
    public class BasketItem
    {
        public int Id { get; set; } //Id of Product
        public string? ProductName { get; set; } 
        public string? Description { get; set; } 
        public decimal Price { get; set; } //Price of item (if there are any discount )
        public int Quantity { get; set; }
        public string? PictureUrl { get; set; } 
        public string? Brand { get; set; } 
        public string? Type { get; set; } 
        //public string Governrate { get; set; } = null!;
    }
}