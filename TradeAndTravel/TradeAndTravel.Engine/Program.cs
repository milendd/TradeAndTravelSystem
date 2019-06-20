using System;

namespace TradeAndTravel.Engine
{
    public class Program
    {
        public static void Main()
        {
            var manager = new AdvancedInteractionManager();
            var engine = new Engine(manager);
            var interactionResults = engine.Start();
            var result = string.Join(Environment.NewLine, interactionResults);
            Console.WriteLine(result);
        }
    }
}
