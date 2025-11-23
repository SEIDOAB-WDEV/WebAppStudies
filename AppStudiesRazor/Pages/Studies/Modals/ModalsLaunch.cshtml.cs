using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure.Core;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Routing;

namespace WebAppStudies.Pages
{
    public class ModalData
    {
        public string postdata { get; set; } 
    }

    public class ModalsLaunchModel : PageModel
    {
        public List<string> Messages { get; set; } = new List<string>();

        public IActionResult OnPostSelect(Guid groupId)
        {
            Messages.Add($"OnPostSelect fired: {groupId}");
            return Page();
        }
    }
}
