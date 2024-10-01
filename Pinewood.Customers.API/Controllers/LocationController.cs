using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pinewood.Customers.API.Models;
using System.IO;
using System.Text.Json;
using Pinewood.Customers.API.ViewModels.Customer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Pinewood.Customers.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LocationController : ControllerBase
    {

        private string filePathLoc = "Locations.json";


        private readonly ILogger<CustomerController> _logger;
        private string fileContentsLoc;
        private List<Location> locList;


        public LocationController(ILogger<CustomerController> logger)
        {
            _logger = logger;

            locList = new();

            try
            {
                fileContentsLoc = System.IO.File.ReadAllText(filePathLoc);

                locList = JsonConvert.DeserializeObject<List<Location>>(fileContentsLoc);

            }

            catch (IOException e)
            {
                _logger.LogError($"A file error occurred: {e.Message}");
            }

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/location")]
        public List<Location> Get()
        {
            return locList;
        }


    }
}
