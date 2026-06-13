using EKemit.Core.Entities;

namespace EKemit.APIs.DTOs
{
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }

        public int ProductBrandId { get; set; } 
        public string ProductBrand { get; set; } //Name of ProductBrand

        public int ProductTypeId { get; set; }    
        public string ProductType { get; set; } //Name of ProductType

        public int GovernrateId { get; set; } //FK -- for Representation
        public string Governrate { get; set; } //Name of Governrate
        public double Rating { get; set; }

        public List<string> comments { get; set; }

    }
}
