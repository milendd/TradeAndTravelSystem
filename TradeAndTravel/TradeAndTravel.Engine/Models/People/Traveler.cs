using TradeAndTravel.Engine.Interfaces;
using TradeAndTravel.Engine.Models.Locations;

namespace TradeAndTravel.Engine.Models.People
{
    public class Traveler : Person, ITraveler
    {
        public Traveler(string name, Location location)
            : base(name, location)
        {
        }

        public virtual void TravelTo(Location location)
        {
            this.Location = location;
        }
    }
}
