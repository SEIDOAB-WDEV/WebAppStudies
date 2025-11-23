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
    public class GroupedPickModel : PageModel
    {
        //Just like for WebApi
        readonly IQuoteService _service = null;
        readonly ILogger<GroupedPickModel> _logger = null;

        //public member becomes part of the Model in the Razor page
        public SelectList QuoteSelection1 { get; set; }
        public List<FamousQuote> SelectedQuotes { get; set; } = new List<FamousQuote>();

        //ModelBinding for Selections
        [BindProperty]
        public Guid[] SelectedQuoteIds { get; set; }


        //Will execute on a Get request
        public IActionResult OnGet()
        {
            //Fill the selection list
            QuoteSelection1 = GetQuotes();
            return Page();
        }

        private SelectList GetQuotes()
        {
            var _quotes = _service.ReadQuotes();
            return new SelectList(_quotes,
                nameof(FamousQuote.QuoteId), nameof(FamousQuote.Quote), null, nameof(FamousQuote.Author));
        }

        public IActionResult OnPost()
        {
            //Fill the selection list
            QuoteSelection1 = GetQuotes();

            //use the service to find the quote according to selected quote id
            var _quotes = _service.ReadQuotes();
            SelectedQuotes = _quotes.Where(q => SelectedQuoteIds.Contains(q.QuoteId)).ToList();

            //Page is rendered as the postback is part of the form tag
            return Page();
        }

        //Inject services just like in WebApi
        public GroupedPickModel(IQuoteService service, ILogger<GroupedPickModel> logger)
        {
            _logger = logger;
            _service = service;
        }
    }
}
