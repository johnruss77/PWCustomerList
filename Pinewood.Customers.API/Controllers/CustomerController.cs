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
    public class CustomerController : ControllerBase
    {

        private string filePathCust = "Customers.json";
        private string filePathLoc = "Locations.json";


        private readonly ILogger<CustomerController> _logger;
        private List<ViewModels.Customer.CustomerViewModel> customerList;
        private string fileContentsCust;
        private string fileContentsLoc;
        private List<Customer> custList;
        private List<Location> locList;


        public CustomerController(ILogger<CustomerController> logger)
        {
            _logger = logger;

            customerList = new();

            try
            {
                fileContentsCust = System.IO.File.ReadAllText(filePathCust);
                fileContentsLoc = System.IO.File.ReadAllText(filePathLoc);

                custList = JsonConvert.DeserializeObject<List<Customer>>(fileContentsCust);
                locList = JsonConvert.DeserializeObject<List<Location>>(fileContentsLoc);

            }

            catch (IOException e)
            {
                _logger.LogError($"A file error occurred: {e.Message}");
            }


        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/customer")]
        public List<ViewModels.Customer.CustomerViewModel> Get()
        {
            try
            {
                customerList = Mappers.CustomerMapper.MapFromEntity(custList, locList)
                    .OrderBy(e => e.Name)
                    .ToList();

            }

            catch (Exception e)
            {

                var message = ("Pinewood.Customers.API.Controllers.CustomerController.Get: " + e.InnerException);
                _logger.LogError(message);
            }

            return customerList;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/customer/{id}")]
        public ViewModels.Customer.CustomerViewModel Get(int id)
        {
            CustomerViewModel customer = null;

            try
            {
                customerList = Mappers.CustomerMapper.MapFromEntity(custList, locList)
                    .OrderBy(e => e.Name)
                    .ToList();

                customer = customerList.Where(c => c.ID == id).FirstOrDefault();

                if (customer == null)
                {
                    // Create new
                    int latestID = customerList.OrderByDescending(item => item.ID).First().ID;

                    customer = new()
                    {
                        ID = latestID + 1,
                        Name = "",
                        Email = "",
                        Phone = "",
                        LocationID = 0
                    };
                
                }

            }

            catch (Exception e)
            {

                var message = ("Pinewood.Customers.API.Controllers.CustomerController.Get{id}: " + e.InnerException);
                _logger.LogError(message);

            }

            return customer;

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/customer/Delete/{id}")]
        public OperationResult<bool> Delete(int id)
        {
            var opresult = new OperationResult<bool>();

            try
            {
                customerList = Mappers.CustomerMapper.MapFromEntity(custList, locList)
                    .OrderBy(e => e.Name)
                    .ToList();

                // check if customer is valid to be deleted

                CustomerViewModel customer = null;
                customer = customerList.Where(c => c.ID == id).FirstOrDefault();

                if (customer != null)
                {
                    // delete

                    IEnumerable<CustomerViewModel> updatedCustomerList = new List<CustomerViewModel>();

                    updatedCustomerList = customerList.Where(c => c.ID != id);

                    // update customer.json file to remove the deleted customer

                    List<CustomerViewModel> newCustList = updatedCustomerList.ToList();

                    string custListStr = JsonConvert.SerializeObject(newCustList, Formatting.Indented);

                    if (custListStr == String.Empty)
                    {
                        throw new Exception("Error updating Customer list");
                    }

                    // Write the updated JSON back to the file
                    System.IO.File.WriteAllText(filePathCust, custListStr);

                    opresult.Data = true;
                    opresult.Success = true;
                    opresult.Message = "OK";

                }
                else
                {
                    opresult.Data = false;
                    opresult.Success = false;
                    var message = ("Pinewood.Customers.API.Controllers.CustomerController.Delete: No customer with that ID");
                    opresult.Message = message;

                }

            }
            catch (Exception e)
            {

                var message = ("Pinewood.Customers.API.Controllers.CustomerController.Delete: " + e.InnerException);
                _logger.LogError(message);
                opresult.AddError(message);
                opresult.Success = false;
                opresult.Message = message;
            }

            return opresult;

         }


        [AllowAnonymous]
        [HttpPost]
        [Route("api/customer/post")]
        public OperationResult Post(ViewModels.Customer.CustomerPostModel customerPostModel)
        {
            var opresult = new OperationResult<List<ViewModels.Customer.CustomerViewModel>>
            {
                Success = true,

            };

            string custListStr = String.Empty;

            try
            {
                customerList = Mappers.CustomerMapper.MapFromEntity(custList, locList)
                    .OrderBy(e => e.Name)
                    .ToList();

                // check if customer is being updated

                CustomerViewModel customer = null;
                customer = customerList.Where(c => c.ID == customerPostModel.ID).FirstOrDefault();

                if (customer != null) {
                    // update

                    IEnumerable<CustomerViewModel> updatedCustomerList = new List<CustomerViewModel>();

                    customer.Name = customerPostModel.Name;
                    customer.Email = customerPostModel.Email;
                    customer.Phone = customerPostModel.Phone;
                    customer.LocationID = customerPostModel.LocationID;

                    updatedCustomerList = customerList.Where(c => c.ID != customerPostModel.ID);
                    List<CustomerViewModel> newCustList = updatedCustomerList.Append(customer).ToList();
                    
                    // update customer.json with modified customer

                    custListStr = JsonConvert.SerializeObject(newCustList, Formatting.Indented);
                }
                else
                {
                    // Create new

                    customer = new()
                    {
                        ID = customerPostModel.ID,
                        Name = customerPostModel.Name,
                        Email = customerPostModel.Email,
                        Phone = customerPostModel.Phone,
                        LocationID = customerPostModel.LocationID
                    };

                    List<CustomerViewModel> newCustList = customerList.Append(customer).ToList();

                    custListStr = JsonConvert.SerializeObject(newCustList, Formatting.Indented);

                }

                if (custListStr == String.Empty)
                {
                    throw new Exception("Error updating Customer list");
                }

                // Write the updated JSON back to the file
                System.IO.File.WriteAllText(filePathCust, custListStr);

            }
            catch (Exception e)
            {

                var message = ("Pinewood.Customers.API.Controllers.CustomerController.Post: " + e.InnerException);
                _logger.LogError(message);
                opresult.AddError(message);
                opresult.Success = false;
                opresult.Message = message;
            }

            return opresult;

        }

    }
}
