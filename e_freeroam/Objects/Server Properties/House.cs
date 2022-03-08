using e_freeroam.Objects;
using e_freeroam.Utilities;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;

namespace e_freeroam.Server_Properties
{
	public class House
	{
		private static byte maxProps = 200, propCount = 0;
		private string ownerName = null;
		private byte propID = byte.MaxValue;
		private ushort entrance = ushort.MaxValue;
		private EnterExit entranceEnEx = null;
		private Vector3 pos = null;
		private FileHandler propHandler = null;

		public House(ref bool failed, Vector3 newPos=null, byte newID=byte.MaxValue, ushort enEx=ushort.MaxValue, bool beingCreated=false)
		{
			if(newID > getMaxHouses()) return;

			string fileName = null;
			string dir = getPropertyFilePath(this.propID, ref fileName);

			this.propHandler = new FileHandler(FileTypes.PROPERTY, dir, fileName);

			if(beingCreated)
			{
				if(newPos == null || newID >= getMaxHouses()) 
				{
					propHandler = null;
					failed = true;
					return;
				}

				this.pos = newPos;
				this.propID = newID;
				this.entrance = enEx;

				this.saveProperty(saveAll: true);
			}

			if(!beingCreated && newID < getMaxHouses()) this.loadProperty();
			else 
			{
				this.propHandler = null;
				failed = true;
			}
		}

		// Static house properties

		public static byte getMaxHouses() {return maxProps;}

		public static void updatePropertyCount(byte value) {House.propCount = value;}
		public static byte getPropertyCount() {return House.propCount;}

		public static string getPropertyFilePath(byte id, ref string fileName) 
		{
			fileName = $"Prop{id}";
			return $"{ServerData.getDefaultServerDir()}ServerData/Properties";
		}

		// General property information

		public void saveProperty(bool saveAll=false, bool saveOwner=false, bool savePos=false, bool saveEnEx=false)
		{
			if(this.propHandler == null) return;
			if(saveAll || saveOwner) this.propHandler.addValue(PropInfo.OWNER.ToString(), ((this.ownerName != null) ? $"{this.ownerName}" : ""));
			if(saveAll || savePos) 
			{
				this.propHandler.addValue(PropInfo.X.ToString(), $"{this.pos.X}");
				this.propHandler.addValue(PropInfo.Y.ToString(), $"{this.pos.Y}");
				this.propHandler.addValue(PropInfo.Z.ToString(), $"{this.pos.Z}");
			}
			if(saveAll || saveEnEx) this.propHandler.addValue(PropInfo.ENEX.ToString(), $"{entrance}");

			this.propHandler.saveFile();
		}

		public void loadProperty()
		{
			string fileName = null;
			string dir = getPropertyFilePath(this.getPropertyID(), ref fileName);
			if(this.propHandler == null) this.propHandler = new FileHandler(FileTypes.PROPERTY, dir, fileName);

			if(!this.propHandler.loadFile()) return;
			string key = $"{PropInfo.OWNER.ToString()}", xStr = null, yStr = null, zStr = null, enExStr = null;
			if(this.propHandler.containsKey(key)) this.ownerName = this.propHandler.getValue(key);
			key = $"{PropInfo.X.ToString()}";
			if(this.propHandler.containsKey(key)) xStr = this.propHandler.getValue(key);
			key = $"{PropInfo.Y.ToString()}";
			if(this.propHandler.containsKey(key)) yStr = this.propHandler.getValue(key);
			key = $"{PropInfo.Z.ToString()}";
			if(this.propHandler.containsKey(key)) zStr = this.propHandler.getValue(key);
			key = $"{PropInfo.ENEX.ToString()}";
			if(this.propHandler.containsKey(key)) enExStr = this.propHandler.getValue(key);

			bool failed = false;
			float x = NumberUtils.parseFloat(xStr, ref failed), y = NumberUtils.parseFloat(yStr, ref failed), z = NumberUtils.parseFloat(zStr, ref failed);
			ushort enExID = NumberUtils.parseUnsignedShort(enExStr, ref failed);

			this.pos = new Vector3(x, y, z);
			this.entrance = enExID;

			entranceEnEx = ServerData.getEnEx(this.entrance);
		}

		public void updateOwnerName(string value) 
		{
			this.ownerName = value;
			saveProperty(saveOwner: true);
		}
		public string getOwnerName() {return this.ownerName;}

		public void updatePropertyID(byte value) {this.propID = value;}
		public byte getPropertyID() {return this.propID;}

		public void updatePropertyPos(Vector3 value) 
		{
			this.pos = value;
			this.saveProperty(savePos: true);
		}
		public Vector3 getPropertyPos() {return this.pos;}

		public void updatePropertyEnEx(ushort value) 
		{
			this.entrance = value;
			this.entranceEnEx = ServerData.getEnEx(value);
			this.saveProperty(saveEnEx: true);
		}
		public ushort getPropertyEnExID() {return this.entrance;}
		public EnterExit getPropertyEnEx() {return this.entranceEnEx;}

		public void updatePropertyHandler(FileHandler value) {this.propHandler = value;}
		public FileHandler getPropertyHandler() {return this.propHandler;}
	}
}
