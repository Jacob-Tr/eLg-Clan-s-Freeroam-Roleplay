using e_freeroam.Utilities.ServerUtils;
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
            if(!isValidHex(hex)) return new Color(0x000000);
			if(hex.Length == 3)
			{
				string newHex = "";
				for(byte i = 0; i < hex.Length; i++) newHex += $"{hex[i]}{hex[i]}";
				hex = newHex;
			}

            if(hex.Length == 6) return new Color(int.Parse(hex, System.Globalization.NumberStyles.HexNumber));
            else return new Color(Convert.ToInt32(hex, 16));
        }

		public static bool isValidHex(string hex)
		{
			if(hex.Length != 6 && hex.Length != 8 && hex.Length != 3) return false;
			if(hex.Length == 8 && ((((byte) hex[0]) != 48) || (((byte) hex[1]) != 120))) return false; // First and second chars of 8 digit hexidecimal must be 0 and x respectively.

			ushort charVal = 0;
			byte index = (hex.Length == 8) ? ((byte) 2) : ((byte) 0);
			for(byte i = index; i < hex.Length; i++)
			{
				charVal = ((ushort) hex[i]);
				if((charVal < 65 || charVal > 70) && (charVal < 97 || charVal > 102) && (!NumberUtils.isNumber(hex[i]) && charVal != 46)) return false;
			}
			return true;
		}
    
        public static string colorString(string text, string colorHex, string defaultHex=null) {return (defaultHex == null) ? "!{#" + colorHex + "}" +  text : "!{#" + colorHex + "}" + text + "!{#" + defaultHex + "}";}
    }
}
