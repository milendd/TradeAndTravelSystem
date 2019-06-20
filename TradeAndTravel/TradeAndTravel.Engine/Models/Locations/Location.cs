using System;
using TradeAndTravel.Engine.Enums;

namespace TradeAndTravel.Engine.Models.Locations
{
    public abstract class Location : WorldObject
    {
        public LocationType LocationType { get; private set; }

        public Location(string name, string type)
            : base(name)
        {
            this.LocationType = (LocationType)Enum.Parse(typeof(LocationType), type);
        }

        public Location(string name, LocationType type)
            : base(name)
        {
            this.LocationType = type;
        }
    }
}
