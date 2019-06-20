using System.Collections.Generic;
using System.Linq;
using TradeAndTravel.Engine.Models.Items;
using TradeAndTravel.Engine.Models.Locations;

namespace TradeAndTravel.Engine.Models.People
{
    public class Person : WorldObject
    {
        private HashSet<Item> inventoryItems;

        public Person(string name, Location location)
            : base(name)
        {
            this.Location = location;
            this.inventoryItems = new HashSet<Item>();
        }
        
        public Location Location { get; protected set; }
        
        public void AddToInventory(Item item)
        {
            this.inventoryItems.Add(item);
        }

        public void RemoveFromInventory(Item item)
        {
            this.inventoryItems.Remove(item);
        }

        public List<Item> ListInventory()
        {
            var items = this.inventoryItems.ToList();
            return items;
        }
    }
}
