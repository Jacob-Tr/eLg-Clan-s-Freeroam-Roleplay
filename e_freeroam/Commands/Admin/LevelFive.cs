using e_freeroam.Objects;
using e_freeroam.Server_Properties;
using e_freeroam.Utilities;
using e_freeroam.Utilities.PlayerUtils;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;
using System.Linq;

namespace e_freeroam
{
	class LevelFive : Script
    {
		[Command("createorg", GreedyArg=true)]
		public void CreateOrg(Player user, string hex="NullCMDStr", string name="NullCMDStr")
		{
			string failMsg = null;
			string[] parameters = {"createorg", "Color", "Name", hex, name};

            if(ServerData.commandCheck(user, out failMsg, parameters, adminLevel: 5))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

			if(!ChatUtils.isValidHex(hex, user))
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid RGB format.");
				return;
			}

			if(name.Length > 48)
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Organization names must be a maximum of 48 characters.");
				return;
			}

			sbyte id = ServerData.addOrg(name, ChatUtils.getHexAsColor(hex), user);

			if(id == -1)
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, $"The number of organizations has reached the limit of {Organization.getMaxOrgs()}.");
				return;
			}

			ChatUtils.sendClientMessage(user, ServerData.COLOR_SILVER, $"Organization '{name}' created (ID = {id})");
			ChatUtils.sendMessageToAdmins(ServerData.COLOR_ADMIN_NOTES_LOG, $"{user.Name} has created organization '{name}' (ID{id}).");
		}

		[Command("deleteorg")]
		public void DelOrg(Player user, string orgIDStr="NullCMDText")
		{
			string failMsg = null;
			string[] parameters = {"deleteorg", "Organization ID", orgIDStr};

			if(ServerData.commandCheck(user, out failMsg, parameters, adminLevel: 5))
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
				return;
			}

			if(!NumberUtils.isNumeric(orgIDStr))
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid Organization ID.");
				return;
			}
			bool failed = false;
			sbyte orgID = NumberUtils.parseSignedByte(orgIDStr, ref failed);

			Organization org = ServerData.getOrg(orgID);
			if(org == null)
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid Organization ID.");
				return;
			}

			org.removeOrg();
			ChatUtils.sendClientMessageToAll(ServerData.COLOR_WHITE, $"~ An admin has deleted Organization '{org.getName()}'.");
			ChatUtils.sendMessageToAdmins(ServerData.COLOR_ADMIN_NOTES_LOG, $"{PlayerDataInfo.getPlayerName(user)} has deleted Organization '{org.getName()}' ({orgID}).");
		}

		[Command("addbus", GreedyArg=true)]
		public void AddBus(Player user, string busCost="NullCMDText", string busName="NullCMDText")
		{
			string failMsg = null;
			string[] parameters = {"addbus", "Cost", "Name", busCost, busName};

            if(ServerData.commandCheck(user, out failMsg, parameters, adminLevel: 5))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

			if(Business.getBusCount() + 1 >= Business.getMaxBusinesses())
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, $"Error: Business limit reached. {Business.getMaxBusinesses()}");
				return;
			}

			if(!NumberUtils.isNumeric(busCost))
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Usage: /addbus [Cost] [Name]");
				return;
			}

			if(busName.Length < 3 || busName.Length > 48)
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid business name.");
				return;
			}

			bool failed = false;
			int cost = NumberUtils.parseInt(busCost, ref failed);

			if(cost < 1 || cost > 99999999)
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid amount.");
				return;
			}

			Vector3 pos = user.Position;

			failed = false;
			Business bus = new Business(ref failed, pos.X, pos.Y, pos.Z, user.Dimension, cost, byte.MaxValue, busName, true);

			if(failed)
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Failed to add business to server.");
				bus = null;
				return;
			}

			bus.saveBusiness(all: true);
			ServerData.addBusiness(bus, bus.getBusinessID());

			ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"Added business ID {bus.getBusinessID()}, name {busName}, cost ${cost}.");
			ChatUtils.sendMessageToAdmins(ServerData.COLOR_ADMIN_NOTES_LOG, $"{PlayerDataInfo.getPlayerName(user)} has added business ID {bus.getBusinessID()}, name {busName}, cost ${cost}.");
		}

		[Command("removebus")]
		public void RemoveBus(Player user, string businessID="NullCMDText")
		{
			string failMsg = null;
			string[] parameters = {"removebus", "Business ID", businessID};

            if(ServerData.commandCheck(user, out failMsg, parameters, adminLevel: 5))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

			bool failed = false;
			byte busID = NumberUtils.parseByte(businessID, ref failed);

			if(busID >= Business.getMaxBusinesses())
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, $"Error: Invalid business ID.");
				return;
			}

			Business bus = ServerData.getBusiness(busID);
			if(bus == null)
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Business does not exist.");
				return;
			}

			ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"~ Succenssfully removed business {bus.getBusinessName()} ({busID}).");
			ChatUtils.sendMessageToAdmins(ServerData.COLOR_ADMIN_NOTES_LOG, $"{PlayerDataInfo.getPlayerName(user)} has removed business: {bus.getBusinessName()} (#{busID}.");

			ServerData.removeBus(bus.getBusinessID());
		}

		[Command("addstore")]
		public void AddStore(Player user)
		{
			string failMsg = null;
			string[] parameters = {"addstore"};

            if(ServerData.commandCheck(user, out failMsg, parameters, adminLevel: 5))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

			Vector3 pos = user.Position;
			uint interior = user.Dimension;

			Store store = new Store(pos.X, pos.Y, pos.Z, interior);
			ushort storeID = store.getStoreID();

			if(storeID > Store.getMaxStores())
			{
				string output = $"Failed to add store #{storeID}/{Store.getMaxStores()}.";

				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, output);
				ServerData.logToConsole(output);

				store = null;
				return;
			}
			
			if(ServerData.getStorePool().ToList<Store>()[storeID] != null)
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Store already exists.");
				store = null;
				return;
			}

			ServerData.addStore(store);

			ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"Store #{store.getStoreID()} added.");
			ChatUtils.sendMessageToAdmins(ServerData.COLOR_ADMIN_NOTES_LOG, $"{PlayerDataInfo.getPlayerName(user)} has created store #{storeID}.");
		}

		[Command("removestore")]
		public void RemoveStore(Player user, string storeIDStr="NullCMDStr")
		{
			string failMsg = null;

            if(ServerData.commandCheck(user, out failMsg, adminLevel: 5))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

			bool failed = false;
			ushort storeID = NumberUtils.parseUnsignedShort(storeIDStr, ref failed);

			if(ServerData.getStore(storeID) == null)
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid store ID.");
				return;
			}

			ServerData.removeStore(storeID);
			ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"You have removed store #{storeID}.");
			ChatUtils.sendMessageToAdmins(ServerData.COLOR_ADMIN_NOTES_LOG, $"{PlayerDataInfo.getPlayerName(user)} has removed store #{storeID}.");
		}

		[Command("addgasstation")]
		public void AddGasstation(Player user)
		{
			string failMsg = null;
			string[] parameters = {"addgasstation"};

            if(ServerData.commandCheck(user, out failMsg, parameters, adminLevel: 5))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

			Vector3 pos = user.Position;
			uint interior = user.Dimension;

			GasStation gas = new GasStation(pos.X, pos.Y, pos.Z);
			byte gasID = gas.getGasStationID();

			if(gasID >= GasStation.getMaxGasStations())
			{
				string output = $"Failed to add gas station #{gasID}/{GasStation.getMaxGasStations()}.";

				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, output);
				ServerData.logToConsole(output);

				gas = null;
				return;
			}
			
			if(ServerData.getStorePool().ToList<Store>()[gasID] != null)
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Gas station already exists.");
				gas = null;
				return;
			}

			ServerData.addGasStation(gas);

			ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"Gas station #{gasID} added.");
			ChatUtils.sendMessageToAdmins(ServerData.COLOR_ADMIN_NOTES_LOG, $"{PlayerDataInfo.getPlayerName(user)} has created gas station #{gasID}.");
		}

		[Command("removegasstation")]
		public void RemoveGasStation(Player user, string gasIDStr="NullCMDStr")
		{
			string failMsg = null;

            if(ServerData.commandCheck(user, out failMsg, adminLevel: 5))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

			bool failed = false;
			byte gasID = NumberUtils.parseByte(gasIDStr, ref failed);

			if(ServerData.getGasStation(gasID) == null)
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid gas station ID.");
				return;
			}

			ServerData.removeStore(gasID);
			ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"You have removed gas station #{gasID}.");
			ChatUtils.sendMessageToAdmins(ServerData.COLOR_ADMIN_NOTES_LOG, $"{PlayerDataInfo.getPlayerName(user)} has removed gas station #{gasID}.");
		}

		[Command("viewpropint")]
		public void ViewPropInt(Player user, string intIDStr="NullCMDStr")
		{
			string failMsg = null;
			string[] parameters = {"viewpropint", "Interior ID", intIDStr};

            if(ServerData.commandCheck(user, out failMsg, parameters, adminLevel: 5))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

			ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Command is work in progress.");
			return;
		}
    }
}