using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Models;
using Newtonsoft.Json;
using Services;

namespace WebAppStudies.Pages
{
    public class InputModelListModel : PageModel
    {
        //Just like for WebApi
        readonly IQuoteService _service = null;
        readonly ILogger<InputModelListModel> _logger = null;

        //InputModel (IM) is locally declared classes that contains ONLY the properties of the Model
        //that are bound to the <form> tag
        //EVERY property must be bound to an <input> tag in the <form>
        [BindProperty]
        public List<FamousQuoteIM> QuotesIM { get; set; }

        #region HTTP Requests
        public IActionResult OnGet()
        {
            QuotesIM = _service.ReadQuotes().Select(q => new FamousQuoteIM(q)).ToList();
            return Page();
        }

        public IActionResult OnPostDelete(Guid quoteId)
        {
            //Set the Quote as deleted, it will not be rendered
            QuotesIM.First(q => q.QuoteId == quoteId).StatusIM = StatusIM.Deleted;
            return Page();
        }

        public IActionResult OnPostEdit(Guid quoteId)
        {
            //Set the Quote as Modified, it will later be updated in the database
            var q = QuotesIM.First(q => q.QuoteId == quoteId);
            q.StatusIM = StatusIM.Modified;

            //Implement the changes
            q.Author = q.EditAuthor;
            q.Quote = q.EditQuote;
            return Page();
        }

        public IActionResult OnPostUndo()
        {
            //Reload the InputModel
            QuotesIM = _service.ReadQuotes().Select(q => new FamousQuoteIM(q)).ToList();
            return Page();
        }

        public IActionResult OnPostSave()
        {
            //Check if there are deleted quotes, if so simply remove them
            var _deletes = QuotesIM.FindAll(q => (q.StatusIM == StatusIM.Deleted));
            foreach (var item in _deletes)
            {
                //Remove from the database
                _service.DeleteQuote(item.QuoteId);
            }

            //Check if there are any modified quotes , if so update them in the database
            var _modyfies = QuotesIM.FindAll(a => (a.StatusIM == StatusIM.Modified));
            foreach (var item in _modyfies)
            {
                //get model
                var model = _service.ReadQuote(item.QuoteId);

                //update the changes and save
                model = item.UpdateModel(model);
                _service.UpdateQuote(model);
            }

            //Reload the InputModel
            QuotesIM = _service.ReadQuotes().Select(q => new FamousQuoteIM(q)).ToList();
            return Page();
        }
        #endregion

        #region Constructors
        //Inject services just like in WebApi
        public InputModelListModel(IQuoteService service, ILogger<InputModelListModel> logger)
        {
            _service = service;
            _logger = logger;
        }
        #endregion

        #region Input Model
        //InputModel (IM) is locally declared classes that contains ONLY the properties of the Model
        //that are bound to the <form> tag
        //EVERY property must be bound to an <input> tag in the <form>
        //These classes are in center of ModelBinding and Validation
        public enum StatusIM { Unknown, Unchanged, Inserted, Modified, Deleted}

        public class FamousQuoteIM
        {
            //Status of InputModel
            public StatusIM StatusIM { get; set; }

            //Properties from Model which is to be edited in the <form>
            public Guid QuoteId { get; init; } = Guid.NewGuid();
            public string Quote { get; set; }
            public string Author { get; set; }

            //Added properites to edit in the list with undo
            public string EditQuote { get; set; }
            public string EditAuthor { get; set; }

            #region constructors and model update
            public FamousQuoteIM() { StatusIM = StatusIM.Unchanged; }

            //Copy constructor
            public FamousQuoteIM(FamousQuoteIM original)
            {
                StatusIM = original.StatusIM;

                QuoteId = original.QuoteId;
                Quote = original.Quote;
                Author = original.Author;

                EditQuote = original.EditQuote;
                EditAuthor = original.EditAuthor;
            }

            //Model => InputModel constructor
            public FamousQuoteIM(FamousQuote original)
            {
                StatusIM = StatusIM.Unchanged;
                QuoteId = original.QuoteId;
                Quote = EditQuote = original.Quote;
                Author = EditAuthor = original.Author;
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
