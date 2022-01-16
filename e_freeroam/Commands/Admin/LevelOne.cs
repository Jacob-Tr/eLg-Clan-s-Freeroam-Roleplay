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

            if(ServerData.commandCheck(user, out failMsg, 1, null, 0))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

            uint adminLevel = PlayerDataInfo.getPlayerData(user).getPlayerAdminLevel();

            ChatUtils.sendClientMessage(user, (adminLevel < 5) ? ServerData.COLOR_ADMIN : ServerData.COLOR_UPPER_ADMIN, "Admin Commands");
            ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, ChatUtils.colorString("Level 1 -", "FFFF00", "FFFFFF") + " /ac, /kick, /asay");
			if(adminLevel >= 2)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, ChatUtils.colorString("Level 2 -", "FFFF00", "FFFFFF") + "");
            }
            if(adminLevel >= 3)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, ChatUtils.colorString("Level 3 -", "FFFF00", "FFFFFF") + " /giveweapon");
            }
            if(adminLevel >= 4)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, ChatUtils.colorString("Level 4 -", "FFFF00", "FFFFFF") + " /vehicle, vcolor, /addserverv");
            }
            if(adminLevel >= 5)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, ChatUtils.colorString("Level 5 -", "FFFF00", "FFFFFF") + "");
            }
            if(adminLevel >= 6)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, ChatUtils.colorString("Level 6 -", "FFFF00", "FFFFFF") + "");
            }
            if(adminLevel >= 7)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, ChatUtils.colorString("Level 7 -", "FFFF00", "FFFFFF") + "");
            }
        }

        [Command("kick", GreedyArg=true)]
        public void kick(Player user, string target="NullCMDStr", string reason=null)
        {
            string failMsg = null;
			string[] parameters = {"kick", "Player ID", target};

            if(ServerData.commandCheck(user, out failMsg, 1, parameters, ((byte) parameters.Length)))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

            uint adminLevel = PlayerDataInfo.getPlayerData(user).getPlayerAdminLevel();

            int targetid = (NumberUtils.isNumeric(target, target.Length)) ? NumberUtils.parseInt(target, target.Length) : -1;
            Player targetPlayer = null;

            if(targetid == -1) foreach(Player player in NAPI.Pools.GetAllPlayers()) if(player.Name.Equals(target)) targetPlayer = player;
            else foreach(Player searchPlayer in NAPI.Pools.GetAllPlayers()) if(searchPlayer.Value == targetid) targetPlayer = searchPlayer;

            if(targetPlayer == null)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid playerid.");
                return;
            }

            if(PlayerDataInfo.getPlayerData(targetPlayer).getPlayerAdminLevel() >= adminLevel)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: You cannot kick higher level admins.");
                return;
            }

            string targetString = (reason == null) ? $"~ You have been kicked from the server for '{reason}'." : "~ You have been kicked from the server.";
            ChatUtils.sendClientMessage(targetPlayer, ServerData.COLOR_RED, targetString);

            string globalString = (reason == null) ? $"~ {targetPlayer.Name} has been kicked from the server for '{reason}'." : $"~ {targetPlayer.Name} has been kicked from the server.";
            ChatUtils.sendClientMessageToAll(ServerData.COLOR_RED, globalString);

            string adString = (reason == null) ? $"{user.Name} has kicked {targetPlayer.Name} (ID{targetPlayer.Value}) for '{reason}'." : $"{user.Name} has kicked {targetPlayer.Name} (ID{targetPlayer.Value}).";
            ChatUtils.sendMessageToAdmins(ServerData.COLOR_ADMIN_NOTES_LOG, adString);

            targetPlayer.Kick();
            return;
        }

        [Command("ac", GreedyArg=true)]
        public void ac(Player user, string text="NullCMDStr")
        {
            string failMsg = null;
			string[] parameters = {"ac", "Text", text};

            if(ServerData.commandCheck(user, out failMsg, 1, parameters, ((byte) parameters.Length)))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

            uint adminLevel = PlayerDataInfo.getPlayerData(user).getPlayerAdminLevel();

            string formatText = $"[Admin Chat] [{user.Name}] {text}";
            ChatUtils.sendMessageToAdmins((adminLevel < 5) ? ServerData.COLOR_ADMIN : ServerData.COLOR_UPPER_ADMIN, formatText, false);
            return;
        }

        [Command("asay", GreedyArg=true)]
        public void aSay(Player user, string text="NullCMDStr")
        {
            string failMsg = null;
			string[] parameters = {"asay", "Text", text};

            if(ServerData.commandCheck(user, out failMsg, 1, parameters, ((byte) parameters.Length)))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

            uint adminLevel = PlayerDataInfo.getPlayerData(user).getPlayerAdminLevel();

            PlayerData data;
            uint targetALevel = 0;
            string formatText = null;

            foreach(Player player in NAPI.Pools.GetAllPlayers())
            {
                data = PlayerDataInfo.getPlayerData(player);
                targetALevel = data.getPlayerAdminLevel();

                formatText = (targetALevel > 0) ? $"* Admin [{user.Name}] [{user.Value}]: {text}" : $"* Admin: {text}";
                ChatUtils.sendClientMessageToAll((adminLevel < 5) ? ServerData.COLOR_ADMIN : ServerData.COLOR_UPPER_ADMIN, formatText);
            }
            return;
        }
    }
}
