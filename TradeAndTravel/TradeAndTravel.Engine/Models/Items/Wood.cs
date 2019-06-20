using TradeAndTravel.Engine.Enums;
using TradeAndTravel.Engine.Models.Locations;

namespace TradeAndTravel.Engine.Models.Items
{
    public class Wood : Item
    {
        private const int GeneralWoodValue = 2;

        public Wood(string name, Location location = null) :
            base(name, GeneralWoodValue, ItemType.Wood, location)
        {
        }

        public override void UpdateWithInteraction(string interaction)
        {
            // TODO: extract
            if (interaction == "drop")
            {
                if (this.Value > 0)
                {
                    this.Value--;
                }
            }

            base.UpdateWithInteraction(interaction);
        }
    }
}
