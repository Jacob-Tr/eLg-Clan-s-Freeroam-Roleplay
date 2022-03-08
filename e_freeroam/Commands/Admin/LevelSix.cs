using e_freeroam.Utilities;
using e_freeroam.Utilities.PlayerUtils;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;

namespace e_freeroam.Commands.Admin
{
    class LevelSix : Script
    {
		[Command("setsmember", GreedyArg=false)]
		public void setSupportMember(Player user, string id="NullCMDStr", string levelStr="NullCMDStr")
		{
            string failMsg = null;
			string[] parameters = {"setadmin", "Player ID", "Level", id, levelStr};

            if(ServerData.commandCheck(user, out failMsg, parameters, adminLevel: 6))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

			bool failed = false;
			ushort playerid = NumberUtils.parseByte(id, ref failed);
			if(PlayerDataInfo.getPlayerFromID(playerid) == null)
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid player ID.");
				return;
			}

			byte level = (NumberUtils.isNumeric(levelStr)) ? NumberUtils.parseByte(levelStr, ref failed) : ((byte) 0);
			if(level < 1 || level > 2)
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid level. (1-2)");
				return;
			}

			PlayerData targetData = PlayerDataInfo.getPlayerDataFromID(playerid);
			targetData.updateSupportLevel((byte) level);

			ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"You have set {PlayerDataInfo.getPlayerName(PlayerDataInfo.getPlayerFromID(playerid))}'s support level it {level}.");
			ChatUtils.sendMessageToAdmins(ServerData.COLOR_ADMIN_NOTES_LOG, $"{PlayerDataInfo.getPlayerName(user)} has set {PlayerDataInfo.getPlayerName(PlayerDataInfo.getPlayerFromID(playerid))}'s support level to {level}.");
			ChatUtils.sendClientMessage(targetData.getPlayer(), ServerData.COLOR_WHITE, $"Your support level has been set to {level}.");
		}

		[Command("setadmin", GreedyArg=false)]
		public void setAdmin(Player user, string id="NullCMDStr", string levelStr="NullCMDStr")
		{
            string failMsg = null;
			string[] parameters = {"setadmin", "Player ID", "Level", id, levelStr};

            if(ServerData.commandCheck(user, out failMsg, parameters, adminLevel: 6))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

			if(!NumberUtils.isNumeric(id))
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid player ID.");
				return;
			}

			bool failed = false;
			ushort playerid = NumberUtils.parseUnsignedShort(id, ref failed);

			byte level = (NumberUtils.isNumeric(levelStr) ? NumberUtils.parseByte(levelStr, ref failed) : ((byte) 0));
			if(level < 1 || level > 7)
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid level. (1-7)");
				return;
			}

			PlayerData targetData = PlayerDataInfo.getPlayerDataFromID(playerid);
			targetData.updateAdminLevel(level);

			ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"You have set {PlayerDataInfo.getPlayerName(PlayerDataInfo.getPlayerFromID(playerid))}'s admin level it {level}.");
			ChatUtils.sendMessageToAdmins(ServerData.COLOR_ADMIN_NOTES_LOG, $"{PlayerDataInfo.getPlayerName(user)} has set {PlayerDataInfo.getPlayerName(PlayerDataInfo.getPlayerFromID(playerid))}'s admin level to {level}.");
			ChatUtils.sendClientMessage(targetData.getPlayer(), ServerData.COLOR_WHITE, $"Your admin level has been set to {level}.");
		}
    }
}
