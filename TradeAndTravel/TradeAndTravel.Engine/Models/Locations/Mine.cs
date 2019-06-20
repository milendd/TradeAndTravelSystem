using TradeAndTravel.Engine.Enums;
using TradeAndTravel.Engine.Models.Items;

namespace TradeAndTravel.Engine.Models.Locations
{
    public class Mine : GatheringLocation
    {
        public Mine(string name) :
            base(name, LocationType.Mine, ItemType.Iron, ItemType.Armor)
        {
        }

        public override Item ProduceItem(string itemName)
        {
            return new Iron(itemName, null);
        }
    }
}
