using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Pinewood.Customer.Business;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace Pinewood.Customers.UI.Pages
{
    public class EditModel : PageModel
    {
        public string JsonContent { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public EditModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            string customerID = HttpContext.Request.Query["id"][0];


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:43681/customer/api/customer/"+customerID);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            string jsonResult = "";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                jsonResult = reader.ReadToEnd();

            }

            this.JsonContent = jsonResult.ToString();

            this.Customer = JsonConvert.DeserializeObject<Pinewood.Customer.Business.Customer>(JsonContent);

            //--------------------------------Get Locations

            HttpWebRequest requestLoc = (HttpWebRequest)WebRequest.Create("http://localhost:43681/location/api/location");
            requestLoc.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            string jsonResultLoc = "";
            using (HttpWebResponse responseLoc = (HttpWebResponse)requestLoc.GetResponse())
            using (Stream stream = responseLoc.GetResponseStream())
            using (StreamReader readerLoc = new StreamReader(stream))
            {
                jsonResultLoc = readerLoc.ReadToEnd();

            }

            this.JsonContent = jsonResultLoc.ToString();

            this.locList = JsonConvert.DeserializeObject<List<Pinewood.Customer.Business.Location>>(JsonContent);

            return Page();

        }

        [BindProperty]
        public Pinewood.Customer.Business.Customer? Customer { get; set; }

        [BindProperty]
        public List<Pinewood.Customer.Business.Location>? locList { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:43681/customer/api/customer/post");
            request.ContentType = "application/json";
            request.Method = "POST";

            string jsonPost = JsonConvert.SerializeObject(Customer);

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(jsonPost);
            }

            string jsonResult = "";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                jsonResult = reader.ReadToEnd();

            }

            this.JsonContent = jsonResult.ToString();

            return RedirectToPage("../Index");
        }
    }
}
