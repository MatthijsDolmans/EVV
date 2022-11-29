using Evv.Classes;

namespace Evv.Models
{
    public class OverviewViewModel
    {
        //Date, Penalty Points (Score), Distance, Transport
        public List<Trip> trips { get; set; }
        public string Id { get; set; }
        public double totalscore { get; set; }

        public string button { get; set; }

    }
}
