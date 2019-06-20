using TradeAndTravel.Engine.Interfaces;
using TradeAndTravel.Engine.Models.Locations;

namespace TradeAndTravel.Engine.Models.People
{
    public class Merchant : Shopkeeper, ITraveler
    {
        public Merchant(string name, Location location = null)
            : base(name, location)
        {
        }

        public void TravelTo(Location location)
        {
            this.Location = location;
        }
    }
}
