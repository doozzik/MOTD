using Rocket.Core.Plugins;
using Rocket.Core.Logging;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using Rocket.Unturned.Chat;
using Rocket.API;
using System;

namespace MOTD
{
    public class MOTD : RocketPlugin<MOTDConfiguration>
    {
        private Tps tpsMonitor;

        protected override void Load()
        {
            tpsMonitor = new Tps();
            CheckConfig();

            U.Events.OnPlayerConnected += ShowMessages;
        }

        protected override void Unload()
        {
            tpsMonitor = null;
            U.Events.OnPlayerConnected -= ShowMessages;
        }

        private void ShowMessages(UnturnedPlayer player)
        {
            // Get player permission
            string permission = null;
            foreach (Group group in Configuration.Instance.Groups)
            {
                if (player.HasPermission("motd." + group.Name.ToLower()))
                {
                    permission = group.Name;
                }
            }
            if (permission == null) { return; }

            // Show messages to player
            foreach (Group group in Configuration.Instance.Groups)
            {
                if (permission == group.Name)
                {
                    foreach (Message m in group.Messages)
                    {
                        try
                        {
                            LineText lineText = new LineText(m.text, tpsMonitor, player, Configuration.Instance.ServerOpened);
                            string text = lineText.getText();

                            LineColor lineColor = new LineColor(m.color);
                            UnityEngine.Color color = new UnityEngine.Color(lineColor.get('r'), lineColor.get('g'), lineColor.get('b'));

                            UnturnedChat.Say(player, text, color);
                        }
                        catch(Exception e)
                        {
                            Logger.LogError("[MOTD] Error: Cant show message to player " + player.SteamName + "\n" + e.Message);
                        }
                    }
                    return;
                }
            }
        }

        private void CheckConfig()
        {
            if (Configuration.Instance.Groups.Count == 0)
            {
                Logger.LogError(@"[MOTD] Warning: You have 0 groups in MOTD.configuration.xml");
            }

            foreach (Group g in Configuration.Instance.Groups)
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
