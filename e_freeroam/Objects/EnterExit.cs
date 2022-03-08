using System;
using e_freeroam.Utilities;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;

namespace e_freeroam.Objects
{
	public class EnterExit
	{
		private static ushort maxEnExs = 500, enExCount = 0;
		private static FileHandler enExHandler = new FileHandler(FileTypes.ENEX, $"{ServerData.getDefaultServerDir()}ServerData", "EnterExits");

		private ushort enExID = 0;
		private Vector3 pos = null;
		private uint world = 0;

		private Marker point = null;
		private EnterExit linkedEnEx = null;
		private ColShape interact = null;

		public EnterExit(Vector3 newPos, uint newWorld, ushort linkedEnExID=ushort.MaxValue, ushort newID=ushort.MaxValue)
		{
			this.pos = newPos;
			this.world = newWorld;

			EnterExit linked = null;
			if(linkedEnExID != ushort.MaxValue) linked = ServerData.getEnEx(linkedEnExID);
			if(linked != null) this.updateLinkedEnEx(linked);

			interact = NAPI.ColShape.CreatCircleColShape(this.pos.X, this.pos.Y, 0.25F, this.world);
			point = NAPI.Marker.CreateMarker(0, this.pos, new Vector3(0, 0, 10), new Vector3(), 1F, ServerData.COLOR_YELLOW, true, this.world);
		}

		// Static EnEx Properties

		public static ushort getMaxEnterExits() {return maxEnExs;}

		public static void updateEnExHandler(FileHandler value) {enExHandler = value;}
		public static FileHandler getEnExHandler() {return enExHandler;}

		public static void updateEnExCount(ushort value) {enExCount = value;}
		public static ushort getEnExCount() {return enExCount;}

		// General EnEx information

		public void updateEnExColshape(ColShape value) {this.interact = value;}
		public ColShape getEnExColshape() {return this.interact;}

		public void updateEnExID(ushort value) {this.enExID = value;}
		public ushort getEnExID() {return this.enExID;}

		public void updateEnExPos(Vector3 newPos=null, uint newWorld=uint.MaxValue) 
		{
			if(newWorld == uint.MaxValue) newWorld = this.world;
			if(newPos == null) newPos = this.pos;

			this.pos = newPos;
			this.world = newWorld;

			interact = NAPI.ColShape.CreatCircleColShape(this.pos.X, this.pos.Y, 0.25F, this.world);
			point = NAPI.Marker.CreateMarker(0, this.pos, new Vector3(0, 0, 10), new Vector3(), 1F, ServerData.COLOR_YELLOW, true, this.world);
		}
		public Vector3 getEnExPos() {return this.pos;}

		public void updateEnExWorld(uint value) {this.world = value;}
		public uint getEnExWorld() {return this.world;}

		public void updateLinkedEnEx(EnterExit value) 
		{
			if(value == null) return;

			this.linkedEnEx = value;
			value.updateLinkedEnEx(this);

			ServerData.logToConsole($"EnterExits {this.enExID} and {value.getEnExID()} now linked.");
		}
		public EnterExit getLinkedEnex() {return this.linkedEnEx;}
	}
}
