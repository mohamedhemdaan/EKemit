using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKemit.Core.Entities.Identity
{
    public class Emaill
    {
        public int Id { get; set; } // if it will map to table in Databsae
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
