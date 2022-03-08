using e_freeroam.Server_Properties;
using e_freeroam.Utilities;
using e_freeroam.Utilities.PlayerUtils;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;

namespace e_freeroam.Commands.General
{
	class Bus : Script
	{
		[Command("buybus")]
		public void buyBusiness(Player user)
		{
            string failMsg = null;
			string[] parameters = {"buybus"};

            if(ServerData.commandCheck(user, out failMsg, parameters))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

			PlayerData data = PlayerDataInfo.getPlayerData(user);
			object colshapeOwner = ServerData.getColShapeOwner(data.getPlayerCurrentColshape());

			if(!(colshapeOwner is Bus))
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: You must be at a business.");
				return;
			}

			Business bus = (Business) colshapeOwner;
			string playerName = PlayerDataInfo.getPlayerName(user);
			if(bus.isOwned())
			{
				if(bus.getOwnerName() == playerName) ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: You already own this business.");
				else ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Business is already owned.");
				return;
			}

			if(data.doesPlayerOwnBusiness())
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: You already own a business.");
				return;
			}

			if(data.getPlayerMoney() < bus.getBusinessPrice())
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: You cannot afford to buy this business.");
				return;
			}

			data.updatePlayerMoney((int) (data.getPlayerMoney() - bus.getBusinessPrice()));
			data.setPlayerBusiness(bus.getBusinessID());
			bus.updateOwnerName(playerName);

			ChatUtils.sendClientMessage(user, ServerData.COLOR_GREEN, $"~ You have successfully purchased {bus.getBusinessName()}.");
			ChatUtils.sendClientMessage(user, ServerData.COLOR_YELLOW, $"This business produces {bus.getBusinessOutput()} every 15 minutes.");

			ServerData.logToConsole($"{playerName} has purchased business: {bus.getBusinessName()} ({bus.getBusinessID()}) for ${bus.getBusinessPrice()}.");
		}

		[Command("sellbus")]
		public void SellBus(Player user)
		{
            string failMsg = null;
			string[] parameters = {"sellbus"};

            if(ServerData.commandCheck(user, out failMsg, parameters))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

			PlayerData data = PlayerDataInfo.getPlayerData(user);
			ColShape shape = data.getPlayerCurrentColshape();

			object shapeOwner = ServerData.getColShapeOwner(shape);
			if(!(shapeOwner is Business))
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: You must be at your business.");
				return;
			}

			Business bus = (Business) shapeOwner;
			if(bus.getOwnerName() != PlayerDataInfo.getPlayerName(user))
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: You do not own this business.");
				if(data.getPlayerBusinessID() == bus.getBusinessID()) data.setPlayerBusiness(byte.MaxValue);
				return;
			}

			uint profit = bus.getBusinessProfit();
			ChatUtils.sendClientMessage(user, ServerData.COLOR_LBLUE, $"You have collected ${profit} from your business.");
			profit += bus.getBusinessProfit();
			data.updatePlayerMoney((int) (data.getPlayerMoney() + profit));

			ChatUtils.sendMessageToAdmins(ServerData.COLOR_ADMIN_NOTES_LOG, $"{PlayerDataInfo.getPlayerName(user)} has sold their business: {bus.getBusinessName()} ({bus.getBusinessID()}) and recieved ${profit}.");
		}
	}
}
