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
        
        protected override void Load()
        {
            Instance = this;

            U.Events.OnPlayerConnected += ShowMessages;
        }

        private void ShowMessages(UnturnedPlayer player) // dont understand, how i get UnturnedPlayer player
        {
            string permission = PlayerPermission(player);
            if (permission == "none") { return; }

            foreach (Group g in Configuration.Instance.Gropus) // i dont know, how to do it easier (not to initialize every group)
            {
                if (permission == "motd." + g.Name.ToLower())
                {
                    foreach (Message m in g.Messages)
                    {
                        try
                        {
                            LineText lineText = new LineText(m.text, player);
                            string text = lineText.getText();

                            LineColor lineColor = new LineColor(m.color);
                            Color color = new Color(lineColor.get('r'), lineColor.get('g'), lineColor.get('b'));

                            UnturnedChat.Say(player, text, color);
                        }
                        catch(Exception e) // I am not pro in Exceptions, maybe this is not good solution
                        {
                            Logger.LogError("[MOTD plugin] " + e.Message);
                        }
                    }
                    return;
                }
            }
        }

        private string PlayerPermission(UnturnedPlayer player)
        {
            int amount = 0;
            string permission = "invalid";

            foreach(Group g in Configuration.Instance.Gropus)
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
                    Logger.LogWarning("[MOTD plugin] Cant find permission for player " + player.DisplayName + ". Nothing will be shown to  him");
                }
                return "none";
            }

            if (Configuration.Instance.ShowWarnings)
            {
                Logger.LogWarning("[MOTD plugin] Player " + player.DisplayName + " has more than one permission. We will show messages only from latest group to him");
            }
            return permission;
        }
    }
}
