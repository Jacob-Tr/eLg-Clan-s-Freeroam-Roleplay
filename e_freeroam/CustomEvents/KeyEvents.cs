using GTANetworkAPI;
using System.Reflection;

namespace e_freeroam.CustomEvents
{
    class KeyEvents : Script
    {
        [RemoteEvent("OnKeyPress")]
        public void KeyPressEvent(Player player, int key)
        {
            Utilities.ChatUtils.sendClientMessage(player, Utilities.ChatUtils.getColorAsHex(Utilities.ServerUtils.ServerData.COLOR_WANTED_BLUE), "" + key);
        }

        [RemoteEvent("OnKeyRelease")]
        public void KeyReleaseEvent(Player player, int key)
        {
            Utilities.ChatUtils.sendClientMessage(player, Utilities.ChatUtils.getColorAsHex(Utilities.ServerUtils.ServerData.COLOR_WANTED_BLUE), "" + key);
        }
    }
}
