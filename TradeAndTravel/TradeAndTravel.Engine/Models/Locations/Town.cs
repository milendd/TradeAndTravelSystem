using TradeAndTravel.Engine.Enums;

namespace TradeAndTravel.Engine.Models.Locations
{
    public class Town : Location
    {
        public Town(string name)
            : base(name, LocationType.Town)
        {
        }
    }
}
