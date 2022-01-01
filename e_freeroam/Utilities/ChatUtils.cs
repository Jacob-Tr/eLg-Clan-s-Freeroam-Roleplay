using GTANetworkAPI;

namespace e_freeroam.Utilities
{
    public abstract class ChatUtils
    {
        public static void SendMessageToAdmins(string str)
        {
            string adminMsg = $"~w~* Admin Log: {str}";
            foreach(Player player in NAPI.Pools.GetAllPlayers())
            {
                player.SendChatMessage(adminMsg);
            }
        }
    }
}
