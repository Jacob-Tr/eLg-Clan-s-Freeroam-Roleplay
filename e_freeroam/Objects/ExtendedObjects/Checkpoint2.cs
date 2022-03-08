using e_freeroam.Utilities;
using e_freeroam.Utilities.PlayerUtils;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;

namespace e_freeroam.Objects
{
    public class Checkpoint2
    {
		private static ushort maxCPs = 1000;
		private static ushort cpCount = 0;

        private CPType type;
        private GTANetworkAPI.Marker point = null;
		private GTANetworkAPI.ColShape interact = null;
        private ushort id;

        public Checkpoint2(Vector3 location, uint dimension=0, CPType cpType=CPType.NULL)
        {
			location.Z -= 1F;

            this.type = cpType;
			this.interact = NAPI.ColShape.CreatCircleColShape(location.X, location.Y, 0.75F, dimension);
            this.point = NAPI.Marker.CreateMarker(1, location, new Vector3(0, 0, 10), new Vector3(), 1.5F, new Color(255, 0, 0, 155), false, dimension);
			NAPI.Entity.SetEntityPosition(this.point.Handle, location);

			NAPI.Entity.SetEntityTransparency(this.point.Handle, 155);
            this.id = this.point.Id;
        }

		// Static checkpoint properties

		public static ushort getMaxCheckpoints() {return maxCPs;}
		public static void updateMaxCheckpoints(ushort value) {maxCPs = value;}

		public static ushort getCPCount() {return cpCount;}
		public static void updateCPCount(ushort value) {cpCount = value;}

		// General checkpoint utilities

        public ushort getID() {return this.id;}

        public GTANetworkAPI.Marker getCheckpoint() {return this.point;}
		public GTANetworkAPI.ColShape getColShape() {return this.interact;}

        public void destroyCheckpoint() 
		{
			this.point.Delete();
			this.interact.Delete();

			ServerData.removeCP(this.id);
		}

		public bool isPlayerInCP(Player player) {return NAPI.ColShape.IsPointWithinColshape(this.getColShape(), player.Position);}

		public object getCheckpointOwner()
		{
			if(interact != null) return ServerData.getColShapeOwner(interact);
			foreach(Organization org in ServerData.getOrgPool()) if(org.getCheckpoint2() == this) return org;

			return null;
		}
    }
}
