using e_freeroam.Utilities;
using e_freeroam.Utilities.PlayerUtils;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;

namespace e_freeroam.Objects
{
    public class Checkpoint2
    {
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

        public ushort getID() {return this.id;}

        public GTANetworkAPI.Marker getCheckpoint() {return this.point;}
		public GTANetworkAPI.ColShape getColShape() {return this.interact;}

        public void destroyCheckpoint() 
		{
			ServerData.removeCP(this.id);
			this.point = null;
		}

		public bool isPlayerInCP(Player player) {return NAPI.ColShape.IsPointWithinColshape(this.getColShape(), player.Position);}
    }
}
