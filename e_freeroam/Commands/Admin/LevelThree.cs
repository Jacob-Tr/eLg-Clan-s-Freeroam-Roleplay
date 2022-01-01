using GTANetworkAPI;

namespace e_freeroam.Commands.Admin
{
    class LevelThree : Script
    {
        [Command("giveweapon", "~r~Usage: /giveweapon [Player ID] [Weapon Name]", GreedyArg = true)]
        public void GiveWeapon(Player user, uint playerid, string weaponName, uint ammo)
        {
            WeaponHash weapon = NAPI.Util.WeaponNameToModel(weaponName);
            Player player = null;

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
            if(ammo > 999)
            {
                user.SendChatMessage("~r~Error: Invalid ammo amount.");
                return;
            }

            NAPI.Player.GivePlayerWeapon(player, weapon, (int) ammo);

            string personalMsg = (user == player) ? $"~w~* You have given yourself a {weapon} with {ammo} ammo." : $"~w~* You have given {player.Name} a {weapon} with {ammo} ammo.";
            string adminMsg = (user == player) ? $"~w~* {user.Name} has given themself a {weapon} with {ammo} ammo." : $"~w~* {user.Name} has given {player.Name} (ID{playerid}) a {weapon} with {ammo} ammo.";

            user.SendChatMessage(personalMsg);
            e_freeroam.Utilities.ChatUtils.SendMessageToAdmins(adminMsg);
        }
    }
}
