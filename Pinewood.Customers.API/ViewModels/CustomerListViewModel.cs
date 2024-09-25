using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pinewood.Customers.API.ViewModels.Customer
{
    public class CustomerListViewModel
    {
        public List<CustomerViewModel> Customers { get; set; }

        public CustomerListViewModel()
        {
            this.Customers = new List<CustomerViewModel>();

        }
    }
}
