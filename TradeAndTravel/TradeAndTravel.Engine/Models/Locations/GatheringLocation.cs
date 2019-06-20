using TradeAndTravel.Engine.Enums;
using TradeAndTravel.Engine.Interfaces;
using TradeAndTravel.Engine.Models.Items;

namespace TradeAndTravel.Engine.Models.Locations
{
    public abstract class GatheringLocation : Location, IGatheringLocation
    {
        public GatheringLocation(string name, LocationType locType, ItemType gatheredItemType, ItemType requiredItemType) 
            : base(name, locType)
        {
            this.GatheredType = gatheredItemType;
            this.RequiredItem = requiredItemType;
        }

        public ItemType GatheredType { get; protected set; }

        public ItemType RequiredItem { get; protected set; }

        public abstract Item ProduceItem(string itemName);
    }
}
