using Evv.Classes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Evv.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Evv.Models;

public class TripViewModel
{
    public int TripId { get; set; }
    [Range(0, int.MaxValue, ErrorMessage = "Please enter more then 0 KM")]
    public double Distance { get; set; }
    [Range(0, int.MaxValue, ErrorMessage = "Please enter 1 person or more.")]
    public int People { get; set; }
    public double score { get; set; }
    public string? displaystate { get; set; }
    [DataType(DataType.Date, ErrorMessage = "Date is required")]
    [DisplayFormat(ApplyFormatInEditMode = true)]
    public DateTime DateCreated { get; set; }
    public Vehicle_Modifier Vehicle_Modifier { get; set; }
    public List<string>? FavoriteNames { get; set; }
    public string? ChosenfavoriteName { get; set; }
    public string? FavoriteName { get; set; }
    public string? view { get; set; }
    public List<Trip>? tripsbyjourney { get; set; }
    public SelectList VehicleModifierSelectList {get        
        {
            IEnumerable<Vehicle_Modifier> vehicleModifiers = Enum.GetValues(typeof(Vehicle_Modifier)).Cast<Vehicle_Modifier>(); // google: how get all enum values in a list c#

            List<SelectListItem> listItems = new List<SelectListItem>();

            foreach (var vehicleModifier in vehicleModifiers)
            {
                listItems.Add(new SelectListItem
                {
                    Text = vehicleModifier.GetDescription(),
                    Value = ((int)vehicleModifier).ToString()
                });
            }

            SelectList list = new SelectList(listItems, nameof(SelectListItem.Value), nameof(SelectListItem.Text)); // google: asp.net mvc enum to select

            return list;
        }
    }
}
