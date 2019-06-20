using System.Collections.Generic;
using System.Text;
using TradeAndTravel.Engine.Interfaces;
using TradeAndTravel.Engine.Models.Items;
using TradeAndTravel.Engine.Models.Locations;
using TradeAndTravel.Engine.Models.People;

namespace TradeAndTravel.Engine
{
    public class InteractionManager
    {
        protected List<string> commandResponse = new List<string>();
        protected HashSet<Location> locations = new HashSet<Location>();
        protected HashSet<Person> people = new HashSet<Person>();

        protected Dictionary<Person, int> moneyByPerson = new Dictionary<Person, int>();
        protected Dictionary<Item, Person> ownerByItem = new Dictionary<Item, Person>();
        protected Dictionary<string, Person> personByName = new Dictionary<string, Person>();
        protected Dictionary<string, Location> locationByName = new Dictionary<string, Location>();
        protected Dictionary<Location, List<Item>> strayItemsByLocation = new Dictionary<Location, List<Item>>();
        protected Dictionary<Location, List<Person>> peopleByLocation = new Dictionary<Location, List<Person>>();

        public void HandleInteraction(string[] commandWords)
        {
            var command = commandWords[0];
            if (command == Contants.CreateCommand)
            {
                this.HandleCreationCommand(commandWords);
            }
            else
            {
                var actor = this.personByName[command];
                this.HandlePersonCommand(commandWords, actor);
            }
        }

        public string[] GetInteractionResults()
        {
            return this.commandResponse.ToArray();
        }

        protected virtual void HandlePersonCommand(string[] commandWords, Person actor)
        {
            switch (commandWords[1])
            {
                case Contants.DropAction:
                    HandleDropInteraction(actor);
                    break;
                case Contants.PickupAction:
                    HandlePickUpInteraction(actor);
                    break;
                case Contants.SellAction:
                    HandleSellInteraction(commandWords, actor);
                    break;
                case Contants.BuyAction:
                    HandleBuyInteraction(commandWords, actor);
                    break;
                case Contants.InventoryAction:
                    HandleListInventoryInteraction(actor);
                    break;
                case Contants.MoneyAction:
                    int money = moneyByPerson[actor];
                    commandResponse.Add(money.ToString());
                    break;
                case Contants.TravelAction:
                    HandleTravelInteraction(commandWords, actor);
                    break;
                default:
                    break;
            }
        }

        private void HandleTravelInteraction(string[] commandWords, Person actor)
        {
            var traveler = actor as ITraveler;
            if (traveler != null)
            {
                var targetLocation = this.locationByName[commandWords[2]];
                peopleByLocation[traveler.Location].Remove(actor);
                traveler.TravelTo(targetLocation);
                peopleByLocation[traveler.Location].Add(actor);

                var inventory = actor.ListInventory();
                foreach (var item in inventory)
                {
                    item.UpdateWithInteraction(Contants.TravelAction);
                }
            }
        }

        private void HandleListInventoryInteraction(Person actor)
        {
            var inventory = actor.ListInventory();
            foreach (var item in inventory)
            {
                if (ownerByItem[item] == actor)
                {
                    commandResponse.Add(item.Name);
                    item.UpdateWithInteraction(Contants.InventoryAction);
                }
            }

            if (inventory.Count == 0)
            {
                commandResponse.Add(Contants.EmptyResult);
            }
        }

        private void HandlePickUpInteraction(Person actor)
        {
            var inventory = strayItemsByLocation[actor.Location];
            foreach (var item in inventory)
            {
                this.AddToPerson(actor, item);
                item.UpdateWithInteraction(Contants.PickupAction);
            }

            strayItemsByLocation[actor.Location].Clear();
        }

        private void HandleDropInteraction(Person actor)
        {
            var inventory = actor.ListInventory();
            foreach (var item in inventory)
            {
                if (ownerByItem[item] == actor)
                {
                    strayItemsByLocation[actor.Location].Add(item);
                    this.RemoveFromPerson(actor, item);

                    item.UpdateWithInteraction(Contants.DropAction);
                }
            }
        }

