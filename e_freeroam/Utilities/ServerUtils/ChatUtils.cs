using GTANetworkAPI;

namespace e_freeroam.Utilities
{
    public abstract class ChatUtils
    {
        public static void sendMessageToAdmins(string str)
        {
            NAPI.Util.ConsoleOutput(str);
            foreach (Player player in NAPI.Pools.GetAllPlayers()) sendClientMessage(player, Utilities.ServerUtils.ServerData.COLOR_ADMIN_NOTES_LOG, str);
        }

        public static void sendClientMessage(Player player, GTANetworkAPI.Color color, string text)
        {
            text = colorString(text, getColorAsHex(color));
            player.SendChatMessage(text);
        }

        public static void sendClientMessageToAll(GTANetworkAPI.Color color, string text) {foreach(Player player in NAPI.Pools.GetAllPlayers()) sendClientMessage(player, color, text);}

        public static string getColorAsHex(GTANetworkAPI.Color color) {return $"#{color.Blue:X2}{color.Green:X2}{color.Red:X2}";}
    
        public static string colorString(string text, string colorHex) {return "!{" + colorHex + "}" +  text; }
    }
}
