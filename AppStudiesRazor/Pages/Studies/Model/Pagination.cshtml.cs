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
    public class PaginationModel : PageModel
    {
        //Just like for WebApi
        readonly IQuoteService _service = null;
        readonly ILogger<PaginationModel> _logger = null;

        //public member becomes part of the Model in the Razor page
        public List<FamousQuote> Quotes { get; set; } = new List<FamousQuote>();

        //Pagination
        public int NrOfPages { get; set; }
        public int PageSize { get; } = 5;

        public int ThisPageNr { get; set; } = 0;
        public int PrevPageNr { get; set; } = 0;
        public int NextPageNr { get; set; } = 0;
        public int PresentPages { get; set; } = 0;



        //Will execute on a Get request
        public IActionResult OnGet(string pagenr)
        {
            //Read a QueryParameter
            if (int.TryParse(pagenr, out int _pagenr))
            {
                ThisPageNr = _pagenr;
            }

            //Pagination
            NrOfPages = (int) Math.Ceiling((double)_service.NrOfQuotes() / PageSize);
            PrevPageNr = Math.Max(0, ThisPageNr - 1);
            NextPageNr = Math.Min(NrOfPages-1, ThisPageNr + 1);
            PresentPages = Math.Min(3, NrOfPages);

            //Use the Service
            Quotes = _service.ReadQuotes(ThisPageNr, PageSize);

            return Page();
        }

        //Inject services just like in WebApi
        public PaginationModel(IQuoteService service, LatinService latin, ILogger<PaginationModel> logger)
        {
            _logger = logger;
            _service = service;
        }
    }
}
