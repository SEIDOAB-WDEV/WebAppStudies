using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Models;
using Services;
using WebAppStudies.Pages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebAppStudies.Pages
{
    //Demonstrate how to use the model to present a list of objects
    public enum Animals { Zebra, Tiger, Lion, Elephant }

    public class EnumPickModel : PageModel
    {
        //Just like for WebApi
        readonly IQuoteService _service = null;
        readonly ILogger<EnumPickModel> _logger = null;


        //ModelBinding for Selections
        [BindProperty]
        public Animals? SelectedAnimal1 { get; set; } = null;
        [BindProperty]
        public Animals? SelectedAnimal2 { get; set; } = null;
        [BindProperty]
        public List<Animals> SelectedAnimal3 { get; set; } = new List<Animals>();


        //Will execute on a Get request
        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            //Page is rendered as the postback is part of the form tag
            return Page();
        }


        //Inject services just like in WebApi
        public EnumPickModel(IQuoteService service, ILogger<EnumPickModel> logger)
        {
            _logger = logger;
            _service = service;
        }
    }
}
