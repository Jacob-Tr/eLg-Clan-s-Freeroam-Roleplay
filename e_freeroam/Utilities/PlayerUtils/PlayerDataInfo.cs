using GTANetworkAPI;
using System.Collections.Generic;

namespace e_freeroam.Utilities.PlayerUtils
{
    public abstract class PlayerDataInfo
    {
        private static List<PlayerData> playerList = new List<PlayerData>(NAPI.Server.GetMaxPlayers());

        public static void addPlayerData(PlayerData player) 
        {
            foreach(PlayerData data in playerList)
            {
                if(data.getPlayer() == player.getPlayer())
                {
                    playerList.Remove(data);
                    break;
                }
            }
            playerList.Insert(player.getPlayer().Value, player);
        }
        public static PlayerData getPlayerData(Player player) {return playerList[player.Value];}
		public static PlayerData getPlayerDataFromID(short playerid) {return playerList[playerid];}

        public static void removePlayerData(PlayerData player) {if(playerList.Contains(player)) playerList.Remove(player);}
    }
}
