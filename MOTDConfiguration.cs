using Rocket.API;
using System.Collections.Generic;

namespace MOTD
{
    public class MOTDConfiguration : IRocketPluginConfiguration
    {
        public string ServerOpened;

        public List<Group> Groups;

        public void LoadDefaults()
        {
            ServerOpened = "12-31-2016";

            Groups = new List<Group>
            {
                new Group("Default", new List<Message>
                {
                    new Message("Welcome to %servername%", "yellow"),
                    new Message("This server works for %serverdays% days", "yellow"),
                    new Message("There are %online% players online", "yellow"),
                    new Message("Enjoy and have fun", "yellow")
                }),

                new Group("Vip", new List<Message>
                {
                    new Message("Welcome back, %playername%", "yellow"),
                    new Message("There are %online% players online", "yellow"),
                    new Message("We are glad that you choose this server", "yellow"),
                    new Message("Call a friends and play together!", "yellow")
                }),

                new Group("Admin", new List<Message>
                {
                    new Message("Players online: %online%", "yellow"),
                    new Message("Admins online: %adminsonline%", "yellow"),
                    new Message("Server uptime: %uptime%", "yellow"),
                    new Message("TPS from last 1m, 5m, 15m: %tps%", "yellow")
                })
            };
        }
    }
}
