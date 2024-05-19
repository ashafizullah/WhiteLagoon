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

            return RedirectToAction(nameof(Index));
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

    public IActionResult Update(int villaNumberId)
    {
        VillaNumberVM villaNumberVm = new()
        {
            VillaList = _db.Villas.ToList().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }),
            VillaNumber = _db.VillaNumbers.FirstOrDefault(x => x.Villa_Number == villaNumberId)
        };

        if (villaNumberVm.VillaNumber is null)
        {
            return RedirectToAction("Error", "Home");
        }

        return View(villaNumberVm);
    }

    [HttpPost]
    public IActionResult Update(VillaNumberVM villaNumberVm)
    {
        if (ModelState.IsValid)
        {
            _db.VillaNumbers.Update(villaNumberVm.VillaNumber);
            _db.SaveChanges();

            TempData["success"] = "Villa number has been updated successfully";

            return RedirectToAction(nameof(Index));
        }

        villaNumberVm.VillaList = _db.Villas.ToList().Select(x => new SelectListItem
        {
            Text = x.Name,
            Value = x.Id.ToString()
        });

        return View(villaNumberVm);
    }

    public IActionResult Delete(int villaNumberId)
    {
        VillaNumberVM villaNumberVm = new()
        {
            VillaList = _db.Villas.ToList().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }),
            VillaNumber = _db.VillaNumbers.FirstOrDefault(x => x.Villa_Number == villaNumberId)
        };

        if (villaNumberVm.VillaNumber is null)
        {
            return RedirectToAction("Error", "Home");
        }

        return View(villaNumberVm);
    }

    [HttpPost]
    public IActionResult Delete(VillaNumberVM villaNumberVm)
    {
        VillaNumber? villaNumber =
            _db.VillaNumbers.FirstOrDefault(x => x.Villa_Number == villaNumberVm.VillaNumber.Villa_Number);

        if (villaNumber is not null)
        {
            _db.VillaNumbers.Remove(villaNumber);
            _db.SaveChanges();

            TempData["success"] = "Villa number has been deleted successfully";
            return RedirectToAction(nameof(Index), "VillaNumber");
        }

        TempData["error"] = "Villa number could not be deleted";
        return View();
    }
}