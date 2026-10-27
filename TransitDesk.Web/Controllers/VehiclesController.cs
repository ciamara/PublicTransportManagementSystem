using Microsoft.AspNetCore.Mvc;
using TransitDesk.Core.Entities;
using TransitDesk.Core.Enums;
using TransitDesk.Web.Extensions;
using TransitDesk.Web.Filters;
using TransitDesk.Web.ViewModels;

namespace TransitDesk.Web.Controllers;

public class VehiclesController : Controller
{
    private const string WatchlistSessionKey = "MonitoredVehiclesWatchlist";

    private static readonly List<Vehicle> _vehicles = new()
    {
        new Vehicle { SideNumber = 1052, ModelName = "BMW", DepotName = "Lawendowe Wzgórze", ProductionYear = 2019, Capacity = 200, IsLowFloor = true, ServiceStatus = ServiceStatus.W_Ruchu },
        new Vehicle { SideNumber = 2011, ModelName = "Mercedes", DepotName = "Piekarnicza", ProductionYear = 2020, Capacity = 150, IsLowFloor = true, ServiceStatus = ServiceStatus.W_Ruchu }
    };

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
    [AuditLog]
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddVehicleToWatchlist(int sideNumber)
    {
        var watchlist = HttpContext.Session.GetObject<List<int>>(WatchlistSessionKey) ?? new List<int>();

        if (!watchlist.Contains(sideNumber))
        {
            watchlist.Add(sideNumber);
            HttpContext.Session.SetObject(WatchlistSessionKey, watchlist);
            TempData["SuccessMessage"] = $"Pojazd #{sideNumber} dodany do monitorowanych.";
        }
        else
        {
            TempData["InfoMessage"] = $"Pojazd #{sideNumber} już znajduje się na liście monitorowanych.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Watchlist()
    {
        var watchlist = HttpContext.Session.GetObject<List<int>>(WatchlistSessionKey) ?? new List<int>();
        var monitoredVehicles = _vehicles.Where(v => watchlist.Contains(v.SideNumber)).ToList();

        return View(monitoredVehicles);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RemoveFromWatchlist(int sideNumber)
    {
        var watchlist = HttpContext.Session.GetObject<List<int>>(WatchlistSessionKey) ?? new List<int>();
        if (watchlist.Remove(sideNumber))
        {
            HttpContext.Session.SetObject(WatchlistSessionKey, watchlist);
            TempData["SuccessMessage"] = $"Pojazd #{sideNumber} został usunięty z monitorowanych.";
        }

        return RedirectToAction(nameof(Watchlist));
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