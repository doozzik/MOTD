using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MOTD
{
    class LineText
    {
        private string _text;

        public string getText()
        {
            return _text;
        }
         
        public LineText(string configText, UnturnedPlayer player)
        {
            configText = configText.Replace("%servername%", Steam.serverName);
            configText = configText.Replace("%playername%", player.CharacterName);
            configText = configText.Replace("%adminsonline%", Admins());
            configText = configText.Replace("%online%", Steam.Players.Count().ToString() + "/" + Steam.MaxPlayers.ToString());

            _text = configText;
        }

        private string Admins()
        {
            string str = "";
            List<SteamPlayer> Players = Steam.Players;

            foreach(SteamPlayer p in Players)
            {
                if (p.IsAdmin)
                {
                    if(str != "") { str += ", "; }
                    str += p.player.name;
                }
            }

            return str;
        }
    }
}
