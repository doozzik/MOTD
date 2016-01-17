using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using Rocket.Unturned.Chat;
using UnityEngine;
using Rocket.API;
using Rocket.Core.Logging;
using System;

namespace MOTD
{
    public class MOTD : RocketPlugin<MOTDConfiguration>
    {
        public static MOTD Instance;
        private Tps tpsMonitor;

        protected override void Load()
        {
            Instance = this;
            tpsMonitor = new Tps();
            CheckConfig();

            U.Events.OnPlayerConnected += ShowMessages;
        }

        protected override void Unload()
        {
            U.Events.OnPlayerConnected -= ShowMessages;
        }

        private void ShowMessages(UnturnedPlayer player)
        {
            string permission = PlayerPermission(player);
            if (permission == "none") { return; }

            foreach (Group g in Configuration.Instance.Groups)
            {
                if (permission == "motd." + g.Name.ToLower())
                {
                    foreach (Message m in g.Messages)
                    {
                        try
                        {
                            LineText lineText = new LineText(m.text, tpsMonitor, player);
                            string text = lineText.getText();

                            LineColor lineColor = new LineColor(m.color);
                            Color color = new Color(lineColor.get('r'), lineColor.get('g'), lineColor.get('b'));

                            UnturnedChat.Say(player, text, color);
                        }
                        catch(Exception e)
                        {
                            Logger.LogError("[MOTD] Error: Cant show message to player " + player.DisplayName);
                            Logger.LogError("[MOTD] Error: " + e.Message);
                        }
                    }
                    return;
                }
            }
        }

        private string PlayerPermission(UnturnedPlayer player)
        {
            int amount = 0;
            string permission = "none";

            foreach(Group g in Configuration.Instance.Groups)
            {
                if (player.HasPermission("motd." + g.Name.ToLower()))
                {
                    amount++;
                    permission = "motd." + g.Name.ToLower();
                }
            }
            
            if (amount == 1)
            {
                return permission;
            }

            if (amount == 0)
            {
                if (Configuration.Instance.ShowWarnings)
                {
                    Logger.LogWarning("[MOTD] Warning: Cant find permission for player " + player.DisplayName);
                    Logger.LogWarning("[MOTD] Warning: Nothing will be shown to  him.");
                }
                return "none";
            }

            if (Configuration.Instance.ShowWarnings)
            {
                Logger.LogWarning("[MOTD] Warning: Player " + player.DisplayName + " has more than one permission.");
                Logger.LogWarning("[MOTD] Warning: We will show messages only from latest group (in MOTD config) to him.");
            }

            return permission;
        }

        private void CheckConfig()
        {
            if (Configuration.Instance.Groups.Count == 0)
            {
                Logger.LogError(@"[MOTD] Error: You have 0 groups in MOTD.configuration.xml.");
                Logger.LogError(@"[MOTD] Error: If you just updated the plugin, you must replace all words ""Gropus"" with ""Groups"" in your MOTD.configuration.xml");
            }

            foreach (Group g in Configuration.Instance.Groups)
            {
                if (Configuration.Instance.ShowWarnings)
                {

                    if (g.Messages.Count == 0)
                    {
                        Logger.LogWarning("[MOTD] Warning: You have 0 messages for group " + g.Name);
                        Logger.LogWarning("[MOTD] Warning: Nothing will be shown to players from that group");
                    }

                    if (g.Messages.Count > 4)
                    {
                        Logger.LogWarning("[MOTD] Warning: You have more than 4 messages for group " + g.Name);
                        Logger.LogWarning("[MOTD] Warning: We will display only latest 4 messages to players");
                    }
                }
            }
        }
    }
}
