using GTANetworkAPI;

namespace e_freeroam.Commands.General
{
    class Roleplay : Script
    {
        [Command("me", "~r~Usage: /me [Text]", GreedyArg=true)]
        public void Me(Player user, string text)
        {
            string meStr = $"{user.Name} {text}";
            Utilities.ChatUtils.sendClientMessageToAll(Utilities.ServerUtils.ServerData.COLOR_LIGHT_PINK, meStr);
        }

        [Command("do", "~r~Usage: /do [Text]", GreedyArg=true)]
        public void Do(Player user, string text)
        {
            string meStr = $"* {text} ({user.Name})";
            Utilities.ChatUtils.sendClientMessageToAll(Utilities.ServerUtils.ServerData.COLOR_LIGHT_PINK, meStr);
        }
    }
}
