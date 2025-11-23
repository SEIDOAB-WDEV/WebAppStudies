using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace AppStudiesMVC.Models;

public class LatinViewModel
{
        public List<LatinSentence> Latins { get; set; } = new List<LatinSentence>();

        //Pagination
        public int NrOfPages { get; set; }
        public int PageSize { get; } = 5;

        public int ThisPageNr { get; set; } = 0;
        public int PrevPageNr { get; set; } = 0;
        public int NextPageNr { get; set; } = 0;
        public int PresentPages { get; set; } = 0;

        //ModelBinding for the form
        [BindProperty]
        public string SearchFilter { get; set; }


        public void UpdatePagination(LatinService service)
        {
            //Pagination
            NrOfPages = (int)Math.Ceiling((double)service.Count(SearchFilter) / PageSize);
            PrevPageNr = Math.Max(0, ThisPageNr - 1);
            NextPageNr = Math.Min(NrOfPages - 1, ThisPageNr + 1);
            PresentPages = Math.Min(3, NrOfPages);
        }
}