using TradeAndTravel.Engine.Enums;
using TradeAndTravel.Engine.Models.Items;

namespace TradeAndTravel.Engine.Models.Locations
{
    public class Forest : GatheringLocation
    {
        public Forest(string name) :
            base(name, LocationType.Forest, ItemType.Wood, ItemType.Weapon)
        {
        }

        public override Item ProduceItem(string itemName)
        {
            return new Wood(itemName, null);
        }
    }
}
