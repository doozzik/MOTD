using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MOTD
{
    class LineText
    {
        private string text;

        public string getText()
        {
            return text;
        }

        public LineText(string configText, Tps tpsMonitor, UnturnedPlayer player)
        {
            configText = configText.Replace("%servername%", Provider.serverName);
            configText = configText.Replace("%playername%", player.CharacterName);
            configText = configText.Replace("%online%", Provider.Players.Count().ToString() + "/" + Provider.MaxPlayers.ToString());
            configText = configText.Replace("%adminsonline%", Admins());
            configText = configText.Replace("%mode%", Provider.mode.ToString().ToLower());
            configText = configText.Replace("%pvp/pve%", PvPorPvE());
            configText = configText.Replace("%map%", Provider.map);
            configText = configText.Replace("%uptime%", UpTime());
            configText = configText.Replace("%tps%", TPS(tpsMonitor));

            text = configText;
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
                    if (str != "") { str += ", "; }
                    str += p.player.name;
                }
            }

            if (str == "")
            {
                str = "none";
            }

            return str;
        }

        private string UpTime()
        {
            int seconds = (int)Time.realtimeSinceStartup;

            string uptime = "";

            if (seconds >= (60 * 60 * 24))
            {
                uptime = (int)(seconds / (60 * 60 * 24)) + "d ";
            }
            if (seconds >= (60 * 60))
            {
                uptime += (int)((seconds / (60 * 60)) % 24) + "h ";
            }
            if (seconds >= 60)
            {
                uptime += (int)((seconds / 60) % 60) + "m ";
            }
            uptime += (int)(seconds % 60) + "s";

            return uptime;
        }

        private string TPS(Tps tpsMonitor)
        {
            string tps1Min = tpsMonitor.get1Min().ToString("0.0");
            string tps5Min = tpsMonitor.get5Min().ToString("0.0");
            string tps15Min = tpsMonitor.get15Min().ToString("0.0");
            return tps1Min + ", " + tps5Min + ", " + tps15Min;
        }
    }
}
