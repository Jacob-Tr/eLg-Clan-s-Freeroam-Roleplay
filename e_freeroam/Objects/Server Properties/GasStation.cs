using e_freeroam.Objects;
using e_freeroam.Utilities;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;

namespace e_freeroam.Server_Properties
{
	public class GasStation
	{
		private static byte maxGasStations = 100, gasStationCount = 0;
		private static FileHandler gasHandler = new FileHandler(Utilities.ServerUtils.FileTypes.GASSTATION, $"{ServerData.getDefaultServerDir()}ServerData", "GasStations");

		private byte gasStationID = 0;

		private Vector3 pos = null;
		private Checkpoint2 point = null;

		public GasStation(float x, float y, float z, byte id=byte.MaxValue)
		{
			if(id == byte.MaxValue) id = (byte) (gasStationCount + 1);
			this.pos = new Vector3(x, y, z);
		}

		// Static gasstation properties

		public static byte getMaxGasStations() {return maxGasStations;}

		public static void updateGasHandler(FileHandler value) {gasHandler = value;}
		public static FileHandler getGasHandler() {return gasHandler;}

		public static void updateGasStationCount(byte value) {gasStationCount = value;}
		public static byte getGasStationCount() {return gasStationCount;}

		// General gasstation information

		public void updateGasStationID(byte value) {this.gasStationID = value;}
		public byte getGasStationID() {return this.gasStationID;}

		public void updateGasStationCP() 
		{
			if(this.point != null) ServerData.removeCP(this.point.getID());
			this.point = new Checkpoint2(this.pos);
		}
		public Checkpoint2 getGasStationCP() {return this.point;}

		public void updateGasStationPos(Vector3 newPos) {this.pos = newPos;}
		public Vector3 getGasStationPos() {return this.pos;}
	}
}