using e_freeroam.Objects;
using e_freeroam.Utilities;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace e_freeroam.Server_Properties
{
	public class Store
	{
		private static ushort maxStores = 250, storeCount = 0;
		private ushort id = ushort.MaxValue;

		private static FileHandler storeHandler = new FileHandler(FileTypes.STORE, $"{ServerData.getDefaultServerDir()}ServerData", "Stores");

		private Vector3 pos = null;
		private uint world = uint.MaxValue;

		Checkpoint2 storeCP = null;

		public Store(float x, float y, float z, uint vWorld, ushort newID=ushort.MaxValue)
		{
			this.pos = new Vector3(x, y, z);
			this.world = vWorld;

			if(newID == ushort.MaxValue) this.id = (ushort) (getStoreCount() + 1);
			else this.id = newID;
			if(this.id >= Store.getMaxStores()) return;
		}

		// Static store properties

		public static ushort getMaxStores() {return maxStores;}

		public static FileHandler getStoreHandler() {return storeHandler;}

		public static void updateStoreCount(ushort value) {storeCount = value;}
		public static ushort getStoreCount() {return storeCount;}

		// General store information

		public void updateStoreID(ushort value) {this.id = value;}
		public ushort getStoreID() {return this.id;}

		public void updateStoreCP()
		{
			if(this.storeCP != null) ServerData.removeCP(this.storeCP.getID());
			this.storeCP = null;

			this.storeCP = ServerData.addCP(this.pos, this.world, CPType.STORE);
		}
		public Checkpoint2 getStoreCP() {return this.storeCP;}

		public void updateStoreWorld(uint value) 
		{
			this.world = value;
			this.updateStoreCP();
		}
		public uint getStoreWorld() {return this.world;}

		public void updateStorePos(Vector3 value) 
		{
			this.pos = value;
			this.updateStoreCP();
		}
		public Vector3 getStorePos() {return this.pos;}
	}
}
