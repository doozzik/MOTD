using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using System.Linq;

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
            configText = configText.Replace("%online%", Steam.Players.Count().ToString() + "/" + Steam.MaxPlayers.ToString());
            configText = configText.Replace("%adminsonline%", Admins());
            configText = configText.Replace("%mode%", Steam.mode.ToString().ToLower());
            configText = configText.Replace("%pvp/pve%", PvPorPvE());
            configText = configText.Replace("%map%", Steam.map);

            _text = configText;
        }

        private string PvPorPvE()
        {
            if (Steam.isPvP) { return "PvP"; }
            return "PvE";
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
