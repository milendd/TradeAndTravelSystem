using TradeAndTravel.Engine.Models.Locations;

namespace TradeAndTravel.Engine.Interfaces
{
    public interface ITraveler
    {
        void TravelTo(Location location);

        Location Location { get; }
    }
}
