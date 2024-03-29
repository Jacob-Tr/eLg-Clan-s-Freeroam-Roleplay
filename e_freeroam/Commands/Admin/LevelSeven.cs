﻿using e_freeroam.Objects;
using e_freeroam.Utilities;
using e_freeroam.Utilities.PlayerUtils;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;

namespace e_freeroam.Commands.Admin
{
    class LevelSeven : Script
    {
        [Command("addserverv", GreedyArg=false)]
        public void addServerVehicle(Player user)
        {
            string failMsg = null;

            if(ServerData.commandCheck(user, out failMsg, adminLevel: 4))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

            if (!user.IsInVehicle)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: You must be inside a vehicle.");
                return;
            }
            Vehicle veh = user.Vehicle;
            Vehicle2 vehicle = ServerData.getVehicleObject(veh);

            ServerData.addVehicleToServer(vehicle);
            ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, "* You have successfully added a static server vehicle.");
        }
    }
}
