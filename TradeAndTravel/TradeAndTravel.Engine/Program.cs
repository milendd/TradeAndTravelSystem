using System;

namespace TradeAndTravel.Engine
{
    public class Program
    {
        public static void Main()
        {
            var engine = new Engine(new AdvancedInteractionManager());
            var interactionResults = engine.Start();
            Console.WriteLine(interactionResults);
        }
    }
}
