using Rocket.API;
using System.Collections.Generic;

namespace MOTD
{
    public class MOTDConfiguration : IRocketPluginConfiguration
    {
        public bool ShowWarnings;

        public List<Group> Gropus;

        public void LoadDefaults()
        {
            ShowWarnings = true;

            Gropus = new List<Group>
            {
                new Group("Default", new List<Message>
                {
                    new Message("Welcome to %servername%!", "yellow"),
                    new Message("There are %online% players online", "yellow"),
                    new Message("Enjoy and have fun!", "yellow"),
                }),

                new Group("Premium", new List<Message>
                {
                    new Message("Welcome back, %playername%", "yellow"),
                    new Message("There are %online% players online", "yellow"),
                    new Message("We are glad that you choose this server", "yellow"),
                    new Message("Call a friends and play together!", "yellow"),
                }),

                new Group("Admin", new List<Message>
                {
                    new Message("Current TPS: %tps%", "yellow"),
                    new Message("Players online: %online%", "yellow"),
                    new Message("Administrators online: %adminsonline%", "yellow"),
                }),
            };
        }
    }
}
