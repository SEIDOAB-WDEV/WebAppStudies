using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Seido.Utilities.SeedGenerator;
using Models;

namespace WebAppStudies.Pages.Studies
{
    public class LatSentence
    {
        public Guid id { get; set; }
        public string Sentence { get; set; }
    }

	public class ModalsPostModel : PageModel
    {
        [FromQuery(Name = "idGet")]
        public string IdGet { get; set; }

        [BindProperty]
        public LatSentence Latin { get; set; } = new LatSentence();

        public string Message { get; set; }
        public IActionResult OnGet()
        {
            var rnd = new SeedGenerator();
            Latin = new LatSentence { id = Guid.NewGuid(), Sentence = rnd.LatinSentence };

            Message = $"Latin created: {Latin.id}, {Latin.Sentence}";

            return Page();
        }

        public IActionResult OnPost(Guid id)
        {
            Message = $"Post() fired with id: {id}";

            //Page is rendered as the postback is part of the form tag
            return Page();
        }

        public IActionResult OnPostSave(Guid id)
        {
            Message = $"PostSave() fired with id: {id}";

            //Page is rendered as the postback is part of the form tag
            return Page();
        }
    }
}
