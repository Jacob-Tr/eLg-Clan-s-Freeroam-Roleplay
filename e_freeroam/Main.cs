using System;
using System.IO;
using GTANetworkAPI;
using e_freeroam.Utilities.ServerUtils;
using e_freeroam.Objects;
using e_freeroam.Utilities;
using e_freeroam.Utilities.PlayerUtils;
using System.Globalization;
using System.Threading;

namespace e_freeroam
{
    public class Main : Script
    {
        public Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        }

        [ServerEvent(Event.ResourceStart)]
        public void OnGamemodeInit()
        {
            ServerData.loadServerData();
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
            PlayerData data = new PlayerData(player);
            PlayerDataInfo.addPlayerData(data);

            uint adminLevel = data.getPlayerAdminLevel();

            if(data.isPlayerRegistered()) ChatUtils.sendClientMessage(player, ServerData.COLOR_WHITE, $"Welcome back {player.Name}. Login to your account to play!");
            else ChatUtils.sendClientMessage(player, ServerData.COLOR_WHITE, $"Account isn't registered. /Register to play.");
        }

        [ServerEvent(Event.PlayerDisconnected)]
        public void OnPlayerDisconnect(Player player, DisconnectionType type, string reason)
        {
            PlayerData data = PlayerDataInfo.getPlayerData(player);

            Vector3 pos = player.Position;
            float angle = player.Heading;

            data.updatePlayerHealth((sbyte) player.Health);
            data.updatePlayerArmor((sbyte) player.Armor);
            data.updatePlayerPos(pos, angle);

            data.getPlayerFileHandler().saveFile();

            PlayerDataInfo.removePlayerData(data);
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
        }
    }
}
