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

            //if (Customer != null) _context.Customer.Add(Customer);
            //await _context.SaveChangesAsync();

            return RedirectToPage("../Index");
        }
    }
}
