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
            configText = configText.Replace("%servername%", Provider.serverName);
            configText = configText.Replace("%playername%", player.CharacterName);
            configText = configText.Replace("%online%", Provider.Players.Count().ToString() + "/" + Provider.MaxPlayers.ToString());
            configText = configText.Replace("%adminsonline%", Admins());
            configText = configText.Replace("%mode%", Provider.mode.ToString().ToLower());
            configText = configText.Replace("%pvp/pve%", PvPorPvE());
            configText = configText.Replace("%map%", Provider.map);

            _text = configText;
        }

        private string PvPorPvE()
        {
            if (Provider.PvP) { return "PvP"; }
            return "PvE";
        }

        private string Admins()
        {
            string str = "";
            List<SteamPlayer> Players = Provider.Players;

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
