using Microsoft.AspNetCore.Mvc;
using TransitDesk.Core.Enums;
using TransitDesk.Web.ViewModels;

namespace TransitDesk.Web.Controllers;

public class IncidentsController : Controller
{
    [HttpGet]
    public IActionResult VerifyVehicleAvailable(int vehicleSideNumber)
    {
        bool isValid = vehicleSideNumber >= 1000 && vehicleSideNumber <= 9999 && vehicleSideNumber != 1000;

        if (!isValid)
        {
            return Json($"Pojazd #{vehicleSideNumber} nie występuje w czynnym rejestrze floty.");
        }

        return Json(true);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new IncidentReportViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(IncidentReportViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        TempData["SuccessMessage"] = $"Zgłoszenie dla linii {model.LineNumber} (pojazd #{model.VehicleSideNumber}) zostało pomyślnie zarejestrowane.";
        return RedirectToAction(nameof(Success));
    }

    [HttpGet]
    public IActionResult Success()
    {
        return View();
    }
}