using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

        public LineText(string configText, Tps tpsMonitor, UnturnedPlayer player, string date)
        {
            configText = Regex.Replace(configText, "%servername%", Provider.serverName, RegexOptions.IgnoreCase);
            configText = Regex.Replace(configText, "%playername%", player.CharacterName, RegexOptions.IgnoreCase);
            configText = Regex.Replace(configText, "%online%", Provider.clients.Count() + "/" + Provider.maxPlayers, RegexOptions.IgnoreCase);
            configText = Regex.Replace(configText, "%adminsonline%", Admins(), RegexOptions.IgnoreCase);
            configText = Regex.Replace(configText, "%mode%", Provider.mode.ToString(), RegexOptions.IgnoreCase);
            configText = Regex.Replace(configText, "%pvp/pve%", PvPorPvE(), RegexOptions.IgnoreCase);
            configText = Regex.Replace(configText, "%map%", Provider.map, RegexOptions.IgnoreCase);
            configText = Regex.Replace(configText, "%uptime%", UpTime(), RegexOptions.IgnoreCase);
            configText = Regex.Replace(configText, "%tps%", TPS(tpsMonitor), RegexOptions.IgnoreCase);
            configText = Regex.Replace(configText, "%serverdays%", ServerDays(date), RegexOptions.IgnoreCase);

            text = configText;
        }
        
        private string ServerDays(string dateStart)
        {
            DateTime time = Convert.ToDateTime(dateStart);
            TimeSpan result = DateTime.Now.Subtract(time);

            return result.Days.ToString();
        }

        private string PvPorPvE()
        {
            if (Provider.isPvP) { return "PvP"; }
            return "PvE";
        }

        private string Admins()
        {
            string str = "";
            List<SteamPlayer> Players = Provider.clients;

            foreach(SteamPlayer p in Players)
            {
                if (p.isAdmin)
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
