using Microsoft.AspNetCore.Mvc;
using TransitDesk.Core.Entities;
using TransitDesk.Web.ViewModels;

namespace TransitDesk.Web.Controllers;

public class VehiclesController : Controller
{
    private static readonly List<Vehicle> _vehicles = new();

    public IActionResult Index()
    {
        return View(_vehicles);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new VehicleViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(VehicleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var entity = new Vehicle
        {
            SideNumber = model.SideNumber,
            ModelName = model.ModelName,
            DepotName = model.DepotName,
            ProductionYear = model.ProductionYear,
            Capacity = model.Capacity,
            IsLowFloor = model.IsLowFloor,
            ServiceStatus = model.ServiceStatus
        };

        _vehicles.Add(entity);

        TempData["SuccessMessage"] = $"Pojazd o numerze bocznym {entity.SideNumber} został pomyślnie zarejestrowany w systemie.";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult DepotFleetList(string depotName, string? tractionType)
    {
        ViewBag.DepotName = depotName;
        ViewBag.TractionType = tractionType ?? "Wszystkie typy trakcji";
        return View();
    }

    [HttpGet("pojazd/{sideNumber:fleetSideNo}")]
    public IActionResult Details(int sideNumber)
    {
        ViewBag.SideNumber = sideNumber;
        return View();
    }
}