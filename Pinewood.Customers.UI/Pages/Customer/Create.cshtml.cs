﻿using Microsoft.AspNetCore.Mvc;
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
    public class CreateModel : PageModel
    {
        public string JsonContent { get; set; }

        public List<Pinewood.Customer.Business.Customer> customerList;

        private readonly ILogger<IndexModel> _logger;

        public CreateModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            return Page();

        }

        [BindProperty]
        public Pinewood.Customer.Business.Customer? Customer { get; set; }

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
