using e_freeroam.Utilities;
using e_freeroam.Utilities.PlayerUtils;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;
using System;

namespace e_freeroam.Commands.General
{
    class Utility : Script
    {
        [Command("commands", GreedyArg = false)]
        public void Commands(Player user)
        {
            string failMsg = null;

            if(ServerData.commandCheck(user, out failMsg))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

            ChatUtils.sendClientMessage(user, ServerData.COLOR_YELLOW, "Commands");
            ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, "/me, /do");
            return;
        }

        [Command("register", "~r~Usage: /register [Password] [Confirm Password]", GreedyArg = true)]
        public void Register(Player user, string pass, string confirm)
        {
            PlayerData data = PlayerDataInfo.getPlayerData(user);
            if(data.isPlayerLoggedIn())
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, "You are already registered.");
                return;
            }
            if (data.isPlayerRegistered())
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, "Account already exists! Type /login to continue.");
                return;
            }

            string passHash = NAPI.Util.GetHashSha256(pass);
            string confHash = NAPI.Util.GetHashSha256(confirm);

            if(!(passHash.Equals(confHash)))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "The passwords you entered didn't match!");
                return;
            }

            data.createAccount(passHash);
            return;
        }

        [Command("login", "~r~Usage: /login [Password]", GreedyArg = true)]
        public void Login(Player user, string passHash)
        {
            PlayerData data = PlayerDataInfo.getPlayerData(user);
            if(!data.isPlayerRegistered())
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, "Account doesn't exist! Type /register to continue.");
                return;
            }
            if(data.isPlayerLoggedIn())
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, "You are already logged in!");
                return;
            }

            passHash = NAPI.Util.GetHashSha256(passHash);
            data.loadAccount(passHash);
            return;
        }
    }
}
