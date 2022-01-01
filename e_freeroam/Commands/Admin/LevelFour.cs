using GTANetworkAPI;

namespace e_freeroam_Commands_Admin
{
    class LevelFour: Script
    {
        [Command("vehicle", "~r~Usage: /vehicle [Vehicle ID] [Color 1] [Color 2]", GreedyArg = true, Alias = "v")]
        public void L4_Vehicle(Player user, string model, int color1 = -1, int color2 = -1)
        {
            VehicleHash vehicle = NAPI.Util.VehicleNameToModel(model);

            if (vehicle == 0)
            {
                user.SendChatMessage("~r~Error: Invalid vehicle name.");
                return;
            }

            float x = user.Position.X, y = user.Position.Y, z = user.Position.Z, rot = user.Rotation.Z;
            x = (float)(x + (System.Math.Sin((double)-rot) * 5.0));
            y = (float)(y + (System.Math.Cos((double)-rot) * 5.0));
            rot = rot + (float)90.0;

            Vector3 vect = new Vector3(x, y, z);

            if ((color1 > 159 || color1 < 0) || (color2 > 159 || color2 < 0))
            {
                System.Random colorRand = new System.Random();
                if (color1 > 159 || color1 < 0) color1 = colorRand.Next(160);
                if (color2 > 159 || color2 < 0) color2 = colorRand.Next(160);
            }

            NAPI.Vehicle.SpawnVehicle(NAPI.Vehicle.CreateVehicle((uint)vehicle, vect, rot, color1, color2), vect);

            user.SendChatMessage($"~w~ * You have created a {vehicle}.");
            e_freeroam.Utilities.ChatUtils.SendMessageToAdmins($"~w~{user.Name} has created a {vehicle}.");
        }
    }
}