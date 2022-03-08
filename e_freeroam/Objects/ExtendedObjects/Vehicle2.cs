using GTANetworkAPI;
using e_freeroam.Utilities.ServerUtils;
using System;
using e_freeroam.Utilities;

namespace e_freeroam.Objects
{
    public class Vehicle2
    {
		private static ushort  maxVehicles = 1000;

        private VehicleType type;
        private Vehicle vehicle = null;

        private Vector3 spawn = null, spawnRot = null, speed = null;
        private float  fuel = 0F;

        private bool engineStatus = false;

        public Vehicle2(Vehicle newVehicle, VehicleType vehType = VehicleType.CMD_VEHICLE)
        {
            this.type = vehType;
            this.vehicle = newVehicle;

			this.setVehicleFuel(new Random().Next(25, 100));

            this.spawn = this.vehicle.Position;
            this.spawnRot = this.vehicle.Rotation;

			this.speed = new Vector3(0, 0, 0);
        }

		//Static vehicle properties

		public static ushort getMaxVehicles() {return maxVehicles;}

		// General Vehicle Utilities

        public VehicleType getVehicleType() {return this.type;}

        public Vehicle getVehicle() {return this.vehicle;}
        public ushort getID() {return this.vehicle.Id;}

        public void respawnVeh()
        {
			int vehicleid = this.vehicle.Value;
			uint hashKey = this.vehicle.Model;

			ServerData.removeVehicle(vehicleid);
			if(this.getVehicleType() != VehicleType.CMD_VEHICLE) ServerData.addVehicle(hashKey, this.spawn, this.spawnRot, -1, -1, this.type);
        }

		// Engine related

        public void startEngine() 
        {
            NAPI.Vehicle.SetVehicleEngineStatus(this.vehicle, true);
			NAPI.ClientEventThreadSafe.TriggerClientEventForAll("ToggleEngine", this.vehicle.Value, true);

            this.engineStatus = true;
        }
        public void stopEngine() 
        {
            NAPI.Vehicle.SetVehicleEngineStatus(this.vehicle, false);
			NAPI.ClientEventThreadSafe.TriggerClientEventForAll("ToggleEngine", this.vehicle.Value, false);

            this.engineStatus = false;
        }

        public bool getEngineStatus() {return this.engineStatus;}

		// Velocity/Speed

		public void updateVehicleSpeedValue(Vector3 value) {this.speed = value;}
		public Vector3 getVehicleSpeedValue() {return this.speed;}

		public float getVehicleSpeed()
		{
			float x = this.speed.X, y = this.speed.Y, z = this.speed.Z;
			return (float) Math.Round(Math.Sqrt(NumberUtils.floatExp(Math.Abs(x), 2) + NumberUtils.floatExp(Math.Abs(y), 2) + NumberUtils.floatExp(Math.Abs(z), 2)));
		}

		// Fuel

		public void setVehicleFuel(float value) {this.fuel = value;}
		public float getVehicleFuel() {return this.fuel;}

        public bool isAnyoneInVehicle()
        {
            Vehicle veh = this.getVehicle();

            foreach(Player player in NAPI.Pools.GetAllPlayers()) if(player.Vehicle == veh) return true;
            return false;
        }
    }
}
