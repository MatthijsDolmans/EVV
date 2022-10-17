using Evv.Classes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Evv.Helpers;

namespace Evv.Models;

public class TripViewModel
{
    public double Distance { get; set; }
    public int People { get; set; }
    public double score { get; set; }
    public Vehicle_Modifier Vehicle_Modifier { get; set; }
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
