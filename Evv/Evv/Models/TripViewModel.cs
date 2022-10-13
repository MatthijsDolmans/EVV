using Evv.Classes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Evv.Models
{
    public class TripViewModel
    {
        public double Distance { get; set; }
        public int People { get; set; }
        public double score { get; set; }
        public Vehicle_Modifier Vehicle_Modifier { get; set; }
        public List<SelectListItem> Vehicles { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Scooter_petrol", Text = "Scooter_petrol" },
            new SelectListItem { Value = "Scooter_electric", Text = "Scooter_electric" },
            new SelectListItem { Value = "Car_electric", Text = "Car_electric"  },

        };
    }
}
