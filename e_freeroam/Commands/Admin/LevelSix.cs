using e_freeroam.Utilities;
using e_freeroam.Utilities.PlayerUtils;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;

namespace e_freeroam.Commands.Admin
{
    class LevelSix : Script
    {
		[Command("setadmin", GreedyArg=true)]
		public void setAdmin(Player user, string id="NullCMDStr", string levelStr="NullCMDStr")
		{
            string failMsg = null;
			string[] parameters = {"setadmin", "Player ID", "Level", id, levelStr};

            if(ServerData.commandCheck(user, out failMsg, 6, parameters, ((byte) parameters.Length)))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

			short playerid = ServerData.getTargetPlayerID(id);
			if(playerid == -1)
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid player ID.");
				return;
			}

			sbyte level = (NumberUtils.isNumeric(levelStr, levelStr.Length)) ? NumberUtils.parseByte(levelStr, levelStr.Length) : ((sbyte) 0);
			if(level < 1 || level > 7)
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid level. (1-7)");
				return;
			}

			PlayerData targetData = PlayerDataInfo.getPlayerDataFromID(playerid);
			targetData.updatePlayerAdminLevel((byte) level);
		}
    }
}
