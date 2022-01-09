using GTANetworkAPI;
using e_freeroam.Utilities.ServerUtils;

namespace e_freeroam.Objects
{
    public class Vehicle2
    {
        private VehicleType type;
        private Vehicle vehicle = null;

        private Vector3 pos = null, spawn = null;
        private float rot = 0.0F, spawnRot = 0.0F;

        private bool engineStatus = false;

        public Vehicle2(Vehicle newVehicle, VehicleType vehType = VehicleType.CMD_VEHICLE)
        {
            this.type = vehType;
            this.vehicle = newVehicle;

            this.spawn = this.vehicle.Position;
            this.spawnRot = this.vehicle.Rotation.Z;
        }

        public void startEngine() 
        {
            NAPI.Vehicle.SetVehicleEngineStatus(this.vehicle, true);
            this.engineStatus = true;
        }
        public void stopEngine() 
        {
            NAPI.Vehicle.SetVehicleEngineStatus(this.vehicle, false);
            this.engineStatus = false;
        }
        public bool getEngineStatus() {return this.engineStatus;}

        public VehicleType getVehicleType() {return this.type;}

        public Vehicle getVehicle() {return this.vehicle;}
        public ushort getID() {return this.vehicle.Id;}

        public void respawnVeh()
        {
            this.vehicle.Repair();
            this.vehicle.Position = spawn;
        }
    }
}
