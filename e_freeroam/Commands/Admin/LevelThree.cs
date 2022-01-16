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

            if(ServerData.commandCheck(user, out failMsg, 3, parameters, ((byte) parameters.Length)))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

            WeaponHash weapon = NAPI.Util.WeaponNameToModel(weaponName);

            short playerid = ServerData.getTargetPlayerID(playeridStr);

			if(playerid == -1) playerid = ((short) user.Value);

			if(!NumberUtils.isNumeric(ammoStr, ammoStr.Length))
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid ammo value.");
				return;
			}

			short ammo = Utilities.NumberUtils.parseShort(ammoStr, ammoStr.Length);

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
    }
}
