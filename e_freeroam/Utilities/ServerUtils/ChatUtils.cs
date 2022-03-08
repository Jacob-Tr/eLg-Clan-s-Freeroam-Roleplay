using e_freeroam.Objects;
using e_freeroam.Utilities.PlayerUtils;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;
using System;

namespace e_freeroam.Utilities
{
    public abstract class ChatUtils
    {
        public static void sendMessageToAdmins(GTANetworkAPI.Color color, string str, bool removeCols=true, bool showLog=true, Player sender=null)
        {
            string output = showLog ? $"Admin Log: {str}" : str;
            ServerData.logToConsole(output);

            foreach(Player player in NAPI.Pools.GetAllPlayers())
            {
                if(player.Equals(sender)) continue;
                if(PlayerDataInfo.getPlayerData(player).getAdminLevel() > 0) sendClientMessage(player, color, output, removeCols, true);
            }
        }

		public static void sendMessageToHDOs(Color color, string str, bool removeCols=true, Player sender=null)
		{
            foreach(Player player in NAPI.Pools.GetAllPlayers())
            {
                if(player.Equals(sender)) continue;
                if(PlayerDataInfo.getPlayerData(player).getSupportLevel() > 0) sendClientMessage(player, color, str, removeCols, true);
            }
		}

		public static void sendMessageToOrgMembers(Organization org, Color color, string str, bool coLeaders=true, bool members=true, bool removeCols=true)
		{
			byte count = org.getOrgMemberCount(), index = 0;
			Player player = null;
			for(byte i = 1; i <= count; i++)
			{
				index = (byte) (i + 1);
				if(coLeaders)
				{
					if(index == 1)
					{
						player = NAPI.Player.GetPlayerFromName(org.getLeaderName());
						sendClientMessage(player, color, str, removeCols, true);
					}
					if(index <= 3)
					{
						player = NAPI.Player.GetPlayerFromName(org.getColeaderName(i));
						sendClientMessage(player, color, str, removeCols, true);
					}
				}
				if(members)
				{
					player = NAPI.Player.GetPlayerFromName(org.getMemberName(i));
					sendClientMessage(player, color, str, removeCols, true);
				}
			}
		}

        public static void sendClientMessage(Player player, GTANetworkAPI.Color color, string text, bool removeCols=false, bool logged=false)
        {
            if(!NAPI.Player.IsPlayerConnected(player) || (!PlayerDataInfo.getPlayerData(player).isPlayerLoggedIn() && logged) || player == null || text == null) return;

			if(removeCols) removeEmbeddedColors(ref text);

            text = colorString(text, getColorAsHex(color));
            player.SendChatMessage(text);
        }

        public static void sendClientMessageToAll(GTANetworkAPI.Color color, string text, bool removeCols=false) {foreach(Player player in NAPI.Pools.GetAllPlayers()) sendClientMessage(player, color, text, removeCols);}

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

		public static bool isValidHex(string hex, Player user=null)
		{
			if(hex.Length != 6 && hex.Length != 8 && hex.Length != 3) return false;
			if(hex.Length == 8 && ((byte) hex[0] != 48 || (byte) hex[1] != 120)) return false; // First and second chars of 8 digit hexidecimal must be 0 and x respectively.

			ushort charVal = 0;
			byte index = (hex.Length == 8) ? ((byte) 2) : ((byte) 0);
			for(byte i = index; i < hex.Length; i++)
			{
				charVal = ((ushort) hex[i]);
				if((charVal < 65 || charVal > 70) && (charVal < 97 || charVal > 102) && (!NumberUtils.isNumber(hex[i]) && charVal != 46)) return false;
			}
			return true;
		}

		public static void removeEmbeddedColors(ref string text)
		{
			string beginText = "";

			for(byte i = 0; i < text.Length; i++)
			{
				if(i >= text.Length - 1) break;
				if(i < (text.Length - 9) && text[i] == '!' && text[(i + 1)] == '{' && text[(i + 2)] == '#')
				{
					int closeIndex = text.IndexOf('}', (i + 1));

					if(closeIndex == (i + 9))
					{
						if(i != 0) beginText = text.Substring(0, i);
						if(closeIndex < (text.Length - 1)) beginText = $"{beginText}{text.Substring(closeIndex + 1)}";
					}
					else continue;

					text = beginText;
					beginText = "";
				}
			}
		}
    
        public static string colorString(string text, string colorHex, string defaultHex=null) {return (defaultHex == null) ? "!{#" + colorHex + "}" +  text : "!{#" + colorHex + "}" + text + "!{#" + defaultHex + "}";}

		public static string strTok(string str, ref byte index, char seperator=' ') // Written by some guy in the SA-MP community (originally in pawn).
		{
			int length = str.Length;
			if(!str.Contains(seperator) || length <= index) return str;

			int offset = index;
			string result = new string(seperator, length);

			while((index < length) && (str[index] != seperator) && ((index - offset) < length)) 
			{
				byte location = (byte) (index - offset);
				result = result.Insert(location, $"{str[index]}");
				index++;
			}

			result = result.Substring(0, result.IndexOf(seperator));
			if((index < length) && (str[index] == seperator)) index++;
			return result;
		}	
	}
}