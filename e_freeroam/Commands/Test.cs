using e_freeroam.Objects;
using e_freeroam.Utilities;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;
using System;
using System.Collections.Generic;

namespace e_freeroam.Commands
{
    class Test : Script
    {
        [Command("test", "~w~ERROR: Command not found.", GreedyArg=true)]
        public void test(Player user, string str="lol")
        {
			ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"{str}");
        }
    }
}
