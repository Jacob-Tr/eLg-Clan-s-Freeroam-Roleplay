using GTANetworkAPI;

namespace e_freeroam.Commands
{
    class Test : Script
    {
        [Command("test", "Usage: /test", GreedyArg=true)]
        public void test(Player user)
        {
            string str = $"This is a test string! {user.Name} And a name!";
            str = Utilities.ChatUtils.colorString(str, Utilities.ChatUtils.getColorAsHex(Utilities.ServerUtils.ServerData.COLOR_BLUE));
            user.SendChatMessage(str);
        }
    }
}
