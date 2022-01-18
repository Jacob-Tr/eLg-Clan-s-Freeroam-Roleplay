using e_freeroam.Utilities;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;

namespace e_freeroam
{
	class LevelFive : Script
    {
		[Command("createorg", GreedyArg=true)]
		public void CreateOrg(Player user, string hex="NullCMDStr", string name="NullCMDStr")
		{
			string failMsg = null;
			string[] parameters = {"createorg", "Color", "Name", hex, name};

            if(ServerData.commandCheck(user, out failMsg, 5, parameters, ((byte) parameters.Length)))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

			if(!ChatUtils.isValidHex(hex, user))
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid RGB format.");
				return;
			}

			if(name.Length > 48)
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Organization names must be a maximum of 48 characters.");
				return;
			}

			Vector3 vect = user.Position;

			sbyte id = ServerData.addOrg(name, ChatUtils.getHexAsColor(hex), user);

			if(id == -1)
			{
				ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, $"The number of organizations has reached the limit of {ServerData.maxOrgs}.");
			}

			ChatUtils.sendClientMessage(user, ServerData.COLOR_SILVER, $"Organization '{name}' created (ID = {id})");
			ChatUtils.sendMessageToAdmins(ServerData.COLOR_ADMIN_NOTES_LOG, $"{user.Name} has created organization '{name}' (ID{id}).");
		}
    }
}
