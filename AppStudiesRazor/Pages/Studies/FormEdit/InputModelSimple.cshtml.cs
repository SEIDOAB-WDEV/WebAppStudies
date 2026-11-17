using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Models;
using Services;
using WebAppStudies.Pages;
using System.Security.Cryptography;

namespace WebAppStudies.Pages
{
    //Demonstrate how to read Query parameters
    public class InputModelSimpleModel : PageModel
    {
        //Just like for WebApi
        readonly IQuoteService _service = null;
        readonly ILogger<InputModelSimpleModel> _logger = null;

        [BindProperty]
        public FamousQuoteIM QuoteIM { get; set; }

        public string PageHeader { get; set; }

        //public member becomes part of the Model in the Razor page
        public string ErrorMessage { get; set; } = null;

        //Will execute on a Get request
        public IActionResult OnGet()
        {
            try
            {
                if (Guid.TryParse(Request.Query["id"], out Guid _id))
                {
                    //Use the Service and populate the InputModel
                    QuoteIM = new FamousQuoteIM(_service.ReadQuote(_id));
                    PageHeader = "Edit details of a quote";
                }
                else
                {
                    //Create an empty InputModel
                    QuoteIM = new FamousQuoteIM();
                    QuoteIM.StatusIM = StatusIM.Inserted;
                    PageHeader = "Create a new quote";
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return Page();
        }

        public IActionResult OnPostUndo()
        {
            //Use the Service and populate the InputModel
            QuoteIM = new FamousQuoteIM(_service.ReadQuote(QuoteIM.QuoteId));          
            PageHeader = "Edit details of a quote";
            return Page();
        }

        public IActionResult OnPostSave()
        {
            if (QuoteIM.StatusIM == StatusIM.Inserted)
            {
                //It is an create
                var model = QuoteIM.UpdateModel(new FamousQuote());
                model = _service.CreateQuote(model);

                QuoteIM = new FamousQuoteIM(model);
            }
            else
            {
                //It is an update
                //Get orginal
                var model = _service.ReadQuote(QuoteIM.QuoteId);

                //update the changes and save
                model = QuoteIM.UpdateModel(model);
                model = _service.UpdateQuote(model);
                
                QuoteIM = new FamousQuoteIM(model);
            }

            PageHeader = "Edit details of a quote";
            return Page();
        }


        //Inject services just like in WebApi
        public InputModelSimpleModel(IQuoteService service, ILogger<InputModelSimpleModel> logger)
        {
            _logger = logger;
            _service = service;
        }

        #region Input Model
        //InputModel (IM) is locally declared classes that contains ONLY the properties of the Model
        //that are bound to the <form> tag
        //EVERY property must be bound to an <input> tag in the <form>
        //These classes are in center of ModelBinding and Validation
        public enum StatusIM { Unknown, Unchanged, Inserted, Modified, Deleted }

        public class FamousQuoteIM
        {
            //Status of InputModel
            public StatusIM StatusIM { get; set; }

            //Properties from Model which is to be edited in the <form>
            public Guid QuoteId { get; init; } = Guid.NewGuid();
            public string Quote { get; set; }
            public string Author { get; set; }

            #region constructors and model update
            public FamousQuoteIM() { StatusIM = StatusIM.Unchanged; }

            //Copy constructor
            public FamousQuoteIM(FamousQuoteIM original)
            {
                StatusIM = original.StatusIM;

                QuoteId = original.QuoteId;
                Quote = original.Quote;
                Author = original.Author;
            }

            //Model => InputModel constructor
            public FamousQuoteIM(FamousQuote original)
            {
                StatusIM = StatusIM.Unchanged;
                QuoteId = original.QuoteId;
                Quote = original.Quote;
                Author = original.Author;
            }

            //InputModel => Model
            public FamousQuote UpdateModel(FamousQuote model)
            {
                model.QuoteId = QuoteId;
                model.Quote = Quote;
                model.Author = Author;
                return model;
            }
            #endregion

        }
        #endregion
    }
}