using GTANetworkAPI;

namespace e_freeroam.Utilities.ServerUtils
{
    public abstract class ServerData
    {
        public const int maxKeys = 100;

        public static string getDefaultServerDir() {return "scriptfiles\\";}
    }
}
