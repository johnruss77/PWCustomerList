using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinewood.Customer.Business
{
    public class Customer
    {
        public int ID { get; set; }
        // Customer full name
        public string Name { get; set; }
        
        public string Email { get; set; }
        public string Phone { get; set; }
        public string LocationString { get; set; }

    }
}
