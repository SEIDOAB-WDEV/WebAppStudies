using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Models;
using Services;
using WebAppStudies.Pages;
using Azure.Core;

namespace WebAppStudies.Pages
{
    //Demonstrate how to use the model to present a list of objects
    public class ModelListModel : PageModel
    {
        //Just like for WebApi
        readonly IQuoteService _service = null;
        readonly ILogger<ModelListModel> _logger = null;

        //public member becomes part of the Model in the Razor page
        public List<FamousQuote> Quotes { get; set; } = new List<FamousQuote>();


        //Will execute on a Get request
        public IActionResult OnGet()
        {
            //Just to show how to get current uri
            var uri = Request.Path;

            //Use the Service
            Quotes = _service.ReadQuotes();
            return Page();
        }

        //Inject services just like in WebApi
        public ModelListModel(IQuoteService service, ILogger<ModelListModel> logger)
        {
            _logger = logger;
            _service = service;
        }
    }
}