        private void HandleBuyInteraction(string[] commandWords, Person actor)
        {
            Item saleItem = null;
            string saleItemName = commandWords[2];

            var buyer = personByName[commandWords[3]] as Shopkeeper;
            if (buyer != null && peopleByLocation[actor.Location].Contains(buyer))
            {
                var inventory = buyer.ListInventory();
                foreach (var item in inventory)
                {
                    if (ownerByItem[item] == buyer && saleItemName == item.Name)
                    {
                        saleItem = item;
                    }
                }

                var price = buyer.CalculateSellingPrice(saleItem);
                moneyByPerson[buyer] += price;
                moneyByPerson[actor] -= price;

                this.RemoveFromPerson(buyer, saleItem);
                this.AddToPerson(actor, saleItem);

                saleItem.UpdateWithInteraction(Contants.BuyAction);
            }
        }

        private void HandleSellInteraction(string[] commandWords, Person actor)
        {
            Item saleItem = null;
            string saleItemName = commandWords[2];

            var inventory = actor.ListInventory();
            foreach (var item in inventory)
            {
                if (ownerByItem[item] == actor && saleItemName == item.Name)
                {
                    saleItem = item;
                }
            }

            var buyer = personByName[commandWords[3]] as Shopkeeper;
            if (buyer != null && peopleByLocation[actor.Location].Contains(buyer))
            {
                var price = buyer.CalculateBuyingPrice(saleItem);
                moneyByPerson[buyer] -= price;
                moneyByPerson[actor] += price;

                this.RemoveFromPerson(actor, saleItem);
                this.AddToPerson(buyer, saleItem);

                saleItem.UpdateWithInteraction(Contants.SellAction);
            }
        }

        protected void AddToPerson(Person actor, Item item)
        {
            actor.AddToInventory(item);
            ownerByItem[item] = actor;
        }

        protected void RemoveFromPerson(Person actor, Item item)
        {
            actor.RemoveFromInventory(item);
            ownerByItem[item] = null;
        }

        protected void HandleCreationCommand(string[] commandWords)
        {
            if (commandWords[1] == Contants.ItemAction)
            {
                var itemType = commandWords[2];
                var itemName = commandWords[3];
                var itemLocation = commandWords[4];
                this.HandleItemCreation(itemType, itemName, itemLocation);
            }
            else if (commandWords[1] == Contants.LocationAction)
            {
                var locationType = commandWords[2];
                var locationName = commandWords[3];
                this.HandleLocationCreation(locationType, locationName);
            }
            else
            {
                var personType = commandWords[1];
                var personName = commandWords[2];
                var personLocation = commandWords[3];
                this.HandlePersonCreation(personType, personName, personLocation);
            }
        }

        protected virtual void HandleLocationCreation(string locationType, string locationName)
        {
            Location location = CreateLocation(locationType, locationName);

            locations.Add(location);
            strayItemsByLocation[location] = new List<Item>();
            peopleByLocation[location] = new List<Person>();
            locationByName[locationName] = location;
        }

        protected virtual void HandlePersonCreation(string personType, string personName, string personLocationText)
        {
            var personLocation = locationByName[personLocationText];

            Person person = CreatePerson(personType, personName, personLocation);

            personByName[personName] = person;
            peopleByLocation[personLocation].Add(person);
            moneyByPerson[person] = Contants.InitialPersonMoney;
        }

        protected virtual void HandleItemCreation(string itemType, string itemName, string itemLocationText)
        {
            var itemLocation = locationByName[itemLocationText];

            Item item = null;
            item = CreateItem(itemType, itemName, itemLocation, item);

            ownerByItem[item] = null;
            strayItemsByLocation[itemLocation].Add(item);
        }

        protected virtual Item CreateItem(string itemType, string itemName, Location itemLocation, Item item)
        {
            switch (itemType)
            {
                case Contants.ArmorCreation:
                    item = new Armor(itemName, itemLocation);
                    break;
                default:
                    break;
            }

            return item;
        }

        protected virtual Person CreatePerson(string personType, string personName, Location personLocation)
        {
            Person person = null;
            switch (personType)
            {
                case Contants.ShopkeeperCreation:
                    person = new Shopkeeper(personName, personLocation);
                    break;
                case Contants.TravelerCreation:
                    person = new Traveler(personName, personLocation);
                    break;
                default:
                    break;
            }

            return person;
        }

        protected virtual Location CreateLocation(string locationType, string locationName)
        {
            Location location = null;
            switch (locationType)
            {
                case Contants.TownCreation:
                    location = new Town(locationName);
                    break;
                default:
                    break;
            }

            return location;
        }
    }
}
