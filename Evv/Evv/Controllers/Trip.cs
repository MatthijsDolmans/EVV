using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml.Linq;

namespace Evv.Controllers
{
    public enum Vehicle_Modifier
    {
        // Scooter
        Scooter_petrol = 56,
        Scooter_electric = 23,

        // Cars
        Car_electric = 85,
        Car_hydrogen = 112,
        Car_hybrid = 128,
        Car_gas = 152,
        Car_petrol = 204,
        Car_diesel = 180,

        // Spare
        PT = 20,

        Walk_Bike = 0,
        Bike_electric = 6,

        Airplane = 182,

        Motor = 113,

    }

    public class Trip
    {
        private double Distance { get; set; }
        private double Score { get; set; }
        private int People { get; set; }
        private string DateCreated { get; set; }
        private Vehicle_Modifier Vehicle_Type { get; set; }


        public Trip(double distance, Vehicle_Modifier vehicle_Modifier, int people)
        {
            Distance = distance;
            Vehicle_Type = vehicle_Modifier;
            People = people;
            DateTime dateTime = DateTime.Now;
            DateCreated = dateTime.ToShortDateString();
            Score = CalculateScore();
        }
        public override string ToString()
        {
            return $"distance: {Distance} \n score: {Score} people: {People} vehicle: {Vehicle_Type} Date: {DateCreated}";
        }

        public double CalculateScore()
        {
            Score = Distance * ((double)Vehicle_Type / 1000) / People;
            return Score;
        }
        public double GetDistance()
        {
            return Distance;
        }

        public double GetScore()
        {
            return Score;
        }


    }

}

// Score = Distance * (VehicleType / 1000 ) / people 
// Afgerond op iets
//createtrip, deletetrip,changetrip,calculatescore

