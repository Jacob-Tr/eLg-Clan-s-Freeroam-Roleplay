using e_freeroam.Utilities;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;

namespace e_freeroam.Commands.General
{
    class Utility : Script
    {
        [Command("commands", GreedyArg=false)]
        public void Commands(Player user)
        {
            ChatUtils.sendClientMessage(user, ServerData.COLOR_YELLOW, "Commands");
            ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, "/me, /do");
        }
    }
}
