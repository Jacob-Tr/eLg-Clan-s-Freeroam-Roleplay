using GTANetworkAPI;
using e_freeroam.Utilities;
using e_freeroam.Utilities.ServerUtils;

namespace e_freeroam.Utilities.PlayerUtils
{
    public class PlayerData
    {
        private FileHandler playerHandler = null;
        private Player player = null;

        private bool logged = false, registered = false;

        private byte logAtt = 0;
        private byte adminLevel = 0, supportLevel = 0, vipLevel = 0, health = 0, armor = 0;
        private sbyte organization = -1;

        private float x = 0F, y = 0F, z = 0F, ang = 0F;

		int money = 0;

        public PlayerData(Player user)
        {
            this.playerHandler = new FileHandler(FileTypes.PLAYER, $"{ServerData.getDefaultServerDir()}PlayerData/{user.Name[0]}", $"{user.Name}");
            this.player = user;

            if(this.playerHandler.loadFile()) {this.registered = true;}
        }

        public void createAccount(string password)
        {
            this.playerHandler.addValue(PlayerInfo.ADLVL.ToString(), "4");
			this.playerHandler.addValue(PlayerInfo.STLVL.ToString(), "0");
			this.playerHandler.addValue(PlayerInfo.VIPLVL.ToString(), "0");

            this.playerHandler.addValue(PlayerInfo.HEALTH.ToString(), "100");
            this.health = 100;
            this.playerHandler.addValue(PlayerInfo.ARMOR.ToString(), "0");
            this.armor = 0;

            Player user = this.player;

            Vector3 currentPos = user.Position;

            this.x = user.Position.X;
            this.y = user.Position.Y;
            this.z = user.Position.Z;
            this.playerHandler.addValue(PlayerInfo.X.ToString(), $"{this.x}");
            this.playerHandler.addValue(PlayerInfo.Y.ToString(), $"{this.y}");
            this.playerHandler.addValue(PlayerInfo.Z.ToString(), $"{this.z}");

            this.ang = user.Heading;
            this.playerHandler.addValue(PlayerInfo.ANGLE.ToString(), $"{ang}");

            this.playerHandler.addValue(PlayerInfo.PASSWORD.ToString(), password);

			this.playerHandler.addValue(PlayerInfo.ORG.ToString(), $"{-1}");

			this.playerHandler.addValue(PlayerInfo.MONEY.ToString(), $"{50000}");

            ChatUtils.sendClientMessage(player, ServerData.COLOR_YELLOW, $"Account {player.Name} created, you have been logged in automatically!");
            ChatUtils.sendClientMessageToAll(ServerData.COLOR_GREEN, $"~ {player.Name} has registered a new account. Welcome to our server!");

            this.setPlayerRegistered(true);
            this.setPlayerLoggedIn(true);

            this.playerHandler.saveFile();

            this.loadAccount(password);
        }

