using TradeAndTravel.Engine.Enums;
using TradeAndTravel.Engine.Models.Locations;

namespace TradeAndTravel.Engine.Models.Items
{
    public class Armor : Item
    {
        private const int GeneralArmorValue = 5;

        public Armor(string name, Location location = null)
            : base(name, GeneralArmorValue, ItemType.Armor, location)
        {
        }
    }
}
