using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers;

public class VillaNumberController : Controller
{
    private readonly ApplicationDBContext _db;

    public VillaNumberController(ApplicationDBContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        var villaNumbers = _db.VillaNumbers.ToList();
        return View(villaNumbers);
    }

    public IActionResult Create()
    {
        VillaNumberVM villaNumberVm = new()
        {
            VillaList = _db.Villas.ToList().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            })
        };

        return View(villaNumberVm);
    }

    [HttpPost]
    public IActionResult Create(VillaNumber villaNumber)
    {
        //ModelState.Remove("Villa");
        
        if (ModelState.IsValid)
        {
            _db.VillaNumbers.Add(villaNumber);
            _db.SaveChanges();
            TempData["success"] = "Villa number has been saved successfully";

            return RedirectToAction("Index");
        }

        return View();
    }
}