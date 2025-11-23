using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using WebAppStudies.SeidoHelpers;
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
    public class ServerValidationListAdd : PageModel
    {
        //Just like for WebApi
        readonly IQuoteService _service = null;
        readonly ILogger<ServerValidationListAdd> _logger = null;

        //InputModel (IM) is locally declared classes that contains ONLY the properties of the Model
        //that are bound to the <form> tag
        //EVERY property must be bound to an <input> tag in the <form>
        [BindProperty]
        public List<FamousQuoteIM> QuotesIM { get; set; }

        [BindProperty]
        public FamousQuoteIM NewQuoteIM { get; set; } = new FamousQuoteIM();

        //For Validation
        public ModelValidationResult ValidationResult { get; set; } = new ModelValidationResult(false, null, null);

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
            int idx = QuotesIM.FindIndex(q => q.QuoteId == quoteId);
            string[] keys = { $"QuotesIM[{idx}].EditQuote",
                            $"QuotesIM[{idx}].EditAuthor"};
            if (!ModelState.IsValidPartially(out ModelValidationResult validationResult, keys))
            {
                ValidationResult = validationResult;
                return Page();
            }

            //Set the Quote as Modified, it will later be updated in the database
            var q = QuotesIM.First(q => q.QuoteId == quoteId);
            q.StatusIM = StatusIM.Modified;

            //Implement the changes
            q.Author = q.EditAuthor;
            q.Quote = q.EditQuote;
            return Page();
        }

        public IActionResult OnPostAdd()
        {
            string[] keys = { $"NewQuoteIM.Quote",
                            $"NewQuoteIM.Author"};
            if (!ModelState.IsValidPartially(out ModelValidationResult validationResult, keys))
            {
                ValidationResult = validationResult;
                return Page();
            }

            //Set the Artist as Inserted, it will later be inserted in the database
            NewQuoteIM.StatusIM = StatusIM.Inserted;

            //Need to add a temp Guid so it can be deleted and editited in the form
            //A correct Guid will be created by the DTO when Inserted into the database
            NewQuoteIM.QuoteId = Guid.NewGuid();

            //Add it to the Input Models artists
            QuotesIM.Add(new FamousQuoteIM(NewQuoteIM));

            //Clear the NewArtist so another album can be added
            NewQuoteIM = new FamousQuoteIM();

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
            //Note: Here I will not do any validation as all validation is done during the
            //OnPostEdit and OnPostAdd

            //Check if there are deleted quotes, if so simply remove them
            var _deletes = QuotesIM.FindAll(q => (q.StatusIM == StatusIM.Deleted));
            foreach (var item in _deletes)
            {
                //Remove from the database
                _service.DeleteQuote(item.QuoteId);
            }

            #region Add quotes
            //Check if there are any new quotes added, if so create them in the database
            var _newies = QuotesIM.FindAll(q => (q.StatusIM == StatusIM.Inserted));
            foreach (var item in _newies)
            {
                //Create the corresposning model
                var model = item.UpdateModel(new FamousQuote());

                //create in the database
                model = _service.CreateQuote(model);
            }
            #endregion

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
        public ServerValidationListAdd(IQuoteService service, ILogger<ServerValidationListAdd> logger)
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
            public Guid QuoteId { get; set; } = Guid.NewGuid();

            [Required(ErrorMessage = "You type provide a quote")]
            public string Quote { get; set; }

            [Required(ErrorMessage = "You must provide an author")]
            public string Author { get; set; }

            //Added properites to edit in the list with undo
            [Required(ErrorMessage = "You must provide an quote")]
            public string EditQuote { get; set; }

            [Required(ErrorMessage = "You must provide an author")]
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
