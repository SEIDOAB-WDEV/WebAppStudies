using System;
using Models;

namespace AppStudiesMVC.Models
{
	public class ModelListViewModel
	{
        //public member becomes part of the Model in the Razor page
        public List<FamousQuote> Quotes { get; set; } = new List<FamousQuote>();

        public ModelListViewModel()
		{
		}
	}
}

