using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pinewood.Customers.API.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Pinewood.Customers.API.Models;


namespace Pinewood.Customers.API.Mappers
{
    public class CustomerMapper
    {
        public static ViewModels.Customer.CustomerViewModel MapFromEntity(API.Models.Customer e, IEnumerable<API.Models.Location> locations)
        {
            ViewModels.Customer.CustomerViewModel model = null;
            if (e != null)
            {
                model = new ViewModels.Customer.CustomerViewModel
                {
                    ID = e.ID,
                    Name = e.Name,
                    
                    Email = e.Email,
                    Phone = e.Phone,
                    LocationString =  locations.Where(l => l.ID == e.LocationID).FirstOrDefault()?.Name,

                };
            }
            return model;
        }

      
        public static IEnumerable<ViewModels.Customer.CustomerViewModel> MapFromEntity(IEnumerable<API.Models.Customer> entities,
            IEnumerable<API.Models.Location> locations)
        {
            List<ViewModels.Customer.CustomerViewModel> models = null;
            if (entities != null)
            {
                models = new List<ViewModels.Customer.CustomerViewModel>();
                foreach (var entity in entities)
                {
                    models.Add(MapFromEntity(entity, locations));
                }
            }
            return models;
        }

    }
}
