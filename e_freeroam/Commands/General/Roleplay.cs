using e_freeroam.Utilities;
using e_freeroam.Utilities.PlayerUtils;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;

namespace e_freeroam.Commands.General
{
    class Roleplay : Script
    {
        [Command("me", GreedyArg=true)]
        public void Me(Player user, string text="NullCMDText")
        {
            string failMsg = null;
			string[] parameters = {"me", "Text", text };

            if(ServerData.commandCheck(user, out failMsg, parameters))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

            string meStr = $"{PlayerDataInfo.getPlayerName(user)} {text}";
            ChatUtils.sendClientMessageToAll(Utilities.ServerUtils.ServerData.COLOR_LIGHT_PINK, meStr);
        }

        [Command("do", GreedyArg=true)]
        public void Do(Player user, string text="NullCMDText")
        {
            string failMsg = null;
			string[] parameters = {"do", "Text", text };

            if(ServerData.commandCheck(user, out failMsg, parameters))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

            string meStr = $"* {text} ({PlayerDataInfo.getPlayerName(user)})";
            ChatUtils.sendClientMessageToAll(Utilities.ServerUtils.ServerData.COLOR_LIGHT_PINK, meStr);
        }
    }
}
