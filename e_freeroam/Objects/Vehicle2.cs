using GTANetworkAPI;

namespace e_freeroam.Objects
{
    class Vehicle2
    {
        bool serverVeh = false;
        Vehicle thisVeh = null;

        Vector3 pos = null;
        public Vehicle2(Vehicle vehicle, bool isServer = false)
        {
            this.serverVeh = isServer;
            this.thisVeh = vehicle;

            this.pos = vehicle.Position;
        }

        public bool isServerVehicle() {return this.serverVeh;}

        public void respawnVeh()
        {
            this.thisVeh.Repair();
            this.thisVeh.Position = pos;


        }
    }
}
