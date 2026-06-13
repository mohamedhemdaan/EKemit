using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKemit.Core.Entities
{
    public class Governrate : BaseEntity
    {
        public string Name { get; set; }
        public string? PictureUrl { get; set; }
    }
}
