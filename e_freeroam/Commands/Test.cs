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
        public void test(Player user, string str)
        {
			if(str.Length == 1)
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"{((byte) str[0])}");
				return;
			}
			bool check = ChatUtils.isValidHex(str);
			ChatUtils.sendClientMessage(user, (check) ? ChatUtils.getHexAsColor(str) : ServerData.COLOR_WHITE, $"{str}");
        }
    }
}
