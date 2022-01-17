using GTANetworkAPI;
using e_freeroam.Utilities;
using e_freeroam.Utilities.ServerUtils;
using e_freeroam.Utilities.PlayerUtils;

namespace e_freeroam_Commands_Admin
{
    class LevelFour: Script
    {
        [Command("vehicle", GreedyArg=true, Alias = "v")]
        public void Vehicle(Player user, string model="NullCMDStr", string color1Str = "-1", string color2Str = "-1")
        {
            string failMsg = null;
			string[] parameters = {"vehicle", "Model Name", "Color 1", "Color 2", model, color1Str, color2Str};

            if(ServerData.commandCheck(user, out failMsg, 4, parameters, ((byte) parameters.Length)))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

            int color1 = e_freeroam.Utilities.NumberUtils.parseInt(color1Str, (byte) color1Str.Length), color2 = e_freeroam.Utilities.NumberUtils.parseInt(color2Str, (byte) color2Str.Length); ;

            uint hashKey = (uint)NAPI.Util.GetHashKey(model);

            if(hashKey == 0)
            {
                user.SendChatMessage("~r~Error: Invalid vehicle name.");
                return;
            }

            float x = user.Position.X, y = user.Position.Y, z = user.Position.Z, rot = user.Heading;

            x = (float)(x + (System.Math.Sin((double)-rot) * 5.0));
            y = (float)(y + (System.Math.Cos((double)-rot) * 5.0));
            rot += (float)90.0;

            Vector3 vect = new Vector3(x, y, z);
            Vector3 angle = new Vector3(50, 50, rot);

            if ((color1 > 159 || color1 < 0) || (color2 > 159 || color2 < 0))
            {
                System.Random colorRand = new System.Random();

                if (color1 > 159 || color1 < 0) color1 = colorRand.Next(160);
                if (color2 > 159 || color2 < 0) color2 = colorRand.Next(160);
            }

            ServerData.addVehicle(hashKey, vect, angle, color1, color2, VehicleType.CMD_VEHICLE);

            user.SendChatMessage(ChatUtils.colorString($"* You have created a {model}.", ChatUtils.getColorAsHex(ServerData.COLOR_WHITE)));
            ChatUtils.sendMessageToAdmins(ServerData.COLOR_ADMIN_NOTES_LOG, $"{user.Name} has created a {model}.");
        }

        [Command("vcolor", "~r~Usage: /vcolor [Color1] [Color2]", GreedyArg=true)]
        public void VColor(Player user, string color1Str, string color2Str="-1")
        {
            string failMsg = null;

            if(ServerData.commandCheck(user, out failMsg, 4))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, failMsg);
                return;
            }

            int color1 = e_freeroam.Utilities.NumberUtils.parseInt(color1Str, (byte) color1Str.Length), color2 = e_freeroam.Utilities.NumberUtils.parseInt(color2Str, (byte) color2Str.Length); ;

            if((color1 < 0 || color1 > 159) || color2 != -1 && (color2 < 0 || color2 > 159))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: Invalid color ID. (0 - 159)");
                return;
            }

            if(!NAPI.Player.IsPlayerInAnyVehicle(user))
            {
                ChatUtils.sendClientMessage(user, ServerData.COLOR_RED, "Error: You must be inside a vehicle.");
                return;
            }
            Vehicle vehicle = NAPI.Player.GetPlayerVehicle(user);

            if(color2 == -1) color2 = color1;
            vehicle.PrimaryColor = color1;
            vehicle.SecondaryColor = color2;

            ChatUtils.sendClientMessage(user, ServerData.COLOR_WHITE, $"You have changed this vehicle's colors to {vehicle.PrimaryColor} and {vehicle.SecondaryColor}.");
            return;
        }
    }
}