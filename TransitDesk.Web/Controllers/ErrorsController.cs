using Microsoft.AspNetCore.Mvc;
namespace TransitDesk.Web.Controllers;

public class ErrorsController : Controller
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Status(int code)
    {
        ViewBag.StatusCode = code;
        ViewBag.Message = code switch
        {
            404 => "Nie odnaleziono wskazanego zasobu taboru lub zgłoszenia.",
            403 => "Brak uprawnień dostępu do modułu dyspozytorskiego.",
            _ => "Wystąpił nieoczekiwany błąd protokołu HTTP."
        };
        return View();
    }
}