using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKemit.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }

        public int ProductBrandId { get; set; } //Fk -- for Representation 
        public ProductBrand ProductBrand { get; set; } //Navigational Property [One]

        public int ProductTypeId { get; set; }    //FK -- for Representation
        public ProductType ProductType { get; set; } //Navigational Property [One]

        public int GovernrateId { get; set; } //FK -- for Representation
        public Governrate Governrate { get; set; } //Navgational property[one]

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public double Rating { get; set; }

        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

        //public bool Available { get; set; } = true; // New property with default value


    }
}
