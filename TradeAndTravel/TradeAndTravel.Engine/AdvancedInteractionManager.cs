using System.Linq;
using TradeAndTravel.Engine.Enums;
using TradeAndTravel.Engine.Interfaces;
using TradeAndTravel.Engine.Models.Items;
using TradeAndTravel.Engine.Models.Locations;
using TradeAndTravel.Engine.Models.People;

namespace TradeAndTravel.Engine
{
    public class AdvancedInteractionManager : InteractionManager
    {
        protected override Person CreatePerson(string personType, string personName, Location personLocation)
        {
            Person p = null;
            switch (personType)
            {
                case Contants.MerchantCreation:
                    p = new Merchant(personName, personLocation);
                    break;
                default:
                    p = base.CreatePerson(personType, personName, personLocation);
                    break;
            }

            return p;
        }

        protected override Location CreateLocation(string locationType, string locationName)
        {
            switch (locationType)
            {
                case Contants.ForestCreation:
                    return new Forest(locationName);
                case Contants.MineCreation:
                    return new Mine(locationName);
                default:
                    return base.CreateLocation(locationType, locationName);
            }
        }

        protected override Item CreateItem(string itemType, string itemName, Location itemLocation, Item item)
        {
            switch (itemType)
            {
                case Contants.WeaponCreation:
                    item = new Weapon(itemName, itemLocation);
                    break;
                case Contants.WoodCreation:
                    item = new Wood(itemName, itemLocation);
                    break;
                case Contants.IronCreation:
                    item = new Iron(itemName, itemLocation);
                    break;
                default:
                    return base.CreateItem(itemType, itemName, itemLocation, item);
            }

            return item;
        }

        protected override void HandlePersonCommand(string[] commandWords, Person actor)
        {
            switch (commandWords[1])
            {
                case Contants.GatherAction:
                    this.HandleGatherInteraction(actor, commandWords[2]);
                    break;
                case Contants.CraftAction:
                    this.HandleCraftInteraction(actor, commandWords[2], commandWords[3]);
                    break;
                default:
                    base.HandlePersonCommand(commandWords, actor);
                    break;
            }
        }

        private void HandleCraftInteraction(Person actor, string craftedItemType, string craftedItemName)
        {
            switch (craftedItemType)
            {
                case Contants.ArmorCreation:
                    this.CraftArmor(actor, craftedItemName);
                    break;
                case Contants.WeaponCreation:
                    this.CraftWeapon(actor, craftedItemName);
                    break;
                default:
                    break;
            }
        }

        private void CraftWeapon(Person actor, string craftedItemName)
        {
            var actorInventory = actor.ListInventory();

            var containsIron = actorInventory.Any(item => item.ItemType == ItemType.Iron);
            var containsWood = actorInventory.Any(item => item.ItemType == ItemType.Wood);
            if (containsIron && containsWood)
            {
                var weapon = new Weapon(craftedItemName);
                this.AddToPerson(actor, weapon);
            }
        }

        private void CraftArmor(Person actor, string craftedItemName)
        {
            var actorInventory = actor.ListInventory();

            var containsIron = actorInventory.Any(item => item.ItemType == ItemType.Iron);
            if (containsIron)
            {
                var armor = new Armor(craftedItemName);
                this.AddToPerson(actor, armor);
            }
        }

        private void HandleGatherInteraction(Person actor, string gatheredItemName)
        {
            var gatheringLocation = actor.Location as IGatheringLocation;
            if (gatheringLocation != null)
            {
                var actorInventory = actor.ListInventory();

                var searchedItem = gatheringLocation.RequiredItem;
                var hasRequiredItem = actorInventory.Any(item => item.ItemType == searchedItem);
                if (hasRequiredItem)
                {
                    var producedItem = gatheringLocation.ProduceItem(gatheredItemName);
                    this.AddToPerson(actor, producedItem);
                }
            }
        }
    }
}
