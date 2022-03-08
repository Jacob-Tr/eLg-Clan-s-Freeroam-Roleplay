using e_freeroam.Objects;
using e_freeroam.Utilities;
using e_freeroam.Utilities.PlayerUtils;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;

namespace e_freeroam.Commands.Admin
{
    class LevelThree : Script
    {
        [Command("giveweapon", GreedyArg = true)]
        public void GiveWeapon(Player user, string playeridStr="NullCMDStr", string weaponName="NullCMDStr", string ammoStr="NullCMDStr")
        {
            string failMsg = null;
			string[] parameters = {"giveweapon", "Player ID", "Weapon Name", "Ammo", playeridStr, weaponName, ammoStr};

            if(ServerData.commandCheck(user, out failMsg, parameters, adminLevel: 3))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

			ushort playerid = ushort.MaxValue;
			bool failed = false;

			if(NumberUtils.isNumeric(playeridStr)) 
			{
				if(NumberUtils.isNumericStringNegative(playeridStr))
				{
					ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid player ID.");
					return;
				}
				playerid = NumberUtils.parseUnsignedShort(playeridStr, ref failed);
			}
			else
			{
				Player target = NAPI.Player.GetPlayerFromName(playeridStr);
				if(target != null) playerid = (ushort) target.Value;
			}

			if(playerid == ushort.MaxValue) 
			{
				playerid = ((ushort) user.Value);

				ammoStr = weaponName;
				weaponName = playeridStr;
			}

			if(!NumberUtils.isNumeric(ammoStr) || NumberUtils.isNumericStringNegative(ammoStr))
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid ammo value.");
				return;
			}

			WeaponHash weapon = NAPI.Util.WeaponNameToModel(weaponName);
			ushort ammo = Utilities.NumberUtils.parseUnsignedShort(ammoStr, ref failed);

			Player player = null;
			player = PlayerDataInfo.getPlayerDataFromID(playerid).getPlayer();

            if(player == null)
            {
                user.SendChatMessage("~r~Error: Invalid player ID.");
                return;
            }
            if(weapon == 0)
            {
                user.SendChatMessage("~r~Error: Invalid weapon name.");
                return;
            }
            if(ammo > 999 || ammo < 1)
            {
                user.SendChatMessage($"~r~Error: Invalid ammo amount. {ammo}");
                return;
            }

            NAPI.Player.GivePlayerWeapon(player, weapon, ammo);

            string personalMsg = (user == player) ? $"* You have given yourself a {weapon} with {ammo} ammo." : $"~w~* You have given {player.Name} a {weapon} with {ammo} ammo.";
            string adminMsg = (user == player) ? $"{user.Name} has given themself a {weapon} with {ammo} ammo." : $"~w~* {user.Name} has given {player.Name} (ID{playerid}) a {weapon} with {ammo} ammo.";

            ChatUtils.sendClientMessage(player, ServerData.COLOR_WHITE, personalMsg);
            ChatUtils.sendMessageToAdmins(ServerData.COLOR_ADMIN_NOTES_LOG, adminMsg);
        }

