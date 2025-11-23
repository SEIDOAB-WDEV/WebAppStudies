using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AppStudiesMVC.Models;
using Services;
using System.Net.WebSockets;
using Models;

namespace AppStudiesMVC.Controllers;

public class FormController : Controller
{
    readonly ILogger<FormController> _logger;
    readonly IQuoteService _service = null;


    public FormController(ILogger<FormController> logger, IQuoteService service)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult InputModelSimple()
    {
        var vm = new InputSimpleViewModel();
        try
        {
            if (Guid.TryParse(Request.Query["id"], out Guid _id))
            {
                //Use the Service and populate the InputModel
                vm.QuoteIM = new FamousQuoteIMa(_service.ReadQuote(_id));
                vm.PageHeader = "Edit details of a quote";
            }
            else
            {
                //Create an empty InputModel
                vm.QuoteIM = new FamousQuoteIMa();
                vm.QuoteIM.StatusIM = StatusIM.Inserted;
                vm.PageHeader = "Create a new quote";
            }
        }
        catch (Exception e)
        {
            vm.ErrorMessage = e.Message;
        }
        return View(vm);
    }

    [HttpPost]
    public IActionResult InputModelSimpleUndo(InputSimpleViewModel vm)
    {
        //Use the Service and populate the InputModel
        vm.QuoteIM = new FamousQuoteIMa(_service.ReadQuote(vm.QuoteIM.QuoteId));          
        vm.PageHeader = "Edit details of a quote";
        return View("InputModelSimple", vm);
    }


    [HttpPost]
    public IActionResult InputModelSimpleSave(InputSimpleViewModel vm)
    {
        if (vm.QuoteIM.StatusIM == StatusIM.Inserted)
        {
            //It is an create
            var model = vm.QuoteIM.UpdateModel(new FamousQuote());
            model = _service.CreateQuote(model);

            vm.QuoteIM = new FamousQuoteIMa(model);
        }
        else
        {
            //It is an update
            //Get orginal
            var model = _service.ReadQuote(vm.QuoteIM.QuoteId);

            //update the changes and save
            model = vm.QuoteIM.UpdateModel(model);
            model = _service.UpdateQuote(model);
            
            vm.QuoteIM = new FamousQuoteIMa(model);
        }

        vm.PageHeader = "Edit details of a quote";
        return View("InputModelSimple", vm);
    }
}

