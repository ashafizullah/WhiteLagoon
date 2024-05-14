using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers;

public class VillaController : Controller
{
    private readonly ApplicationDBContext _db;

    public VillaController(ApplicationDBContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        var villas = _db.Villas.ToList();

        return View(villas);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Villa villa)
    {
        if (villa.Name == villa.Description)
        {
            ModelState.AddModelError("description", "The description cannot exactly match the name");    
        }
        
        if (ModelState.IsValid)
        {
            _db.Villas.Add(villa);
            _db.SaveChanges();
            
            TempData["success"] = "Villa has been saved successfully";
            return RedirectToAction("Index");
        }

        return View();
    }

    public IActionResult Update(int villaId)
    {
        Villa? villa = _db.Villas.FirstOrDefault(x => x.Id == villaId);
        /*Villa? villa = _db.Villas.Find(villaId);
        Villa? villa = _db.Villas.Where(x => x.Id == villaId);*/

        if (villa is null)
        {
            return RedirectToAction("Error", "Home");
        }

        return View(villa);
    }

    [HttpPost]
    public IActionResult Update(Villa villa)
    {
        if (ModelState.IsValid && villa.Id > 0)
        {
            _db.Villas.Update(villa);
            _db.SaveChanges();
            
            TempData["success"] = "Villa has beed updated successfully";
            return RedirectToAction("Index");
        }
        
        TempData["error"] = "Villa could not be updated";
        return View(villa);
    }

    public IActionResult Delete(int villaId)
    {
        Villa? villa = _db.Villas.FirstOrDefault(x => x.Id == villaId);

        if (villa is null)
        {
            return RedirectToAction("Error", "Home");
        }

        return View(villa);
    }

    [HttpPost]
    public IActionResult Delete(Villa villa)
    {
        Villa? checkVilla = _db.Villas.FirstOrDefault(x => x.Id == villa.Id);

        if (checkVilla is not null)
        {
            _db.Villas.Remove(checkVilla);
            _db.SaveChanges();
            TempData["success"] = "Villa has been deleted successfully";
            
            return RedirectToAction("Index");
        }
        
        TempData["error"] = "Villa could not be deleted";
        return View();
    }
}