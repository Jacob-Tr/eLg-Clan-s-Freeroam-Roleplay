using e_freeroam.Utilities;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;

namespace e_freeroam.Commands.Admin
{
    class LevelFive : Script
    {
		[Command("createorg", "~r~Usage: /createorg [Color] [Name]", GreedyArg=true)]
		public void CreateOrg(Player user, string hex="NullCMDStr", string name="NullCMDStr")
		{
			string failMsg = null;
			string[] parameters = {"createorg", "Color", "Name", hex, name};

            if(ServerData.commandCheck(user, out failMsg, 5, parameters, ((byte) parameters.Length)))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }
		}
    }
}
