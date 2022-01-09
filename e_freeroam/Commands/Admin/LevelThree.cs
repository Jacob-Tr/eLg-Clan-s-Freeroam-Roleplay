using e_freeroam.Utilities;
using e_freeroam.Utilities.PlayerUtils;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;

namespace e_freeroam.Commands.Admin
{
    class LevelThree : Script
    {
        [Command("giveweapon", "~r~Usage: /giveweapon [Player ID] [Weapon Name] [Ammo]", GreedyArg = true)]
        public void GiveWeapon(Player user, string playeridStr, string weaponName, string ammoStr)
        {
            if(PlayerDataInfo.getPlayerData(user).getPlayerAdminLevel() < 3)
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, "Error: Command not found.");
                return;
            }

            WeaponHash weapon = NAPI.Util.WeaponNameToModel(weaponName);
            Player player = null;

            int playerid = Utilities.NumberUtils.isNumeric(playeridStr, playeridStr.Length) ? Utilities.NumberUtils.parseInt(playeridStr, playeridStr.Length) : -1, ammo = Utilities.NumberUtils.parseInt(ammoStr, ammoStr.Length);

            foreach(Player pool in NAPI.Pools.GetAllPlayers()) if(pool.Value == playerid) player = pool;

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
    }
}
