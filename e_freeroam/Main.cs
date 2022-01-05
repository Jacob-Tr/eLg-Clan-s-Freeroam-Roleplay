using System;
using GTANetworkAPI;

namespace e_freeroam
{
    public class Main : Script
    {

        [ServerEvent(Event.ResourceStart)]
        public void OnGamemodeInit()
        {

        }

        [ServerEvent(Event.ResourceStop)]
        public void OnGamemodeExit()
        {

        }

        [ServerEvent(Event.PlayerConnected)]
        public void OnPlayerConnect(Player player)
        {

        }

        [ServerEvent(Event.PlayerDisconnected)]
        public void OnPlayerDisconnect(Player player)
        {

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
        public void OnPlayerEnterVehicleAttempt(Player player, Vehicle vehicle, sbyte seatID)
        {

        }

        [ServerEvent(Event.PlayerEnterVehicle)]
        public void OnPlayerEnterVehicle(Player player, Vehicle vehicle, sbyte seatID)
        {

        }

        [ServerEvent(Event.PlayerExitVehicle)]
        public void OnPlayerExitVehicle(Player player, Vehicle vehicle)
        {

        }

        [ServerEvent(Event.VehicleDamage)]
        public void OnVehicleTakeDamage(Vehicle vehicle, float bodyHealthLoss, float engineHealthLoss)
        {

        }

        [ServerEvent(Event.VehicleDeath)]
        public void OnVehicleDeath(Vehicle vehicle)
        {

        }
    }
}
