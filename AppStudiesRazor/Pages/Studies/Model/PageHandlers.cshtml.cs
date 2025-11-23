using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppStudies.Pages
{
	public class PageHandlersModel : PageModel
    {
        [FromQuery(Name = "idGet")]
        public Guid? IdGet { get; set; }


        public string Message { get; set; }
        public IActionResult OnGet()
        {
            if (!IdGet.HasValue)
            {
                Message = $"Initial page load";
            }
            else
            {
                Message = $"Get fired: {IdGet}";
            }
            return Page();
        }

        public IActionResult OnPost(Guid id)
        {
             Message = $"Post fired: {id}";
             return Page();
        }

        public IActionResult OnPostDelete(Guid id)
        {
            Message = $"PostDeletehandler fired: {id}";
            return Page();
        }

        public IActionResult OnPostEdit(Guid id)
        {
            Message = $"PostEdithandler fired: {id}";
            return Page();
        }

        public IActionResult OnPostView(string name, Guid vid)
        {
            Message = $"PostView handler fired: {name}, {vid}";
            return Page();
        }
    }
}
