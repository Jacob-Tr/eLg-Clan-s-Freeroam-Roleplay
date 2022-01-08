using GTANetworkAPI;
using System.Collections.Generic;

namespace e_freeroam.Utilities.PlayerUtils
{
    public abstract class PlayerDataInfo
    {
        private static List<PlayerData> playerList = new List<PlayerData>(NAPI.Server.GetMaxPlayers());

        public static void addPlayerData(PlayerData player) {playerList.Insert(player.getPlayer().Value, player);}
        public static PlayerData getPlayerData(Player player) {return playerList[player.Value];}
    }
}
