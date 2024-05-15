using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        var villaNumbers = _db.VillaNumbers.Include(x => x.Villa).ToList();
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
    public IActionResult Create(VillaNumberVM villaNumberVm)
    {
        //ModelState.Remove("Villa");
        bool checkVillaNumber = _db.VillaNumbers.Any(x => x.Villa_Number == villaNumberVm.VillaNumber.Villa_Number);
        
        if (ModelState.IsValid && !checkVillaNumber)
        {
            _db.VillaNumbers.Add(villaNumberVm.VillaNumber);
            _db.SaveChanges();
            TempData["success"] = "Villa number has been saved successfully";

            return RedirectToAction("Index");
        }
        
        if (checkVillaNumber)
        {
            TempData["error"] = "The villa number is already exists";
        }

        villaNumberVm.VillaList = _db.Villas.ToList().Select(x => new SelectListItem
        {
            Text = x.Name,
            Value = x.Id.ToString()
        });

        return View(villaNumberVm);
    }
}