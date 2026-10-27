using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using TransitDesk.Web.Attributes;

namespace TransitDesk.Web.ViewModels;

public class IncidentReportViewModel
{
    [Display(Name = "Numer boczny pojazdu")]
    [Required(ErrorMessage = "Pole 'Numer boczny pojazdu' jest wymagane.")]
    [Remote(action: "VerifyVehicleAvailable", controller: "Incidents", ErrorMessage = "Pojazd o podanym numerze nie istnieje lub jest wycofany.")]
    public int? VehicleSideNumber { get; set; }

    [Display(Name = "Numer linii")]
    [Required(ErrorMessage = "Pole 'Numer linii' jest wymagane.")]
    [RegularExpression(@"^[a-zA-Z0-9]{1,4}$", ErrorMessage = "Dozwolone są wyłącznie litery i cyfry (od 1 do 4 znaków).")]
    public string LineNumber { get; set; } = string.Empty;

    [Display(Name = "Opis zdarzenia")]
    [Required(ErrorMessage = "Pole 'Opis' jest wymagane.")]
    [StringLength(500, MinimumLength = 20, ErrorMessage = "Pole musi zawierać od 20 do 500 znaków.")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Data i czas zdarzenia")]
    [Required(ErrorMessage = "Data i czas zdarzenia są wymagane.")]
    [DataType(DataType.DateTime)]
    [PastOrPresentDate(ErrorMessage = "Data zdarzenia nie może być datą z przyszłości.")]
    public DateTime OccurredAt { get; set; } = DateTime.Now;

    public string? AttachmentPath { get; set; }
}