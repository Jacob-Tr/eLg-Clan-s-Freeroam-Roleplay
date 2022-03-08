using e_freeroam.Objects;
using e_freeroam.Utilities;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;
using System.Linq;

namespace e_freeroam.Commands.Admin
{
    class LevelTwo : Script
    { 
		[Command("respawnv", GreedyArg=false)]
		public void RespawnV(Player user, string vehicleidStr="NullCMDStr")
		{
			string failMsg = null;
			string[] parameters = {"respawnv", "Vehicle ID", vehicleidStr};

			if(ServerData.commandCheck(user, out failMsg, parameters, adminLevel: 2))
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
				return;
			}

			if(!NumberUtils.isNumeric(vehicleidStr))
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid vehicle ID.");
				return;
			}

			bool failed = false;
			int vehicleid = NumberUtils.parseInt(vehicleidStr, ref failed);
			Vehicle2 vehicle = ServerData.getVehicleFromID(vehicleid);

			if(vehicle == null)
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid vehicle ID.");
				return;
			}

			vehicle.respawnVeh();

			ChatUtils.sendMessageToAdmins(ServerData.COLOR_ADMIN_NOTES_LOG, $"{user.Name} has respawned vehicle ID {vehicleid}");
		}

		[Command("vrespawn", GreedyArg=false)]
		public void VRespawn(Player user)
		{
			string failMsg = null;

			if(ServerData.commandCheck(user, out failMsg, adminLevel: 2))
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
				return;
			}

			ServerData.respawnAllVehicles();

			ChatUtils.sendClientMessageToAll(ServerData.COLOR_GREEN, "~ All unowned vehicles currently unoccupied have been respawned.");
			ChatUtils.sendMessageToAdmins(ServerData.COLOR_ADMIN_NOTES_LOG, $"{user.Name} has respawned all empty unowned vehicles.");
		}
    }
}
