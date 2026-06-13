using EKemit.Core.Entities;

namespace EKemit.APIs.DTOs.Dashbord_DTOs
{
    public class ProductDashDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string? PictureUrl { get; set; }
        public IFormFile? Image { get; set; }

        public int ProductBrandId { get; set; }
        //public ProductBrand ProductBrand { get; set; } 

        public int ProductTypeId { get; set; }
        //public ProductType ProductType { get; set; } 

        public int GovernrateId { get; set; } //FK -- for Representation
        //public Governrate Governrate { get; set; } 
    }
}
