using e_freeroam.Objects;
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

		[Command("ask", GreedyArg=false)]
		public void Ask(Player user, string text="NullCMDText")
		{
			string failMsg = null;
			string[] parameters = {"ask", "Text", text};

            if(ServerData.commandCheck(user, out failMsg, parameters))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }
		}

		[Command("request", GreedyArg=false)]
		public void Request(Player user)
		{
			string failMsg = null;

			if(ServerData.commandCheck(user, out failMsg))
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
				return;
			}

			PlayerData data = PlayerDataInfo.getPlayerData(user);
			if(data.isRequestingToJoinOrg())
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_ORANGE, "You are already requesting to join an organization.");
				ChatUtils.sendClientMessage(user, ServerData.COLOR_YELLOW, "Your request will not be withdrawn until you are accepted, declined or type /removerequest");
				return;
			}

			string orgList = "", newString = null;
			Organization org = null;

			for(sbyte i = 0; i < Organization.getMaxOrgs(); i++)
			{
				org = ServerData.getOrg(i);
				if(org == null || !org.doesOrgExist()) continue;
				newString = $"{org.getName()}{org.getOrgMemberCount()}";//$"{org.getName()}\t{org.getOrgMemberCount()} / {org.getOrgMemberCap()}\t{org.getLeaderName()}";
				ChatUtils.sendClientMessage(user, ServerData.COLOR_ADMIN, $"Org: {newString}");
				orgList = $"{orgList}\n{newString}";
			}

			Dialog dialog = new Dialog(DialogID.DIALOG_ORG_REQUEST, DialogType.LIST, "Request", "Caption", orgList, new string[] {"Select", "Cancel"});
			dialog.showDialogForPlayer(user);
		}

        [Command("register", GreedyArg = true)]
        public void Register(Player user)
        {
			string failMsg = null;

            if(ServerData.commandCheck(user, out failMsg, registered:false, logged:false))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

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

            Dialog register = new Dialog(DialogID.DIALOG_REGISTER, DialogType.PASSWORD, "Register an Account", "You must register an account to play in this server.\n\nType a password below to register an account.", "", new string[] {"Confirm", "Cancel"});
            register.showDialogForPlayer(user);
			return;
        }

        [Command("login", GreedyArg = true)]
        public void Login(Player user, string pass="NullCMDText")
        {
			string failMsg = null;
			string[] parameters = {"login", "Pass", pass};

            if(ServerData.commandCheck(user, out failMsg, parameters, logged:false))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

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

			Dialog login = new Dialog(DialogID.DIALOG_LOGIN, DialogType.PASSWORD, "Login to your account", "Type your password below to login to your account.", "", new string[] {"Login", "Cancel"});
			login.showDialogForPlayer(user);
            return;
        }
    }
}
