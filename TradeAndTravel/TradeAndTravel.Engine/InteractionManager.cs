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
        protected StringBuilder commandResponse = new StringBuilder();
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

        public string GetInteractionResults()
        {
            return this.commandResponse.ToString();
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
                    commandResponse.AppendLine(money.ToString());
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
                    commandResponse.AppendLine(item.Name);
                    item.UpdateWithInteraction(Contants.InventoryAction);
                }
            }

            if (inventory.Count == 0)
            {
                commandResponse.AppendLine(Contants.EmptyResult);
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
                string itemTypeString = commandWords[2];
                string itemNameString = commandWords[3];
                string itemLocationString = commandWords[4];
                this.HandleItemCreation(itemTypeString, itemNameString, itemLocationString);
            }
            else if (commandWords[1] == Contants.LocationAction)
            {
                string locationTypeString = commandWords[2];
                string locationNameString = commandWords[3];
                this.HandleLocationCreation(locationTypeString, locationNameString);
            }
            else
            {
                string personTypeString = commandWords[1];
                string personNameString = commandWords[2];
                string personLocationString = commandWords[3];
                this.HandlePersonCreation(personTypeString, personNameString, personLocationString);
            }
        }

        protected virtual void HandleLocationCreation(string locationTypeString, string locationName)
        {
            Location location = CreateLocation(locationTypeString, locationName);

            locations.Add(location);
            strayItemsByLocation[location] = new List<Item>();
            peopleByLocation[location] = new List<Person>();
            locationByName[locationName] = location;
        }

        protected virtual void HandlePersonCreation(string personTypeString, string personNameString, string personLocationString)
        {
            var personLocation = locationByName[personLocationString];

            Person person = CreatePerson(personTypeString, personNameString, personLocation);

            personByName[personNameString] = person;
            peopleByLocation[personLocation].Add(person);
            moneyByPerson[person] = Contants.InitialPersonMoney;
        }

        protected virtual void HandleItemCreation(string itemTypeString, string itemNameString, string itemLocationString)
        {
            var itemLocation = locationByName[itemLocationString];

            Item item = null;
            item = CreateItem(itemTypeString, itemNameString, itemLocation, item);

            ownerByItem[item] = null;
            strayItemsByLocation[itemLocation].Add(item);
        }

        protected virtual Item CreateItem(string itemTypeString, string itemNameString, Location itemLocation, Item item)
        {
            switch (itemTypeString)
            {
                case Contants.ArmorCreation:
                    item = new Armor(itemNameString, itemLocation);
                    break;
                default:
                    break;
            }

            return item;
        }

        protected virtual Person CreatePerson(string personTypeString, string personNameString, Location personLocation)
        {
            Person person = null;
            switch (personTypeString)
            {
                case Contants.ShopkeeperCreation:
                    person = new Shopkeeper(personNameString, personLocation);
                    break;
                case Contants.TravelerCreation:
                    person = new Traveler(personNameString, personLocation);
                    break;
                default:
                    break;
            }

            return person;
        }

        protected virtual Location CreateLocation(string locationTypeString, string locationName)
        {
            Location location = null;
            switch (locationTypeString)
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
