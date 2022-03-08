using e_freeroam.Objects;
using e_freeroam.Utilities;
using e_freeroam.Utilities.PlayerUtils;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;

namespace e_freeroam.Commands.Org
{
	class General : Script
	{
		[Command("oskin", GreedyArg=false)]
		public void OSkin(Player user)
		{
			string failMsg = null;

			if(ServerData.commandCheck(user, out failMsg, orgCMD: true))
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, failMsg);
				return;
			}

			PlayerData data = PlayerDataInfo.getPlayerData(user);

			Organization org = data.getPlayerOrg();

			data.setPlayerColor(org.getColor());
			ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, "ACTION: Switched organization skin.");
		}
	}
}
