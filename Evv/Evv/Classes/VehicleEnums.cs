using System.ComponentModel;

namespace Evv.Classes
{
    public enum Vehicle_Modifier
    {
        // Scooter
        [Description("Scooter - Petrol")]
        Scooter_petrol = 56,
        [Description("Scooter - Electric")]
        Scooter_electric = 23,

        // Cars
        [Description("Car - Electric")]
        Car_electric = 85,
        [Description("Car - Hydrogen")]
        Car_hydrogen = 112,
        [Description("Car - Hybrid")]
        Car_hybrid = 128,
        [Description("Car - Gas")]
        Car_gas = 152,
        [Description("Car - Petrol")]
        Car_petrol = 204,
        [Description("Car - Diesel")]
        Car_diesel = 180,

        // Spare
        [Description("Public transport")]
        Public_transport = 20,
        [Description("Walk/bike")]
        Walk_Bike = 0,
        [Description("Bike - Electric")]
        Bike_electric = 6,
        [Description("Airplane")]
        Airplane = 182,
        [Description("Motor")]
        Motor = 113,
    }
}
