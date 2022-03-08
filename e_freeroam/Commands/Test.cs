using e_freeroam.Objects;
using e_freeroam.Utilities;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.IO;

namespace e_freeroam.Commands
{
    class Test : Script
    {
        [Command("test", GreedyArg=false)]
        public void test(Player user, string valueStr="NullCMDText")
        {
			string storeInfo = "0.000 0.000 0.000 123123";
			byte index = 0;

			string xStr = ChatUtils.strTok(storeInfo, ref index);
			string yStr = ChatUtils.strTok(storeInfo, ref index);
			string zStr = ChatUtils.strTok(storeInfo, ref index);
			string worldStr = ChatUtils.strTok(storeInfo, ref index);

			ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"Original: {storeInfo}");
			ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"Index: {index} Parsed: {xStr} {yStr} {zStr} {worldStr}");

			/*string failMsg = null;
			string[] parameters = {"test", "Value", valueStr};

            if(ServerData.commandCheck(user, out failMsg, parameters, registered: false, logged: false))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

			byte value = NumberUtils.parseByte(valueStr, (byte) valueStr.Length);
			if(value != 5) //NAPI.ClientEventThreadSafe.TriggerClientEvent(user.Handle, "CreateDialog", 0, 2, "name", "caption", "info", "Confirm", "Cancel");
			{
				Dialog dialog = new Dialog(0, DialogType.PASSWORD, "name", "What if the caption part was a little longer", "What if the info part is super long like this one is", new string[] {"Confirm", "Cancel"});
				dialog.showDialogForPlayer(user);
			}
			else NAPI.ClientEventThreadSafe.TriggerClientEvent(user.Handle, "DestroyDialog");*/
		}

		[Command("save", GreedyArg=true)]
		public void Save(Player user, string description="NullCMDStr")
		{
			string positionDir = (ServerData.getDefaultServerDir() + "ServerData");
			FileHandler posHandler = new FileHandler(FileTypes.POSITION, positionDir, "SavedPositions");
		}
    }
}
