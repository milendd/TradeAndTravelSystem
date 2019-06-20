using System;
using TradeAndTravel.Engine.Enums;
using TradeAndTravel.Engine.Models.Locations;

namespace TradeAndTravel.Engine.Models.Items
{
    public abstract class Item : WorldObject
    {
        public ItemType ItemType { get; private set; }

        public int Value { get; protected set; }

        protected Item(string name, int itemValue, string type, Location location = null)
            : base(name)
        {
            this.Value = itemValue;
            this.ItemType = (ItemType)Enum.Parse(typeof(ItemType), type);
        }

        protected Item(string name, int itemValue, ItemType type, Location location = null)
            : base(name)
        {
            this.Value = itemValue;
            this.ItemType = type;
        }

        public virtual void UpdateWithInteraction(string interaction)
        {
        }
    }
}
