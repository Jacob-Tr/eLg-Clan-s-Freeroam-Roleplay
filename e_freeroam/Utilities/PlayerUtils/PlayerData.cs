using GTANetworkAPI;
using e_freeroam.Utilities;
using e_freeroam.Utilities.ServerUtils;

namespace e_freeroam.Utilities.PlayerUtils
{
    public class PlayerData
    {
        private FileHandler playerHandler = null;
        private Player player = null;
        public PlayerData(Player user)
        {
            this.playerHandler = new FileHandler(FileTypes.PLAYER, $"{ServerData.getDefaultServerDir()}PlayerData\\", $"{user.Name[0]}\\{user.Name}.ini");
            this.player = user;

            PlayerDataInfo.addPlayerData(this);
        }

        public Player getPlayer() {return this.player;}
        public FileHandler getPlayerFileHandler() {return this.playerHandler;}
    }
}
