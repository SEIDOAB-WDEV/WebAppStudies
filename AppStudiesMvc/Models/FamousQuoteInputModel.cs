using System;
using System.ComponentModel.DataAnnotations;
using WebAppStudies.SeidoHelpers;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace AppStudiesMVC.Models
{
        #region Input Model
        //InputModel (IM) is locally declared classes that contains ONLY the properties of the Model
        //that are bound to the <form> tag
        //EVERY property must be bound to an <input> tag in the <form>
        //These classes are in center of ModelBinding and Validation
        //public enum StatusIM { Unknown, Unchanged, Inserted, Modified, Deleted }

        public class FamousQuoteIMa
        {
            //Status of InputModel
            public StatusIM StatusIM { get; set; }

            //Properties from Model which is to be edited in the <form>
            public Guid QuoteId { get; init; } = Guid.NewGuid();

            [Required(ErrorMessage = "You must provide a quote")]
            public string Quote { get; set; }

            [Required(ErrorMessage = "You must provide an author")]
            public string Author { get; set; }

            #region constructors and model update
            public FamousQuoteIMa() { StatusIM = StatusIM.Unchanged; }

            //Copy constructor
            public FamousQuoteIMa(FamousQuoteIMa original)
            {
                StatusIM = original.StatusIM;

                QuoteId = original.QuoteId;
                Quote = original.Quote;
                Author = original.Author;
            }

            //Model => InputModel constructor
            public FamousQuoteIMa(FamousQuote original)
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
