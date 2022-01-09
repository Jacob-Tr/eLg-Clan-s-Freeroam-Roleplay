using GTANetworkAPI;
using e_freeroam.Utilities;
using e_freeroam.Utilities.ServerUtils;

namespace e_freeroam_Commands_Admin
{
    class LevelFour: Script
    {
        [Command("vehicle", "~r~Usage: /vehicle [Vehicle ID] [Color 1] [Color 2]", GreedyArg = true, Alias = "v")]
        public void Vehicle(Player user, string model, string color1Str = "-1", string color2Str = "-1")
        {
            int color1 = e_freeroam.Utilities.NumberUtils.parseInt(color1Str, color1Str.Length), color2 = e_freeroam.Utilities.NumberUtils.parseInt(color2Str, color2Str.Length); ;

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
            ChatUtils.sendMessageToAdmins($"{user.Name} has created a {model}.");
        }
    }
}