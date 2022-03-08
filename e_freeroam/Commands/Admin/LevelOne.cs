using e_freeroam.Utilities;
using e_freeroam.Utilities.PlayerUtils;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;

namespace e_freeroam.Commands.Admin
{
    class LevelOne : Script
    {
        [Command("acmds", "~r~Usage: /acmds", GreedyArg = false)]
        public void ACMDS(Player user)
        {
            string failMsg = null;

            if(ServerData.commandCheck(user, out failMsg, adminLevel: 1))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

            byte adminLevel = PlayerDataInfo.getPlayerData(user).getAdminLevel();

            ChatUtils.sendClientMessage(user, (adminLevel < 5) ? ServerData.COLOR_ADMIN : ServerData.COLOR_UPPER_ADMIN, "Admin Commands");
            ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"{ChatUtils.colorString("Level 1 -", "FFFF00", "FFFFFF")} /ac, /kick, /asay, /freeze, /unfreeze");
			if(adminLevel >= 2)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"{ChatUtils.colorString("Level 2 -", "FFFF00", "FFFFFF")} /respawnv, /vrespawn");
            }
            if(adminLevel >= 3)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"{ChatUtils.colorString("Level 3 -", "FFFF00", "FFFFFF")} /aocmds, /giveweapon");
            }
            if(adminLevel >= 4)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"{ChatUtils.colorString("Level 4 -", "FFFF00", "FFFFFF")} /vehicle, vcolor, /addserverv");
            }
            if(adminLevel >= 5)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"{ChatUtils.colorString("Level 5 -", "FFFF00", "FFFFFF")} /createorg, /deleteorg,  /addbus, /removebus, /addstore, /removestore, /addgasstation");
				ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"/removegasstation, /viewpropint");
            }
            if(adminLevel >= 6)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"{ChatUtils.colorString("Level 6 -", "FFFF00", "FFFFFF")} /setsmember, /setadmin");
            }
            if(adminLevel >= 7)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"{ChatUtils.colorString("Level 7 -", "FFFF00", "FFFFFF")} ");
            }
        }

        [Command("kick", GreedyArg=true)]
        public void kick(Player user, string target="NullCMDStr", string reason=null)
        {
            string failMsg = null;
			string[] parameters = {"kick", "Player ID", target};

            if(ServerData.commandCheck(user, out failMsg, parameters, adminLevel: 1))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

            uint adminLevel = PlayerDataInfo.getPlayerData(user).getAdminLevel();

			bool failed = false;
            short targetid = (NumberUtils.isNumeric(target)) ? NumberUtils.parseShort(target, ref failed) : (short) -1;
            Player targetPlayer = null;

			targetPlayer = NAPI.Player.GetPlayerFromName(target);
            if(targetPlayer == null) targetPlayer = PlayerDataInfo.getPlayerDataFromID(NumberUtils.parseUnsignedShort(target, ref failed)).getPlayer();

            if(targetPlayer == null)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid playerid.");
                return;
            }

            if(PlayerDataInfo.getPlayerData(targetPlayer).getAdminLevel() >= adminLevel)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: You cannot kick higher level admins.");
                return;
            }

            string targetString = (reason == null) ? $"~ You have been kicked from the server for '{reason}'." : "~ You have been kicked from the server.";
            ChatUtils.sendClientMessage(targetPlayer, ServerData.COLOR_RED, targetString);

            string globalString = (reason == null) ? $"~ {PlayerDataInfo.getPlayerName(targetPlayer)} has been kicked from the server for '{reason}'." : $"~ {PlayerDataInfo.getPlayerName(targetPlayer)} has been kicked from the server.";
            ChatUtils.sendClientMessageToAll(ServerData.COLOR_RED, globalString);

            string adString = (reason == null) ? $"{user.Name} has kicked {PlayerDataInfo.getPlayerName(targetPlayer)} (ID{targetPlayer.Value}) for '{reason}'." : $"{user.Name} has kicked {targetPlayer.Name} (ID{targetPlayer.Value}).";
            ChatUtils.sendMessageToAdmins(ServerData.COLOR_ADMIN_NOTES_LOG, adString);

            targetPlayer.Kick();
            return;
        }

        [Command("ac", GreedyArg=true)]
        public void ac(Player user, string text="NullCMDStr")
        {
            string failMsg = null;
			string[] parameters = {"ac", "Text", text};

            if(ServerData.commandCheck(user, out failMsg, parameters, adminLevel: 1))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

            uint adminLevel = PlayerDataInfo.getPlayerData(user).getAdminLevel();

            string formatText = $"[Admin Chat] [{PlayerDataInfo.getPlayerName(user)}] {text}";
            ChatUtils.sendMessageToAdmins((adminLevel < 5) ? ServerData.COLOR_ADMIN : ServerData.COLOR_UPPER_ADMIN, formatText, true, false);
            return;
        }

        [Command("asay", GreedyArg=true)]
        public void aSay(Player user, string text="NullCMDStr")
        {
            string failMsg = null;
			string[] parameters = {"asay", "Text", text};

            if(ServerData.commandCheck(user, out failMsg, parameters, adminLevel: 1))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

            uint adminLevel = PlayerDataInfo.getPlayerData(user).getAdminLevel();

            PlayerData data;
            uint targetALevel = 0;
            string formatText = null;

            foreach(Player player in NAPI.Pools.GetAllPlayers())
            {
                data = PlayerDataInfo.getPlayerData(player);
                targetALevel = data.getAdminLevel();

                formatText = (targetALevel > 0) ? $"* Admin [{PlayerDataInfo.getPlayerName(user)}] [{user.Value}]: {text}" : $"* Admin: {text}";
                ChatUtils.sendMessageToAdmins((adminLevel < 5) ? ServerData.COLOR_ADMIN : ServerData.COLOR_UPPER_ADMIN, formatText, showLog: false);
            }
            return;
        }

		[Command("freeze", GreedyArg=true)]
		public void Freeze(Player user, string targetStr="NullCMDText")
		{
            string failMsg = null;
			string[] parameters = {"freeze", "Target ID",  targetStr};

            if(ServerData.commandCheck(user, out failMsg, parameters, adminLevel: 1))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

			bool failed=false;
			ushort playerid = ushort.MaxValue;
			if(NumberUtils.isNumeric(targetStr)) playerid = NumberUtils.parseUnsignedShort(targetStr, ref failed);

			Player target = null;
			if(playerid == ushort.MaxValue) target = NAPI.Player.GetPlayerFromName(targetStr);

			if(target == null && playerid != ushort.MaxValue) target = PlayerDataInfo.getPlayerDataFromID(playerid).getPlayer();
			if(target == null)
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid target ID.");
				return;
			}

			string name = PlayerDataInfo.getPlayerName(target);

			PlayerData data = PlayerDataInfo.getPlayerData(user);
			PlayerData targetData = PlayerDataInfo.getPlayerData(target);

			if(data.getAdminLevel() <= targetData.getAdminLevel())
			{
				if(data.getPlayer() == targetData.getPlayer()) ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: You cannot freeze yourself.");
				else ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, $"Error: You cannot freeze {name}");
				return;
			}

			if(NAPI.Player.IsPlayerRespawning(target))
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, $"Please wait until {name} has spawned.");
				return;
			}

			data.freezePlayer(true);
			ChatUtils.sendClientMessage(target, ServerData.COLOR_RED, "You have been frozen by an admin.");
			ChatUtils.sendMessageToAdmins(ServerData.COLOR_ADMIN_NOTES_LOG, $"{PlayerDataInfo.getPlayerName(user)} has frozen {name}.");
		}

		[Command("unfreeze")]
		public void Unfreeze(Player user, string targetStr)
		{
           string failMsg = null;
			string[] parameters = {"freeze", "Target ID",  targetStr};

            if(ServerData.commandCheck(user, out failMsg, parameters, adminLevel: 1))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

			bool failed=false;
			ushort playerid = ushort.MaxValue;
			if(NumberUtils.isNumeric(targetStr)) playerid = NumberUtils.parseUnsignedShort(targetStr, ref failed);

			Player target = null;
			if(playerid == ushort.MaxValue) target = NAPI.Player.GetPlayerFromName(targetStr);

			if(target == null && playerid != ushort.MaxValue) target = PlayerDataInfo.getPlayerDataFromID(playerid).getPlayer();
			if(target == null)
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid target ID.");
				return;
			}

			string name = PlayerDataInfo.getPlayerName(target);

			PlayerData data = PlayerDataInfo.getPlayerData(user);
			PlayerData targetData = PlayerDataInfo.getPlayerData(target);

			if(data.getPlayer() == targetData.getPlayer())
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, $"Error: You were frozen by a higher level admin.");
				return;
			}

			if(NAPI.Player.IsPlayerRespawning(target))
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, $"Please wait until {name} has spawned.");
				return;
			}

			data.freezePlayer(false);
			ChatUtils.sendClientMessage(target, ServerData.COLOR_RED, "You have been unfrozen by an admin.");
			ChatUtils.sendMessageToAdmins(ServerData.COLOR_ADMIN_NOTES_LOG, $"{PlayerDataInfo.getPlayerName(user)} has unfrozen {name}.");
		}
    }
}