using Evv.Classes;

namespace Evv.Test
{
    public class UnitTest1
    {
        [Fact]
        public void CantHaveNegativeDistance()
        {
            Trip trip = new Trip(-20, Vehicle_Modifier.Motor, 1, DateTime.Now, "1");
            Assert.NotEqual(-20, trip.Distance);
        }

        [Fact]
        public void GetsPeopleFromDB()
        {
            Trip trip = new(2, 10000, "Scooter_electric", DateTime.Now, 76.67);
            Assert.Equal(3, trip.People);
        }
    }
}