using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pinewood.Customers.API.Models
{
    public class Customer
    {
        public int ID { get; set; }
        // Customer full name
        public string Name { get; set; }
        
        public string Email { get; set; }
        public string Phone { get; set; }
        public Location Location { get; set; }
        public int LocationID { get; set; }

    }
}
