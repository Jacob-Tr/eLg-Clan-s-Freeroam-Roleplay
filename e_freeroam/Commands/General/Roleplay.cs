using e_freeroam.Utilities;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;

namespace e_freeroam.Commands.General
{
    class Roleplay : Script
    {
        [Command("me", "~r~Usage: /me [Text]", GreedyArg=true)]
        public void Me(Player user, string text)
        {
            string failMsg = null;

            if(ServerData.commandCheck(user, out failMsg))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

            string meStr = $"{user.Name} {text}";
            ChatUtils.sendClientMessageToAll(Utilities.ServerUtils.ServerData.COLOR_LIGHT_PINK, meStr);
        }

        [Command("do", "~r~Usage: /do [Text]", GreedyArg=true)]
        public void Do(Player user, string text)
        {
            string failMsg = null;

            if(ServerData.commandCheck(user, out failMsg))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

            string meStr = $"* {text} ({user.Name})";
            ChatUtils.sendClientMessageToAll(Utilities.ServerUtils.ServerData.COLOR_LIGHT_PINK, meStr);
        }
    }
}
