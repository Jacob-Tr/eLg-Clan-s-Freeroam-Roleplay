using e_freeroam.Utilities.PlayerUtils;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;

namespace e_freeroam.Objects
{
    public class Checkpoint2
    {
        private CPType type;
        private GTANetworkAPI.Checkpoint point = null;
        private int id;

        public Checkpoint2(Vector3 location, CPType cpType=CPType.NULL)
        {
            this.type = cpType;
            this.point = NAPI.Checkpoint.CreateCheckpoint(CheckpointType.CylinderCheckerboard, location, new Vector3(0, 1, 0), 1F, ServerData.COLOR_RED);

            this.id = this.point.Id;
        }

        public int getID() {return this.id;}

        public GTANetworkAPI.Checkpoint getCheckpoint() {return this.point;}

        public void destroyCheckpoint() {ServerData.removeCP(this.id);}
    }
}