        public void loadAccount(string password)
        {
            string confirm = playerHandler.getValue(PlayerInfo.PASSWORD.ToString());
            if(!(password.Equals(confirm)))
            {
                ChatUtils.sendClientMessage(this.player, ServerData.COLOR_RED, $"Login attempt failed. ({++this.logAtt}/3)");
                if(this.logAtt == 3)
                {
                    ChatUtils.sendClientMessage(this.player, ServerData.COLOR_WHITE, "Bad password threshold reached. You have been kicked!");
                    this.player.Kick();
                    ChatUtils.sendClientMessageToAll(ServerData.COLOR_WHITE, $"~ {this.player.Name} has been kicked from the server (failing to login).");
                }
                return;
            }
            this.logAtt = 0;

			string value = null;

            value = playerHandler.getValue(PlayerInfo.ADLVL.ToString());
            this.adminLevel = (byte) NumberUtils.parseByte(value, (byte) value.Length);

            value = playerHandler.getValue(PlayerInfo.STLVL.ToString());
            this.supportLevel = (byte) NumberUtils.parseByte(value, (byte) value.Length);

			value = playerHandler.getValue(PlayerInfo.VIPLVL.ToString());
            this.vipLevel = (byte) NumberUtils.parseByte(value, (byte) value.Length);

            value = playerHandler.getValue(PlayerInfo.HEALTH.ToString());
            this.health = (byte) NumberUtils.parseByte(value, (byte) value.Length);

            value = playerHandler.getValue(PlayerInfo.ARMOR.ToString());
            this.armor = (byte) NumberUtils.parseByte(value, (byte) value.Length);

            value = playerHandler.getValue(PlayerInfo.X.ToString());
            this.x = NumberUtils.parseFloat(value, (byte) value.Length);

            value = playerHandler.getValue(PlayerInfo.Y.ToString());
            this.y = NumberUtils.parseFloat(value, (byte) value.Length);

            value = playerHandler.getValue(PlayerInfo.Z.ToString());
            this.z = NumberUtils.parseFloat(value, (byte) value.Length);

            value = playerHandler.getValue(PlayerInfo.ANGLE.ToString());
            this.ang = NumberUtils.parseFloat(value, (byte) value.Length);

			value = playerHandler.getValue(PlayerInfo.ORG.ToString());
			this.organization = NumberUtils.parseSignedByte(value, (byte) value.Length);

			value = playerHandler.getValue(PlayerInfo.MONEY.ToString());
            this.money = NumberUtils.parseInt(value, (byte) value.Length);

            if (adminLevel > 0)
            {
                ChatUtils.sendClientMessage(player, (this.adminLevel < 5) ? ServerData.COLOR_ADMIN : ServerData.COLOR_UPPER_ADMIN, $"Admin Level - {this.adminLevel}");
                ChatUtils.sendMessageToAdmins((this.adminLevel < 5) ? ServerData.COLOR_ADMIN : ServerData.COLOR_UPPER_ADMIN, $"* {player.Name} (ID{player.Value}) has logged in as admin level {this.adminLevel}.");
            }

            float angle = 0F;
            NAPI.Player.SetPlayerHealth(player, this.getPlayerHealth());
            NAPI.Player.SetPlayerArmor(player, this.getPlayerArmor());
            player.Position = this.getPlayerPos(out angle);
            player.Rotation = new Vector3(0, 0, angle);

			if(!ServerData.getOrg(this.organization).isPlayerInOrg(player)) this.setPlayerOrg(-1);

            this.setPlayerLoggedIn(true);
        }

        public bool isPlayerLoggedIn() {return this.logged;}
        public void setPlayerLoggedIn(bool value) {this.logged = value;}
        public bool isPlayerRegistered() {return this.registered;}
        public void setPlayerRegistered(bool value) {this.registered = value;}

        public Player getPlayer() {return this.player;}

        public FileHandler getPlayerFileHandler() {return this.playerHandler;}

        public byte getPlayerAdminLevel() {return this.adminLevel;}
        public void updatePlayerAdminLevel(byte value)
        {
            this.adminLevel = value;
            this.playerHandler.addValue(ServerUtils.PlayerInfo.ADLVL.ToString(), "" + value);
        }

        public byte getPlayerHealth() {return this.health;}
        public void updatePlayerHealth(byte value)
        {
            this.health = value;
            this.playerHandler.addValue(ServerUtils.PlayerInfo.HEALTH.ToString(), "" + value);
        }

        public int getPlayerArmor() {return this.armor;}
        public void updatePlayerArmor(byte value)
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
            this.x = ((float)pos.X);
            this.y = ((float)pos.Y);
            this.z = ((float)pos.Z);
            this.ang = angle;

            this.playerHandler.addValue(PlayerInfo.X.ToString(), $"{this.x}");
            this.playerHandler.addValue(PlayerInfo.Y.ToString(), $"{this.y}");
            this.playerHandler.addValue(PlayerInfo.Z.ToString(), $"{this.z}");
            this.playerHandler.addValue(PlayerInfo.ANGLE.ToString(), $"{this.ang}");
        }

		public sbyte getPlayerOrg() {return this.organization;}
		public void setPlayerOrg(sbyte value)
		{
			this.organization = value;
			this.playerHandler.addValue(PlayerInfo.ORG.ToString(), $"{value}");

			this.playerHandler.saveFile();
		}


    }
}
