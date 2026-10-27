using System.ComponentModel.DataAnnotations;
using TransitDesk.Core.Enums;

namespace TransitDesk.Web.ViewModels;

public class VehicleViewModel
{
    [Display(Name = "Numer boczny")]
    public int SideNumber { get; set; }

    [Display(Name = "Nazwa modelu")]
    public string ModelName { get; set; } = string.Empty;

    [Display(Name = "Nazwa zajezdni")]
    public string DepotName { get; set; } = string.Empty;

    [Display(Name = "Rok produkcji")]
    public int ProductionYear { get; set; }

    [Display(Name = "Pojemność pasażerska")]
    public int Capacity { get; set; }

    [Display(Name = "Niskopodłogowy")]
    public bool IsLowFloor { get; set; }

    [Display(Name = "Status serwisowy")]
    public ServiceStatus ServiceStatus { get; set; }
}