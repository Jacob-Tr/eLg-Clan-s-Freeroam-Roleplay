using System;
using System.IO;
using GTANetworkAPI;
using e_freeroam.Utilities.ServerUtils;
using e_freeroam.Objects;
using e_freeroam.Utilities;
using e_freeroam.Utilities.PlayerUtils;

namespace e_freeroam
{
    public class Main : Script
    {
        [ServerEvent(Event.ResourceStart)]
        public void OnGamemodeInit()
        {
            ServerData.loadVehicles();
        }

        [ServerEvent(Event.ResourceStop)]
        public void OnGamemodeExit()
        {
            ServerData.saveVehicles();
        }

        [ServerEvent(Event.PlayerConnected)]
        public void OnPlayerConnect(Player player)
        {
            bool newAcc = false;
            PlayerData data = new PlayerData(player, out newAcc);
            uint adminLevel = data.getPlayerAdminLevel();

            if(!newAcc)
            {
                if (adminLevel > 0)
                {
                    ChatUtils.sendClientMessage(player, (adminLevel < 5) ? ServerData.COLOR_ADMIN : ServerData.COLOR_UPPER_ADMIN, $"Admin Level - {adminLevel}");
                    ChatUtils.sendMessageToAdmins((adminLevel < 5) ? ServerData.COLOR_ADMIN : ServerData.COLOR_UPPER_ADMIN, $"* {player.Name} (ID{player.Value}) has logged in as admin level {adminLevel}.");
                }

                float angle = 0F;
                Vector3 pos = data.getPlayerPos(out angle);

                player.Position = pos;
                player.Heading = angle;
            }
            else
            {
                ChatUtils.sendClientMessage(player, ServerData.COLOR_YELLOW, $"Account {player.Name} created, you have been logged in automatically!");
                ChatUtils.sendClientMessageToAll(ServerData.COLOR_GREEN, $"~ {player.Name} has registered a new account. Welcome to our server!");
            }
        }

        [ServerEvent(Event.PlayerDisconnected)]
        public void OnPlayerDisconnect(Player player)
        {
            PlayerData data = PlayerDataInfo.getPlayerData(player);

            Vector3 pos = player.Position;
            float angle = player.Heading;

            data.updatePlayerHealth(((int) player.Health));
            data.

            data.getPlayerFileHandler().saveFile();
        }

        [ServerEvent(Event.PlayerSpawn)]
        public void OnPlayerSpawn(Player player)
        {

        }

        [ServerEvent(Event.PlayerDeath)]
        public void OnPlayerDeath(Player player, Player Killer, uint reason)
        {

        }

        [ServerEvent(Event.PlayerEnterVehicleAttempt)]
        public void OnPlayerEnterVehicleAttempt(Player player, Vehicle veh, sbyte seatID)
        {
            Vehicle2 vehicle = ServerData.getVehicleObject(veh);
        }

        [ServerEvent(Event.PlayerEnterVehicle)]
        public void OnPlayerEnterVehicle(Player player, Vehicle veh, sbyte seatID)
        {
            Vehicle2 vehicle = ServerData.getVehicleObject(veh);

            if(seatID == 0)
            {
                if(vehicle.getEngineStatus()) vehicle.startEngine();
                else vehicle.stopEngine();
            }
        }

        [ServerEvent(Event.PlayerExitVehicle)]
        public void OnPlayerExitVehicle(Player player, Vehicle veh)
        {
            Vehicle2 vehicle = ServerData.getVehicleObject(veh);

            if(player.VehicleSeat == 0)
            {
                if(vehicle.getEngineStatus()) vehicle.startEngine();
                else vehicle.stopEngine();
            }
        }

        [ServerEvent(Event.VehicleDamage)]
        public void OnVehicleTakeDamage(Vehicle veh, float bodyHealthLoss, float engineHealthLoss)
        {
            Vehicle2 vehicle = ServerData.getVehicleObject(veh);
        }

        [ServerEvent(Event.VehicleDeath)]
        public void OnVehicleDeath(Vehicle veh)
        {
            Vehicle2 vehicle = ServerData.getVehicleObject(veh);
        }

        [RemoteEvent("OnKeyPress")]
        public void KeyPressEvent(Player player, int key)
        {

        }

        [RemoteEvent("OnKeyRelease")]
        public void KeyReleaseEvent(Player player, int key)
        {
            if(key == ServerData.getKeyValue(KeyRef.N_KEY))
            {
                if(player.IsInVehicle && player.VehicleSeat == 0)
                {
                    Vehicle playerVeh = player.Vehicle;
                    Vehicle2 vehicle = ServerData.getVehicleObject(playerVeh);

                    bool engineStatus = vehicle.getEngineStatus();

                    string output = null;
                    Color color;

                    if(!engineStatus)
                    {
                        vehicle.startEngine();

                        output = "~ Engine turned on.";
                        color = ServerData.COLOR_GREEN;
                    }
                    else
                    {
                        vehicle.stopEngine();

                        output = "~ Engine turned off.";
                        color = ServerData.COLOR_WHITE;
                    }

                    Utilities.ChatUtils.sendClientMessage(player, color, output);
                }

                return;
            }

            if (key == ServerData.getKeyValue(KeyRef.TWO_KEY))
            {
                if (player.IsInVehicle && player.VehicleSeat == 0)
                {
                    Vehicle playerVeh = player.Vehicle;
                    bool engineStatus = NAPI.Vehicle.GetVehicleEngineStatus(playerVeh);

                    Utilities.ChatUtils.sendClientMessage(player, ServerData.COLOR_RED, engineStatus ? "On" : "Off");
                }

                return;
            }
        }
    }
}
