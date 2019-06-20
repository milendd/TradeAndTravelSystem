using System;

namespace TradeAndTravel.Engine
{
    public class Engine
    {
        private InteractionManager interactionManager;

        public Engine(InteractionManager interactionManager)
        {
            this.interactionManager = interactionManager;
        }
        
        public void ParseAndDispatch(string command)
        {
            this.interactionManager.HandleInteraction(command.Split(' '));
        }

        public string Start()
        {
            bool endCommandReached = false;
            while (!endCommandReached)
            {
                string command = Console.ReadLine();
                if (command == Contants.EndCommand)
                {
                    endCommandReached = true;
                }
                else
                {
                    this.ParseAndDispatch(command);
                }
            }

            return this.interactionManager.GetInteractionResults();
        }
    }
}
