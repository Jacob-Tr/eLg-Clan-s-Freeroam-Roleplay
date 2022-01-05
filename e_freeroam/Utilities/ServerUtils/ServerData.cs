using GTANetworkAPI;
using System.Collections.Generic;

namespace e_freeroam.Utilities.ServerUtils
{
    public abstract class ServerData
    {
        public static Color COLOR_WHITE = new Color(0xFFFFFF);
        public static Color COLOR_YELLOW = new Color(0xFFFF00);
        public static Color COLOR_RED = new Color(0xFF0000);
        public static Color COLOR_LRED = new Color(0xFF4747);
        public static Color COLOR_GREEN = new Color(0x00FF00);
        public static Color COLOR_BLUE = new Color(0x0000FF);
        public static Color COLOR_LBLUE = new Color(0xADD8E6);
        public static Color COLOR_MEDIUM_BLUE = new Color(0x3399FF);
        public static Color COLOR_ORANGE = new Color(0xFF9900);
        public static Color COLOR_PURPLE = new Color(0xCC00DD);
        public static Color COLOR_PEACH = new Color(0xFFDAB9);
        public static Color COLOR_SILVER = new Color(0xC0C0C0);
        public static Color COLOR_WANTED_BLUE = new Color(0x0055CC);
        public static Color COLOR_GOLD = new Color(0xD4AF37);
        public static Color COLOR_DEEP_PINK = new Color(0xFF1493);
        public static Color COLOR_LIGHT_CYAN = new Color(0xE0FFFF);
        public static Color COLOR_LIGHT_PINK = new Color(0xFFB6C1);
        public static Color COLOR_SUPPORT = new Color(0x00E5EE);
        public static Color COLOR_ADMIN = new Color(0xCC0000);
        public static Color COLOR_UPPER_ADMIN = new Color(0x339933);
        public static Color COLOR_ADMIN_NOTES_LOG = new Color(0xBAFFFD);
        public static Color COLOR_FREEROAM = new Color(0xFFFF33);
        public static Color COLOR_ROLEPLAY = new Color(0xFFFFFF); // COLOR_WHITE
        public static Color COLOR_LISTEN = new Color(0xFFFF99);
        public static Color COLOR_VIPL1 = new Color(0xFFB7E6);
        public static Color COLOR_VIPL2 = new Color(0xFF84D5);
        public static Color COLOR_VIPL3 = new Color(0xFF51C3);
        public static Color COLOR_VIPL4 = new Color(0xFF1EB2);

        public const int maxKeys = 100;
        public const int maxVehicles = 1000;

        public static List<Vehicle> serverVehicles = new List<Vehicle>(maxVehicles);

        public static int addVehicle(Vehicle vehicle) 
        {
            serverVehicles.Add(vehicle);
            return serverVehicles.FindIndex(target => target == vehicle);
        }
        public static void removeVehicle(int vehicleid) {serverVehicles.RemoveAt(vehicleid);}
        public static int getVehicleID(Vehicle vehicle) {return serverVehicles.FindIndex(target => target == vehicle);}

        public static string getDefaultServerDir() {return "scriptfiles\\";}
    }
}
