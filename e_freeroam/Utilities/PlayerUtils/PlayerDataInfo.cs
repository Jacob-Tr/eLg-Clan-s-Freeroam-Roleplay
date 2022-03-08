using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;
using System.Collections.Generic;
using System.Linq;

namespace e_freeroam.Utilities.PlayerUtils
{
    public abstract class PlayerDataInfo
    {
		private static bool playerListInitialized = false;

        private static List<PlayerData> playerList = new List<PlayerData>(NAPI.Server.GetMaxPlayers());
		private static ushort playersOnline = 0;

		// General player utilities

		public static string getPlayerName(Player player)
		{
			string name = player.Name;
			ChatUtils.removeEmbeddedColors(ref name);
			if(player != null && NAPI.Player.IsPlayerConnected(player)) return name;
			return null;
		}
		public static ushort getPlayerIDFromName(string playerName)
		{
			Player player = NAPI.Player.GetPlayerFromName(playerName);
			if(player == null) return ushort.MaxValue;

			return (ushort) player.Value;
		}

		public static ushort getPlayerID(Player player) {return (ushort) player.Value;}

		public static Player getPlayerFromID(ushort playerid) {return playerList[playerid].getPlayer();}

		[RemoteEvent("SetPlayerDialogActive")]
		public void setPlayerActiveDialog(Player player, ushort dialogid)
		{
			PlayerData data = getPlayerData(player);
			data.setPlayerDialog(dialogid);
		}

		// Player Data

		public static ushort getPlayersOnline() {return playersOnline;}
		private static void updatePlayersOnline(ushort value) {playersOnline = value;}

		public static void removePlayerData(PlayerData player) 
		{
			if(playerList.Contains(player)) 
			{
				playerList.Remove(player);
				updatePlayersOnline((ushort) (getPlayersOnline() - 1));
			}
		}
        public static void addPlayerData(PlayerData player) 
        {
			if(!playerListInitialized)
			{
				ServerData.sendServerFatalError("Attempted to add player data to list before initialization.");
				return;
			}

			updatePlayersOnline((ushort) (getPlayersOnline() + 1));
            playerList.Insert(player.getPlayer().Value, player);
        }

		public static void initializePlayerList() 
		{
			for(ushort i = 0; i < NAPI.Server.GetMaxPlayers(); i++) playerList = ObjectUtils.generateListOfNull((ushort) NAPI.Server.GetMaxPlayers()).Cast<PlayerData>().ToList<PlayerData>();
			playerListInitialized = true;
		}

		public static List<PlayerData> getPlayerDataPool() {return playerList;}

        public static PlayerData getPlayerData(Player player) {return getPlayerDataFromID((ushort) player.Value);}
		public static PlayerData getPlayerDataFromID(ushort playerid) {return playerList[playerid];}

		public static string getPlayerDir(string playerName) {return $"{ServerData.getDefaultServerDir()}PlayerData/{playerName[0]}";}

		public static bool isPlayerOnline(string playerName) {return (NAPI.Player.GetPlayerFromName(playerName) != null);}
    }
}