		[Command("aocmds")]
		public void aOCMDS(Player user)
		{
			string output = null;
			if(ServerData.commandCheck(user, out output, adminLevel: 3))
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, output);
				return;
			}

			byte adminLevel = PlayerDataInfo.getPlayerData(user).getAdminLevel();

			ChatUtils.sendClientMessage(user, (adminLevel < 5) ? ServerData.COLOR_ADMIN : ServerData.COLOR_UPPER_ADMIN, "Admin Organization Commands");

			if(adminLevel >= 3) ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"{ChatUtils.colorString("Level 3 -", "FFFF00", "FFFFFF")} /setleader, /kickleader");
			if(adminLevel >= 5) ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"{ChatUtils.colorString("Level 5 -", "FFFF00", "FFFFFF")} /createorg, /deleteorg");
		}

		[Command("setleader", GreedyArg=true)]
		public void setLeader(Player user, string orgIDStr="NullCMDText", string playerName="NullCMDText")
		{
			string[] parameters = {"setleader", "Organization ID", "Player Name", orgIDStr, playerName};
			string output = null;

			if(ServerData.commandCheck(user, out output, parameters, adminLevel: 3))
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, output);
				return;
			}

			if(!NumberUtils.isNumeric(orgIDStr))
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid organization ID.");
				return;
			}

			bool failed = false;
			sbyte orgID = NumberUtils.parseSignedByte(orgIDStr, ref failed);
			if(orgID > Organization.getMaxOrgs() || orgID > Organization.getOrgCount() || orgID < 0)
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid organization ID.");
				return;
			}

			Organization org = ServerData.getOrg(orgID);
			if(org == null)
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid organization ID.");
				return;
			}

			sbyte targetOrgID = -1;
			Player targetPlayer = NAPI.Player.GetPlayerFromName(playerName);
			if(targetPlayer == null)
			{
				FileHandler targetHandler = new FileHandler(FileTypes.PLAYER, PlayerDataInfo.getPlayerDir(playerName), playerName);
				if(!targetHandler.loadFile())
				{
					ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid account name.");
					targetHandler = null;
					return;
				}

				string targetOrgIDStr = targetHandler.getValue(PlayerInfo.ORG.ToString());
				targetOrgID = NumberUtils.parseSignedByte(targetOrgIDStr, ref failed);

				targetHandler = null;
			}
			else targetOrgID = PlayerDataInfo.getPlayerData(targetPlayer).getPlayerOrgID();

			if(targetOrgID != -1)
			{
				if(targetOrgID != orgID) ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, $"{playerName} is in another organization and can't be the leader of this one.");
				else ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, $"Error: {playerName} is already the leader of this organization.");
				return;
			}

			string exLeaderName = org.getLeaderName();

			org.setLeader(playerName);

			Player exLeader = NAPI.Player.GetPlayerFromName(exLeaderName);

			ChatUtils.sendMessageToAdmins(ServerData.COLOR_ADMIN_NOTES_LOG, $"{PlayerDataInfo.getPlayerName(user)} has set {playerName} as leader of {org.getName()}");

			if(exLeader != null) ChatUtils.sendClientMessage(exLeader, ServerData.COLOR_SILVER, $"Your leadership of {org.getName()} has been replaced by {playerName}.");
			if(targetPlayer != null) 
			{
				ChatUtils.sendClientMessage(targetPlayer, ServerData.COLOR_YELLOW, $"An admin has set you as the leader of {org.getName()}.");
				ChatUtils.sendClientMessage(targetPlayer, ServerData.COLOR_GREEN, $"~ Type /lcmds for a list of leader commands.");
			}
		}

		[Command("kickleader", GreedyArg=false)]
		public void kickLeader(Player user, string playerName="NullCMDText")
		{
			string[] parameters = {"kickleader", "Player Name", playerName};
			string output = null;

			if(ServerData.commandCheck(user, out output, parameters, adminLevel: 3))
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, output);
				return;
			}

			sbyte orgID = -1;
			Player target = NAPI.Player.GetPlayerFromName(playerName);
			if(target == null)
			{
				FileHandler targetHandler = new FileHandler(FileTypes.PLAYER, PlayerDataInfo.getPlayerDir(playerName), playerName);
				if(!targetHandler.loadFile())
				{
					ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid account name.");
					targetHandler = null;
					return;
				}

				string orgIDStr = targetHandler.getValue(PlayerInfo.ORG.ToString());

				bool failed = false;
				orgID = NumberUtils.parseSignedByte(orgIDStr, ref failed);
			}
			else orgID = PlayerDataInfo.getPlayerData(target).getPlayerOrgID();

			if(orgID == -1 || ServerData.getOrg(orgID).getLeaderName() != playerName)
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, $"Error: {playerName} is not the leader of an organization.");
				return;
			}

			Organization org = ServerData.getOrg(orgID);

			org.kickMember(playerName);

			ChatUtils.sendMessageToAdmins(ServerData.COLOR_ADMIN_NOTES_LOG, $"{PlayerDataInfo.getPlayerName(user)} has kicked {playerName} as leader of {org.getName()}.");
			if(target != null) ChatUtils.sendClientMessage(target, ServerData.COLOR_RED, $"~ You have been kicked as the leader of {org.getName()}");
		}
    }
}