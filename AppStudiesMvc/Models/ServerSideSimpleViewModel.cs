using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Models;
using Services;

namespace AppStudiesMVC.Models;

public class ServerSideSimpleViewModel
{
    [BindProperty]
    public FamousQuoteIMa QuoteIM { get; set; }

    public string PageHeader { get; set; }

    //public member becomes part of the Model in the Razor page
    public string ErrorMessage { get; set; } = null;


    //For Server Side Validation set by IsValid()
    public bool HasValidationErrors { get; set; }
    public IEnumerable<string> ValidationErrorMsgs { get; set; }
    public IEnumerable<KeyValuePair<string, ModelStateEntry>> InvalidKeys { get; set; }
}
