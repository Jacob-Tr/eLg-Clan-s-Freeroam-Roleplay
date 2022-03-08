using e_freeroam.Objects;
using e_freeroam.Utilities;
using e_freeroam.Utilities.PlayerUtils;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace e_freeroam.Server_Properties
{
	public class Business
	{
		private static byte maxBusinesses = 200, businessCount = 0;

		private byte id = 0;
		private ushort cpID = ushort.MaxValue;

		private float X = 0, Y = 0, Z = 0;
		private uint world = 0;

		private uint price = 0, profit = 0;

		string ownerName = null, name = null;

		private FileHandler busHandler = null;

		private Checkpoint2 busCP = null;

		public Business(ref bool loadFailed, float x=0, float y=0, float z=0, uint newWorld=0, int newPrice=0, byte newID=byte.MaxValue, string newName=null, bool beingCreated=false)
		{
			if(newID == byte.MaxValue) this.id = (byte) (getBusCount() + 1);
			busHandler = new FileHandler(Utilities.ServerUtils.FileTypes.BUSINESS, $"{ServerData.getDefaultServerDir()}ServerData/Businesses", $"BUS{id}");

			if(!busHandler.loadFile())
			{
				if(!beingCreated)
				{
					busHandler = null;
					loadFailed = true;
					return;
				}

				ServerData.logToConsole($"{this.getBusinessID()} didn't fail.");

				busHandler.addValue(BusInfo.OWNER.ToString(), $"");

				if(newName == null) busHandler.addValue(BusInfo.NAME.ToString(), $"");
				else busHandler.addValue(BusInfo.NAME.ToString(), newName);

				busHandler.addValue(BusInfo.X.ToString(), $"{x}");
				busHandler.addValue(BusInfo.Y.ToString(), $"{y}");
				busHandler.addValue(BusInfo.Z.ToString(), $"{z}");

				busHandler.addValue(BusInfo.WORLD.ToString(), $"{newWorld}");

				busHandler.addValue(BusInfo.PRICE.ToString(), $"{newPrice}");
				busHandler.addValue(BusInfo.PROFIT.ToString(), $"{0}");

				busHandler.saveFile();
				busHandler.loadFile();
			}

			bool failed = false;

			this.ownerName = busHandler.getValue(BusInfo.OWNER.ToString());
			this.name = busHandler.getValue(BusInfo.NAME.ToString());

			this.X = NumberUtils.parseFloat(busHandler.getValue(BusInfo.X.ToString()), ref failed);
			this.Y = NumberUtils.parseFloat(busHandler.getValue(BusInfo.Y.ToString()), ref failed);
			this.Z = NumberUtils.parseFloat(busHandler.getValue(BusInfo.Z.ToString()), ref failed);

			this.world = NumberUtils.parseUnsignedInt(busHandler.getValue(BusInfo.WORLD.ToString()), ref failed);

			this.price = NumberUtils.parseUnsignedInt(busHandler.getValue(BusInfo.PRICE.ToString()), ref failed);
			this.profit = NumberUtils.parseUnsignedInt(busHandler.getValue(BusInfo.PROFIT.ToString()), ref failed);

			this.busCP = ServerData.addCP(this.getBusinessPos(), this.world, CPType.BUSINESS);
			this.cpID = busCP.getID();
		}

		// Static business properties

		public static byte getMaxBusinesses() {return maxBusinesses;}

		public static byte getBusCount() {return businessCount;}
		public static void updateBusCount(byte value) {businessCount = value;}

		// General business utilities

		public void saveBusiness(bool all=false, bool owner=false, bool name=false, bool coords=false, bool cost=false, bool output=false, bool vWorld=false, bool intID=false)
		{
			if(all || owner) this.busHandler.addValue(BusInfo.OWNER.ToString(), this.ownerName);
			if(all || name) this.busHandler.addValue(BusInfo.NAME.ToString(), this.name);

			if(all || coords) 
			{ 
				this.busHandler.addValue(BusInfo.X.ToString(), $"{this.X}");
				this.busHandler.addValue(BusInfo.Y.ToString(), $"{this.Y}");
				this.busHandler.addValue(BusInfo.Z.ToString(), $"{this.Z}");
			}

			if(all || cost) this.busHandler.addValue(BusInfo.PRICE.ToString(), $"{this.price}");
			if(all || output) this.busHandler.addValue(BusInfo.PROFIT.ToString(), $"{this.profit}");

			if(all || vWorld) this.busHandler.addValue(BusInfo.WORLD.ToString(), $"{this.world}");

			this.busHandler.saveFile();
		}

		public void deleteBusiness()
		{
			ServerData.removeCP(this.cpID);
			ServerData.addBusiness(null, this.id);
			this.busHandler.deleteFile();
		}

		// General business information

		public byte getBusinessID() {return this.id;}
		public void updateBusinessID(byte newID)
		{
			FileHandler newBusHandler = new FileHandler(FileTypes.BUSINESS, $"{ServerData.getDefaultServerDir()}Businesses", $"BUS{newID}");
			newBusHandler.setInfo(this.busHandler.getInfo());

			this.busHandler.deleteFile();
			this.busHandler = newBusHandler;

			this.saveBusiness();
		}

		public string getBusinessName() {return this.name;}
		public void updateBusinessName(string newName) 
		{
			this.name = newName;
			this.saveBusiness(owner: true);
		}

		public Vector3 getBusinessPos() {return new Vector3(this.X, this.Y, this.Z);}
		public void updateBusinessPos(float x, float y, float z) 
		{
			this.X = x;
			this.Y = y;
			this.Z = z;

			this.saveBusiness(coords: true);
		}

		public uint getBusinessWorld() {return this.world;}
		public void updateBusinessWorld(uint vWorld)
		{
			this.world = vWorld;
			this.saveBusiness(vWorld: true);
		}
		
		public uint getBusinessPrice() {return this.price;}
		public void updateBusinessPrice(uint newPrice)
		{
			this.price = newPrice;
			this.saveBusiness(cost: true);
		}

		public uint getBusinessProfit() {return this.profit;}
		public void updateBusinessProfit(uint value) 
		{
			this.profit = value;
			this.saveBusiness(output: true);
		}

		public int getBusinessOutput() {return (int) Math.Floor(this.getBusinessPrice() / 60F);}

		public bool isOwned() {return (this.ownerName != "");}

		public Checkpoint2 getBusCP() {return this.busCP;}

		// Player related

		public string getOwnerName() {return this.ownerName;}
		public void updateOwnerName(string newName)
		{
			this.ownerName = newName;
			this.saveBusiness(owner: true);
		}

		public void transferProfitToOwner()
		{
			FileHandler playerHandler = null;
			bool playerOnline = false;

			if(PlayerDataInfo.isPlayerOnline(this.ownerName)) 
			{
				PlayerData data = PlayerDataInfo.getPlayerData(NAPI.Player.GetPlayerFromName(this.ownerName));
				if(data.isPlayerLoggedIn())
				{
					playerOnline = true;
					playerHandler = data.getPlayerFileHandler();
					data.updatePlayerMoney((int) (data.getPlayerMoney() + this.profit));
				}
			}
			else if(!playerOnline)
			{
				playerHandler = new FileHandler(FileTypes.PLAYER, $"{ServerData.getDefaultServerDir()}PlayerData/{this.ownerName[0]}", this.ownerName);
				if(!playerHandler.loadFile())
				{
					ServerData.logToConsole($"Transfer of profit (${this.profit}) from business {this.name} (#{this.id}) to {ownerName} failed.");
					return;
				}

				bool failed = false;
				uint playerCash = NumberUtils.parseUnsignedInt(playerHandler.getValue(PlayerInfo.MONEY.ToString()), ref failed);

				playerHandler.addValue(PlayerInfo.MONEY.ToString(), $"{playerCash + this.profit}");
			}

			this.updateBusinessProfit(0);
			ServerData.logToConsole($"{this.ownerName} has collected their business profit from business {this.name} (#{this.id}).");
		}

		public void setOwner(string playerName)
		{
			Player player = NAPI.Player.GetPlayerFromName(playerName);
			PlayerData data = null;

			if(player != null) data = PlayerDataInfo.getPlayerData(player);

			FileHandler playerHandler = null;
			if(data == null) 
			{
				playerHandler = new FileHandler(FileTypes.PLAYER, $"{ServerData.getDefaultServerDir()}PlayerData/{playerName[0]}", playerName);
				if(!playerHandler.loadFile())
				{
					ServerData.logToConsole($"{playerName} was unable to be set owner of Business #{this.id} because their account does not exist.");
					return;
				}
			}
			else 
			{
				playerHandler = data.getPlayerFileHandler();
				if(!playerHandler.hasFileBeenLoaded() || !data.isPlayerLoggedIn())
				{
					ServerData.logToConsole($"Currently online account {playerName} was unable to be set owner of business #{this.id} as they are not logged in.");
					return;
				}
			}
			
			this.updateOwnerName(playerName);
			data.setPlayerBusiness(this.id);

			ServerData.logToConsole($"{playerName} now owns business {this.name} (#{this.id}).");
		}
	}
}
