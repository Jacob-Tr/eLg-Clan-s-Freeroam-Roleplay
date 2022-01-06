using GTANetworkAPI;
using System.Reflection;

namespace e_freeroam.CustomEvents
{
    class KeyEvents : Script
    {
        [RemoteEvent("OnKeyPress")]
        public void KeyPressEvent(Player player, int key)
        {
        }

        [RemoteEvent("OnKeyRelease")]
        public void KeyReleaseEvent(Player player, int key)
        {
        }
    }
}
