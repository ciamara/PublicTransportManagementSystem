using Microsoft.AspNetCore.Mvc;
using TransitDesk.Web.Filters;
using TransitDesk.Web.ViewModels;

namespace TransitDesk.Web.Controllers;

public class IncidentsController : Controller
{
    private readonly IWebHostEnvironment _environment;

    public IncidentsController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

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
    [AuditLog]
    public async Task<IActionResult> Create(IncidentReportViewModel model, IFormFile? attachmentFile)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (attachmentFile != null && attachmentFile.Length > 0)
        {
            const long maxFileSize = 5 * 1024 * 1024;
            if (attachmentFile.Length > maxFileSize)
            {
                ModelState.AddModelError("attachmentFile", "Rozmiar załącznika nie może przekraczać 5 MB.");
                return View(model);
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
            var extension = Path.GetExtension(attachmentFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("attachmentFile", "Niedozwolony typ pliku. Dozwolone formaty: .jpg, .jpeg, .png, .pdf.");
                return View(model);
            }

            var secureFileName = $"{Guid.NewGuid()}{extension}";
            var storageDirectory = Path.Combine(_environment.ContentRootPath, "App_Data", "Uploads");
            Directory.CreateDirectory(storageDirectory);

            var targetPath = Path.Combine(storageDirectory, secureFileName);

            await using (var fileStream = new FileStream(targetPath, FileMode.Create))
            {
                await attachmentFile.CopyToAsync(fileStream);
            }

            model.AttachmentPath = targetPath;
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