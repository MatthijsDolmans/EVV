using Evv.Database;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml.Linq;

namespace Evv.Classes
{
    public class Trip
    {
        public double Distance { get; private set; }
        public double Score { get; private set; }
        public int People { get; private set; } = 1;
        public DateTime DateCreated { get; private set;}
        public Vehicle_Modifier Vehicle_Type { get; private set; }
        public string Vehicle { get; set; }

        public Trip(double distance, Vehicle_Modifier vehicle_Modifier, int people, DateTime datecreated, string accountId)
        {
            DatabaseClass database = new DatabaseClass();
            Distance = distance;
            Vehicle_Type = vehicle_Modifier;
            People = people;
            DateCreated = datecreated;
            Score = CalculateScore();
            if (Score != null && Distance != null && DateCreated != null && accountId != null && Distance > 0 && People > 0)
            {
                    database.AddTrip(Score, Distance, DateCreated, accountId, Vehicle_Type.ToString());    
            }
        }
        public Trip(double distance, Vehicle_Modifier vehicle_Modifier, int people, DateTime datecreated, string accountId,string FavoriteName)
        {
            DatabaseClass database = new DatabaseClass();
            Distance = distance;
            Vehicle_Type = vehicle_Modifier;
            People = people;
            DateCreated = datecreated;
            Score = CalculateScore();
            if (Score != null && Distance != null && DateCreated != null && accountId != null && Distance > 0 && People > 0)
            {
                    database.AddTripToFavorite(Score, Distance, DateCreated, accountId, Vehicle_Type.ToString(),FavoriteName);
            }
        }
        public Trip(double distance, string vehicle_Modifier, DateTime datecreated, double score)
        {
            Distance = distance;
            Vehicle = vehicle_Modifier;
            DateCreated = datecreated;
            Score = score;
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

        public DateTime GetDate()
        {
            return DateCreated;
        }
    }
}

// Score = Distance * (VehicleType / 1000 ) / people 
// Afgerond op iets
//createtrip, deletetrip,changetrip,calculatescore

