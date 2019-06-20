using TradeAndTravel.Engine.Enums;
using TradeAndTravel.Engine.Models.Locations;

namespace TradeAndTravel.Engine.Models.Items
{
    public class Iron : Item
    {
        private const int GeneralIronValue = 3;

        public Iron(string name, Location location = null) :
            base(name, GeneralIronValue, ItemType.Iron, location)
        {
        }
    }
}
