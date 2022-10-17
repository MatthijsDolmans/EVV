using System.ComponentModel;

namespace Evv.Classes
{
    public enum Vehicle_Modifier
    {
        // Scooter
        [Description("Scooter - Petrol")]
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
        Public_Transport = 20,

        Walk_Bike = 0,
        Bike_electric = 6,

        Airplane = 182,

        Motor = 113,
    }
}
