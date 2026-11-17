    using System;
using System.ComponentModel.DataAnnotations;
using WebAppStudies.SeidoHelpers;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace AppStudiesMVC.Models
{
    public class InputSimpleViewModel
    {
        [BindProperty]
        public FamousQuoteIMa QuoteIM { get; set; }

        public string PageHeader { get; set; }

        //public member becomes part of the Model in the Razor page
        public string ErrorMessage { get; set; } = null;
    }
}