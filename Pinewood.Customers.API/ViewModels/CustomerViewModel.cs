using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pinewood.Customers.API.Models;

namespace Pinewood.Customers.API.ViewModels.Customer
{
    public class CustomerViewModel
    {
        public int ID { get; set; }
        // Customer full name
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int LocationID { get; set; }
        public string LocationString { get; set; }

    }
}
