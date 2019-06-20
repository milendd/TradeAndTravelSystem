using TradeAndTravel.Engine.Models.Items;

namespace TradeAndTravel.Engine.Interfaces
{
    public interface IShopkeeper
    {
        int CalculateSellingPrice(Item item);

        int CalculateBuyingPrice(Item item);
    }
}
