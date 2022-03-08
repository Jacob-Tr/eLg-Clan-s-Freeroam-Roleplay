using e_freeroam.Utilities;
using e_freeroam.Utilities.PlayerUtils;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;

namespace e_freeroam.Commands.Support_Team
{
	class LevelOne : Script
	{
		[Command("hcmds", GreedyArg=false)]
		public void HCMDs(Player user)
		{
			string failMsg = null;

			if(ServerData.commandCheck(user, out failMsg))
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
				return;
			}

			ChatUtils.sendClientMessage(user, ServerData.COLOR_SUPPORT, "Helpdesk Commands");
			ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, "/hc");
		}

        [Command("hc", GreedyArg=false)]
        public void hc(Player user, string text="NullCMDStr")
        {
            string failMsg = null;
			string[] parameters = {"hc", "Text", text};

            if(ServerData.commandCheck(user, out failMsg, parameters, supportLevel: 1))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

            uint adminLevel = PlayerDataInfo.getPlayerData(user).getAdminLevel();

            string formatText = $"[Admin Chat] [{PlayerDataInfo.getPlayerName(user)}] {text}";
            ChatUtils.sendMessageToAdmins((adminLevel < 5) ? ServerData.COLOR_ADMIN : ServerData.COLOR_UPPER_ADMIN, formatText, true, false);
            return;
        }
	}
}
