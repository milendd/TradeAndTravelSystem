using TradeAndTravel.Engine.Enums;
using TradeAndTravel.Engine.Models.Items;

namespace TradeAndTravel.Engine.Interfaces
{
    public interface IGatheringLocation
    {
        ItemType GatheredType { get; }

        ItemType RequiredItem { get; }
        
        Item ProduceItem(string name);
    }
}
