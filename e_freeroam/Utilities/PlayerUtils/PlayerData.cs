using GTANetworkAPI;
using e_freeroam.Utilities;
using e_freeroam.Utilities.ServerUtils;

namespace e_freeroam.Utilities.PlayerUtils
{
    public class PlayerData
    {
        private FileHandler playerHandler = null;
        private Player player = null;

        private uint adminLevel = 0;
        private int health = 0;
        private int armor = 0;

        private float x = 0F, y = 0F, z = 0F, ang = 0F;

        Vector3 pos = null;

        public PlayerData(Player user, out bool newAcc)
        {
            this.playerHandler = new FileHandler(FileTypes.PLAYER, $"{ServerData.getDefaultServerDir()}PlayerData\\{user.Name[0]}", $"{user.Name}");
            this.player = user;

            if(!this.playerHandler.loadFile())
            {
                this.playerHandler.addValue(PlayerInfo.ADLVL.ToString(), "0");
                this.adminLevel = 0;

                this.playerHandler.addValue(PlayerInfo.HEALTH.ToString(), "100");
                this.health = 100;
                this.playerHandler.addValue(PlayerInfo.ARMOR.ToString(), "0");
                this.armor = 0;

                Vector3 currentPos = user.Position;

                this.x = user.Position.X;
                this.y = user.Position.Y;
                this.z = user.Position.Z;
                this.playerHandler.addValue(PlayerInfo.X.ToString(), "" + this.x);
                this.playerHandler.addValue(PlayerInfo.Y.ToString(), "" + this.y);
                this.playerHandler.addValue(PlayerInfo.Z.ToString(), "" + this.z);

                this.ang = user.Heading;
                this.playerHandler.addValue(PlayerInfo.ANGLE.ToString(), "" + ang);

                this.playerHandler.saveFile();

                newAcc = true;
            }
            else
            {
                string adStr = playerHandler.getValue(PlayerInfo.ADLVL.ToString());
                this.adminLevel = ((uint)NumberUtils.parseInt(adStr, adStr.Length));

                string hStr = playerHandler.getValue(PlayerInfo.HEALTH.ToString());
                this.health = NumberUtils.parseInt(adStr, adStr.Length);

                string aStr = playerHandler.getValue(PlayerInfo.ARMOR.ToString());
                this.armor = NumberUtils.parseInt(adStr, adStr.Length);

                string xStr = playerHandler.getValue(PlayerInfo.X.ToString());
                this.x = NumberUtils.parseFloat(adStr, adStr.Length);

                string yStr = playerHandler.getValue(PlayerInfo.Y.ToString());
                this.y = NumberUtils.parseFloat(adStr, adStr.Length);

                string zStr = playerHandler.getValue(PlayerInfo.Z.ToString());
                this.z = NumberUtils.parseFloat(adStr, adStr.Length);

                string anStr = playerHandler.getValue(PlayerInfo.ANGLE.ToString());
                this.ang = NumberUtils.parseFloat(adStr, adStr.Length);

                newAcc = false;
            }

            PlayerDataInfo.addPlayerData(this);
        }

        public Player getPlayer() {return this.player;}

        public FileHandler getPlayerFileHandler() {return this.playerHandler;}

        public uint getPlayerAdminLevel() {return this.adminLevel;}
        public void updatePlayerAdminLevel(uint value)
        {
            this.adminLevel = value;
            this.playerHandler.addValue(ServerUtils.PlayerInfo.ADLVL.ToString(), "" + value);
        }

        public int getPlayerHealth() {return this.health;}
        public void updatePlayerHealth(int value)
        {
            this.health = value;
            this.playerHandler.addValue(ServerUtils.PlayerInfo.HEALTH.ToString(), "" + value);
        }

        public int getPlayerArmor() {return this.armor;}
        public void updatePlayerArmor(int value)
        {
            this.armor = value;
            this.playerHandler.addValue(ServerUtils.PlayerInfo.ARMOR.ToString(), "" + value);
        }

        public Vector3 getPlayerPos(out float angle) 
        {
            angle = ang;
            return new Vector3(this.x, this.y, this.z);
        }
        public void updatePlayerPos(Vector3 pos, float angle)
        {
            this.x = pos.X;
            this.y = pos.Y;
            this.z = pos.Z;
            this.ang = angle;

            this.playerHandler.addValue(ServerUtils.PlayerInfo.X.ToString(), "" + this.x);
            this.playerHandler.addValue(ServerUtils.PlayerInfo.Y.ToString(), "" + this.y);
            this.playerHandler.addValue(ServerUtils.PlayerInfo.Z.ToString(), "" + this.z);
            this.playerHandler.addValue(ServerUtils.PlayerInfo.ANGLE.ToString(), "" + this.ang);
        }
    }
}
