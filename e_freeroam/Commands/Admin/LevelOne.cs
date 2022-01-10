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
            uint adminLevel = PlayerDataInfo.getPlayerData(user).getPlayerAdminLevel();
            if(adminLevel == 0)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, "Error: Command not found.");
                return;
            }
            ChatUtils.sendClientMessage(user, (adminLevel < 5) ? ServerData.COLOR_ADMIN : ServerData.COLOR_UPPER_ADMIN, "Admin Commands");
            ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"!{0xFFFF00}Level 1 -!{0xFFFFFF} /ac, /kick, /asay");
            if(adminLevel >= 2)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"!{0xFFFF00}Level 2 -!{0xFFFFFF}");
            }
            if (adminLevel >= 3)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"!{0xFFFF00}Level 3 -!{0xFFFFFF} /giveweapon");
            }
            if (adminLevel >= 4)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"!{0xFFFF00}Level 4 -!{0xFFFFFF} /vehicle");
            }
            if (adminLevel >= 5)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"!{0xFFFF00}Level 5 -!{0xFFFFFF}");
            }
            if (adminLevel >= 6)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"!{0xFFFF00}Level 6 -!{0xFFFFFF}");
            }
            if (adminLevel >= 7)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"!{0xFFFF00}Level 7 -!{0xFFFFFF} /addserverv");
            }
        }

        [Command("kick", "~r~Usage: /kick [Player ID]", GreedyArg=true)]
        public void kick(Player user, string target, string reason=null)
        {
            uint adminLevel = PlayerDataInfo.getPlayerData(user).getPlayerAdminLevel();
            if (adminLevel == 0)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, "Error: Command not found.");
                return;
            }

            int targetid = (NumberUtils.isNumeric(target, target.Length)) ? NumberUtils.parseInt(target, target.Length) : -1;
            Player targetPlayer = null;

            if(targetid == -1) foreach(Player player in NAPI.Pools.GetAllPlayers()) if(player.Name.Equals(target)) targetPlayer = player;
            else foreach(Player searchPlayer in NAPI.Pools.GetAllPlayers()) if(searchPlayer.Value == targetid) targetPlayer = searchPlayer;

            if(targetPlayer == null)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid playerid.");
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

        [Command("ac", "~r~Usage: /ac [Text]", GreedyArg=true)]
        public void ac(Player user, string text)
        {
            uint adminLevel = PlayerDataInfo.getPlayerData(user).getPlayerAdminLevel();
            if (adminLevel == 0)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, "Error: Command not found.");
                return;
            }

            string formatText = $"[Admin Chat] [{user.Name}] {text}";
            ChatUtils.sendMessageToAdmins((adminLevel < 5) ? ServerData.COLOR_ADMIN : ServerData.COLOR_UPPER_ADMIN, formatText, false);
            return;
        }

        [Command("asay", "~r~/asay [Text]", GreedyArg=true)]
        public void aSay(Player user, string text)
        {
            uint adminLevel = PlayerDataInfo.getPlayerData(user).getPlayerAdminLevel();
            if (adminLevel == 0)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, "Error: Command not found.");
                return;
            }

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
