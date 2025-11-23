using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Models;
using Services;
using WebAppStudies.Pages;

namespace WebAppStudies.Pages
{
    //Demonstrate how to use the model to present a list of objects
    //https://www.learnrazorpages.com/razor-pages/forms/checkboxes

    public class RadiobuttonListModel : PageModel
    {
        //Just like for WebApi
        readonly IQuoteService _service = null;
        readonly ILogger<RadiobuttonListModel> _logger = null;

        [BindProperty]
        public Guid? SelectedQuoteId1 { get; set; } = null;
        [BindProperty]
        public Guid? SelectedQuoteId2 { get; set; } = null;

        [BindProperty]
        public List<FamousQuote> Quotes1 { get; set; } = new List<FamousQuote>();
        [BindProperty]
        public List<FamousQuote> Quotes2 { get; set; } = new List<FamousQuote>();

        public FamousQuote SelectedQuote1 { get; set; } = null;
        public FamousQuote SelectedQuote2 { get; set; } = null;


        //Will execute on a Get request
        public IActionResult OnGet()
        {
            //Use the Service
            Quotes1 = _service.ReadQuotes().Take(3).ToList();
            Quotes2 = _service.ReadQuotes().TakeLast(3).ToList();

            return Page();
        }

        public IActionResult OnPost()
        {
            //Page is rendered as the postback is part of the form tag
            if (SelectedQuoteId1 != null)
            {
                SelectedQuote1 = _service.ReadQuote(SelectedQuoteId1.Value);
            }

            if (SelectedQuoteId2 != null)
            {
                SelectedQuote2 = _service.ReadQuote(SelectedQuoteId2.Value);
            }

            return Page();
        }

        //Inject services just like in WebApi
        public RadiobuttonListModel(IQuoteService service, ILogger<RadiobuttonListModel> logger)
        {
            _logger = logger;
            _service = service;
        }
    }
}
