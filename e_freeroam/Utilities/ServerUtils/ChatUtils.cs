using GTANetworkAPI;
using System;

namespace e_freeroam.Utilities
{
    public abstract class ChatUtils
    {
        public static void sendMessageToAdmins(GTANetworkAPI.Color color, string str, bool showLog=true, Player sender=null)
        {
            string output = showLog ? $"Admin Log: {str}" : str;
            NAPI.Util.ConsoleOutput(output);
            foreach (Player player in NAPI.Pools.GetAllPlayers())
            {
                if(player.Equals(sender)) continue;
                sendClientMessage(player, color, output);
            }
        }

        public static void sendClientMessage(Player player, GTANetworkAPI.Color color, string text)
        {
            if(!NAPI.Player.IsPlayerConnected(player)) return;

            text = colorString(text, getColorAsHex(color));
            player.SendChatMessage(text);
        }

        public static void sendClientMessageToAll(GTANetworkAPI.Color color, string text) {foreach(Player player in NAPI.Pools.GetAllPlayers()) sendClientMessage(player, color, text);}

        public static string getColorAsHex(GTANetworkAPI.Color color) {return $"{color.Blue:X2}{color.Green:X2}{color.Red:X2}";}
        public static Color getHexAsColor(string hex)
        {
            if(hex.Length != 6 && hex.Length != 8) return new Color(0xFFFFFF);
            if(hex.Length == 6) return new Color(int.Parse(hex, System.Globalization.NumberStyles.HexNumber));
            else return new Color(Convert.ToInt32(hex, 16));
        }
    
        public static string colorString(string text, string colorHex, string defaultHex=null) {return (defaultHex == null) ? "!{#" + colorHex + "}" +  text : "!{#" + colorHex + "}" + text + "!{#" + defaultHex + "}";}
    }
}
