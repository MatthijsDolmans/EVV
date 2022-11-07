using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml.Linq;

namespace Evv.Classes
{
    public class Trip
    {
        private double Distance { get; set; }
        private double Score { get; set; }
        private int People { get; set; } = 1;
        private string DateCreated { get; set; }
        private Vehicle_Modifier Vehicle_Type { get; set; }


        public Trip(double distance, Vehicle_Modifier vehicle_Modifier, int people)
        {
            Distance = distance;
            Vehicle_Type = vehicle_Modifier;
            People = people;
            DefaultPeople();
            DateTime dateTime = DateTime.Now;
            DateCreated = dateTime.ToShortDateString();
            Score = CalculateScore();
        }

        public double CalculateScore()
        {
            double ScoreTest = Distance * ((double)Vehicle_Type / 1000) / People;
            Score = Math.Round(ScoreTest,2);

            if (Score < 0)
            {
                return Score * -1;
            }
            return Score;
        }
        public double GetDistance()
        {
            return Distance;
        }
        public int DefaultPeople()
        {
            if (People <= 0)
            {
                return People = 1;
            }
            else
            {
                return People;
            }
        }
    }
}

// Score = Distance * (VehicleType / 1000 ) / people 
// Afgerond op iets
//createtrip, deletetrip,changetrip,calculatescore

