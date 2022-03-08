using GTANetworkAPI;
using e_freeroam.Utilities;
using e_freeroam.Utilities.ServerUtils;
using e_freeroam.Objects;
using System;
using e_freeroam.Server_Properties;

namespace e_freeroam.Utilities.PlayerUtils
{
    public class PlayerData
    {
        private FileHandler playerHandler = null;
        private Player player = null;

		private Color playerColor;
		private ColShape lastColShape = null;
		private ColShape currentColShape = null;

		private Player lastClickedPlayer = null;

		private string name = null, password = null;

        private bool logged = false, registered = false, muted = false, frozen = false, banned = false, typing = false;
		private sbyte requestingOrg = -1;
		private ushort mutedTime = 0;

		private sbyte organization = -1;
        private byte logAtt = 0, flooding = 0;
        private byte adminLevel = 0, supportLevel = 0, vipLevel = 0, orgLevel = 0, health = 0, armor = 0;
		private byte timesMuted = 0, timesFrozen = 0, timesKicked = 0, timesBanned = 0;

		private byte business = byte.MaxValue;

		private ushort dialogID = ushort.MaxValue;

        private float x = 0F, y = 0F, z = 0F, ang = 0F;

		private int accountID = 0, money = 0;
		private ushort score = 0, scoreCounter = 0;

        public PlayerData(Player user)
        {
			this.name = this.getPlayerName();
			this.playerColor = ServerData.COLOR_YELLOW;

            this.playerHandler = new FileHandler(FileTypes.PLAYER, $"{ServerData.getDefaultServerDir()}PlayerData/{this.name[0]}/{this.name}", $"{this.name}");
            this.player = user;

            if(this.playerHandler.loadFile()) {this.registered = true;}
        }

		// General Information

		public Player getPlayer() {return this.player;}

		public string getPlayerName() {return this.name;}

		public Color getPlayerColor() {return this.playerColor;}
		public void setPlayerColor(Color color) 
		{
			this.playerColor = color;
			NAPI.Player.SetPlayerName(this.player, ChatUtils.colorString($"{this.getPlayerName()}", ChatUtils.getColorAsHex(this.playerColor),  ChatUtils.getColorAsHex(ServerData.COLOR_WHITE)));
		}

		public void resetTempData()
		{
			this.lastClickedPlayer = null;
			this.currentColShape = null;
			this.lastColShape = null;
		}

		// Account

        public void createAccount(string password)
        {
			this.playerHandler.addValue(PlayerInfo.ACCID.ToString(), $"{ServerData.getAccountCount()}");
			this.playerHandler.addValue(PlayerInfo.PASSWORD.ToString(), password);

            this.playerHandler.addValue(PlayerInfo.ADLVL.ToString(), "4");
			this.playerHandler.addValue(PlayerInfo.STLVL.ToString(), "0");
			this.playerHandler.addValue(PlayerInfo.VIPLVL.ToString(), "0");

            this.playerHandler.addValue(PlayerInfo.HEALTH.ToString(), "100");
            this.health = 100;
            this.playerHandler.addValue(PlayerInfo.ARMOR.ToString(), "0");
            this.armor = 0;

            Player user = this.player;

            this.x = user.Position.X;
            this.y = user.Position.Y;
            this.z = user.Position.Z;
            this.playerHandler.addValue(PlayerInfo.X.ToString(), $"{this.x}");
            this.playerHandler.addValue(PlayerInfo.Y.ToString(), $"{this.y}");
            this.playerHandler.addValue(PlayerInfo.Z.ToString(), $"{this.z}");

            this.ang = user.Heading;
            this.playerHandler.addValue(PlayerInfo.ANGLE.ToString(), $"{ang}");

			this.playerHandler.addValue(PlayerInfo.MUTED.ToString(), "0");
			this.playerHandler.addValue(PlayerInfo.MUTEDTIME.ToString(), "0");
			this.playerHandler.addValue(PlayerInfo.FROZEN.ToString(), "0");
			this.playerHandler.addValue(PlayerInfo.BANNED.ToString(), "0");

			this.playerHandler.addValue(PlayerInfo.TIMES_MUTED.ToString(), "0");
			this.playerHandler.addValue(PlayerInfo.TIMES_FROZEN.ToString(), "0");
			this.playerHandler.addValue(PlayerInfo.TIMES_KICKED.ToString(), "0");
			this.playerHandler.addValue(PlayerInfo.TIMES_BANNED.ToString(), "0");

			this.playerHandler.addValue(PlayerInfo.ORG.ToString(), "-1");
			this.playerHandler.addValue(PlayerInfo.ORGLVL.ToString(), "-1");

			this.playerHandler.addValue(PlayerInfo.MONEY.ToString(), "50000");
			this.playerHandler.addValue(PlayerInfo.SCORE.ToString(), "0");
			this.playerHandler.addValue(PlayerInfo.SCORECOUNTER.ToString(), "0");

			this.playerHandler.addValue(PlayerInfo.BUSINESS.ToString(), $"{byte.MaxValue}");

            ChatUtils.sendClientMessage(player, ServerData.COLOR_YELLOW, $"Account {this.getPlayerName()} created, you have been logged in automatically!");
            ChatUtils.sendClientMessageToAll(ServerData.COLOR_GREEN, $"~ {this.getPlayerName()} has registered a new account. Welcome to our server!");

            this.setPlayerRegistered(true);
            this.setPlayerLoggedIn(true);

			ServerData.updateAccountCount(ServerData.getAccountCount() + 1);

            this.playerHandler.saveFile();

            this.loadAccount(password);

			password = null;
        }

        public void loadAccount(string password)
        {
            string confirm = playerHandler.getValue(PlayerInfo.PASSWORD.ToString());
            if(!(password.Equals(confirm)))
            {
				Dialog login = new Dialog(DialogID.DIALOG_LOGIN, DialogType.PASSWORD, $"Login to your account. ({++this.logAtt}/3)", "Type your password below to login to your account.", "", new string[] {"Login", "Cancel"});
				login.showDialogForPlayer(player);

                if(this.logAtt == 3)
                {
                    ChatUtils.sendClientMessage(this.player, ServerData.COLOR_WHITE, "Bad password threshold reached. You have been kicked!");
                    this.player.Kick();
                    ChatUtils.sendClientMessageToAll(ServerData.COLOR_WHITE, $"~ {this.getPlayerName()} has been kicked from the server (failing to login).");
                }
                return;
            }
            this.logAtt = 0;
			password = null;

			this.setPlayerLoggedIn(true);

			bool failed = false;
			this.accountID = (byte) NumberUtils.parseInt(playerHandler.getValue(PlayerInfo.ACCID.ToString()), ref failed);
			if(failed)
			{
				ServerData.sendServerFatalError($"{this.getPlayerName()} does not have a valid account ID.");
				NAPI.Player.KickPlayer(this.player);
				return;
			}

            this.adminLevel = (byte) NumberUtils.parseByte(playerHandler.getValue(PlayerInfo.ADLVL.ToString(), true, "0"), ref failed);
            this.supportLevel = (byte) NumberUtils.parseByte(playerHandler.getValue(PlayerInfo.STLVL.ToString(), true, "0"), ref failed);
            this.vipLevel = (byte) NumberUtils.parseByte(playerHandler.getValue(PlayerInfo.VIPLVL.ToString(), true, "0"), ref failed);

            this.health = (byte) NumberUtils.parseByte(playerHandler.getValue(PlayerInfo.HEALTH.ToString(), true, "100"), ref failed);
            this.armor = (byte) NumberUtils.parseByte(playerHandler.getValue(PlayerInfo.ARMOR.ToString(), true, "0"), ref failed);

            this.x = NumberUtils.parseFloat(playerHandler.getValue(PlayerInfo.X.ToString(), true), ref failed);
            this.y = NumberUtils.parseFloat(playerHandler.getValue(PlayerInfo.Y.ToString(), true), ref failed);
            this.z = NumberUtils.parseFloat(playerHandler.getValue(PlayerInfo.Z.ToString(), true), ref failed);
            this.ang = NumberUtils.parseFloat(playerHandler.getValue(PlayerInfo.ANGLE.ToString(), true), ref failed);

			this.muted = (NumberUtils.parseByte(playerHandler.getValue(PlayerInfo.MUTED.ToString(), true, "0"), ref failed) == (byte) 1);
			this.mutedTime = (NumberUtils.parseUnsignedShort(playerHandler.getValue(PlayerInfo.MUTEDTIME.ToString(), true, "0"), ref failed));
			this.frozen = (NumberUtils.parseByte(playerHandler.getValue(PlayerInfo.FROZEN.ToString(), true, "0"), ref failed) == (byte) 1);
			this.banned = (NumberUtils.parseByte(playerHandler.getValue(PlayerInfo.BANNED.ToString(), true, "0"), ref failed) == (byte) 1);

			this.timesMuted = NumberUtils.parseByte(playerHandler.getValue(PlayerInfo.TIMES_MUTED.ToString(), true, "0"), ref failed);
			this.timesFrozen = NumberUtils.parseByte(playerHandler.getValue(PlayerInfo.TIMES_FROZEN.ToString(), true, "0"), ref failed);
			this.timesKicked = NumberUtils.parseByte(playerHandler.getValue(PlayerInfo.TIMES_KICKED.ToString(), true, "0"), ref failed);
			this.timesBanned = NumberUtils.parseByte(playerHandler.getValue(PlayerInfo.TIMES_BANNED.ToString(), true, "0"), ref failed);

			this.organization = NumberUtils.parseSignedByte(playerHandler.getValue(PlayerInfo.ORG.ToString(), true, "-1"), ref failed);
			this.orgLevel = NumberUtils.parseByte(playerHandler.getValue(PlayerInfo.ORGLVL.ToString(), true, "-1"), ref failed);

            this.money = NumberUtils.parseInt(playerHandler.getValue(PlayerInfo.MONEY.ToString(), true, "0"), ref failed);
			this.score = NumberUtils.parseUnsignedShort(playerHandler.getValue(PlayerInfo.SCORE.ToString(), true, "0"), ref failed);
			this.scoreCounter = NumberUtils.parseUnsignedShort(playerHandler.getValue(PlayerInfo.SCORECOUNTER.ToString(), true, "0"), ref failed);

			this.business = NumberUtils.parseByte(playerHandler.getValue(PlayerInfo.BUSINESS.ToString(), true, $"{this.business}"), ref failed);

            if(adminLevel > 0)
            {
                ChatUtils.sendClientMessage(this.player, (this.adminLevel < 5) ? ServerData.COLOR_ADMIN : ServerData.COLOR_UPPER_ADMIN, $"Admin Level - {this.adminLevel}");
                ChatUtils.sendMessageToAdmins((this.adminLevel < 5) ? ServerData.COLOR_ADMIN : ServerData.COLOR_UPPER_ADMIN, $"* {this.getPlayerName()} (ID{player.Value}) has logged in as admin level {this.adminLevel}.", false, sender: player);
            }

			string orgName = "None";
			if(this.organization != -1 && (ServerData.getOrg(this.organization) == null || !ServerData.getOrg(this.organization).isPlayerInOrg(player))) this.organization = -1;
			else if(this.organization != -1) orgName = ServerData.getOrg(this.organization).getName();

			ChatUtils.sendClientMessage(this.player, ServerData.COLOR_YELLOW, $"Organization: {orgName}");

			if(this.isMuted()) {ChatUtils.sendClientMessageToAll(ServerData.COLOR_RED, $"{this.getPlayerName()} has been auto-muted for the remainder of their duration.");}

			if(this.business != byte.MaxValue)
			{
				Business bus = ServerData.getBusiness(this.business);
				if(bus.getOwnerName() != this.getPlayerName()) this.setPlayerBusiness(byte.MaxValue);
			}

            float angle = ang;
            NAPI.Player.SetPlayerHealth(this.player, this.getPlayerHealth());
            NAPI.Player.SetPlayerArmor(this.player, this.getPlayerArmor());

            this.player.Position = new Vector3(this.x, this.y, this.z);
            this.player.Rotation = new Vector3(0, 0, angle);

			this.freezePlayer(false);
        }

		public void savePlayerData(bool overrideLog=false)
		{
			if(!this.logged && !overrideLog) return;

            this.x = this.player.Position.X;
            this.y = this.player.Position.Y;
            this.z = this.player.Position.Z;
            this.playerHandler.addValue(PlayerInfo.X.ToString(), $"{this.x}");
            this.playerHandler.addValue(PlayerInfo.Y.ToString(), $"{this.y}");
            this.playerHandler.addValue(PlayerInfo.Z.ToString(), $"{this.z}");

            this.ang = this.player.Heading;
            this.playerHandler.addValue(PlayerInfo.ANGLE.ToString(), $"{ang}");

			this.playerHandler.addValue(PlayerInfo.MUTED.ToString(), $"{((this.muted == true) ? 1 : 0)}");
			this.playerHandler.addValue(PlayerInfo.MUTEDTIME.ToString(), $"{this.mutedTime}");

			this.playerHandler.addValue(PlayerInfo.TIMES_MUTED.ToString(), $"{this.timesMuted}");
			this.playerHandler.addValue(PlayerInfo.TIMES_FROZEN.ToString(), $"{this.timesFrozen}");
			this.playerHandler.addValue(PlayerInfo.TIMES_KICKED.ToString(), $"{this.timesKicked}");
			this.playerHandler.addValue(PlayerInfo.TIMES_BANNED.ToString(), $"{this.timesBanned}");

			this.playerHandler.addValue(PlayerInfo.ORG.ToString(), $"{this.organization}");
			this.playerHandler.addValue(PlayerInfo.ORGLVL.ToString(), $"{this.orgLevel}");

			this.playerHandler.addValue(PlayerInfo.MONEY.ToString(), $"{this.money}");
			this.playerHandler.addValue(PlayerInfo.SCORE.ToString(), $"{this.score}");
			this.playerHandler.addValue(PlayerInfo.SCORECOUNTER.ToString(), $"{this.scoreCounter}");

			this.playerHandler.addValue(PlayerInfo.BUSINESS.ToString(), $"{this.business}");

			this.playerHandler.saveFile();
		}

        public FileHandler getPlayerFileHandler() {return this.playerHandler;}

		// Account Information

		public bool isPlayerTyping() {return this.typing;}
		public void updateTypingStatus(bool value) {this.typing = value;}

        public byte getPlayerHealth() {return this.health;}
        public void updatePlayerHealth(byte value)
        {
            this.health = value;
            this.playerHandler.addValue(ServerUtils.PlayerInfo.HEALTH.ToString(), "" + value);
        }

        public byte getPlayerArmor() {return this.armor;}
        public void updatePlayerArmor(byte value)
        {
            this.armor = value;
            this.playerHandler.addValue(ServerUtils.PlayerInfo.ARMOR.ToString(), "" + value);
        }

		public ushort getPlayerScore() {return this.score;}
		public void updatePlayerScore(ushort value) {this.score = value;}

		public ushort getPlayerScoreCounter() {return this.scoreCounter;}
		public void updatePlayerScoreCounter(ushort value) {this.scoreCounter = value;}

		public int getPlayerMoney() {return this.money;}
		public void updatePlayerMoney(int value)
		{
			this.money = value;
			NAPI.ClientEventThreadSafe.TriggerClientEvent(this.player.Handle, "UpdateMoney", value);
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

		public ColShape getPlayerCurrentColshape() {return this.currentColShape;}
		public void updatePlayerCurrentColshape(ColShape newShape) {this.currentColShape = newShape;}

		public ColShape getPlayerLastColshape() {return this.lastColShape;}
		public void updatePlayerLastColshape(ColShape newShape) {this.lastColShape = newShape;}

		public Player getLastClickedPlayer() {return this.lastClickedPlayer;}
		public void updateLastClickedPlayer(Player player) {this.lastClickedPlayer = player;}

		public void setPlayerDialog(ushort value) {this.dialogID = value;}
		public ushort getPlayerDialog() {return this.dialogID;}

		//* Login/Register
        public bool isPlayerLoggedIn() {return this.logged;}
        public void setPlayerLoggedIn(bool value) {this.logged = value;}
        public bool isPlayerRegistered() {return this.registered;}
        public void setPlayerRegistered(bool value) {this.registered = value;}

		public void setPlayerPassword(string pass) {this.password = pass;}
		public string getPlayerPassword() {return this.password;}

		//* Punishment checks
		public bool isMuted() {return (this.mutedTime > 0);}
		public ushort getMutedTime() {return this.mutedTime;}
		public void updateMutedTime(ushort value) {this.mutedTime = value;}

		public bool isFrozen() {return this.frozen;}

		public ushort getFloodTimer() {return this.flooding;}
		public void updateFloodTimer(byte value) {this.flooding = value;}

		//* Positions

        public byte getAdminLevel() {return this.adminLevel;}
        public void updateAdminLevel(byte value)
        {
			if(value > 7) return;

            this.adminLevel = value;
            this.playerHandler.addValue(ServerUtils.PlayerInfo.ADLVL.ToString(), $"{value}");
        }

		public byte getSupportLevel() {return this.supportLevel;}
		public void updateSupportLevel(byte value)
		{
			if(value > 2) return;

			this.supportLevel = value;
			this.playerHandler.addValue(ServerUtils.PlayerInfo.STLVL.ToString(), $"{value}");
		}

		public byte getVIPLevel() {return this.vipLevel;}
		public void updateVIPLevel(byte value)
		{
			if(value > 3) return;

			this.vipLevel = value;
			this.playerHandler.addValue(ServerUtils.PlayerInfo.VIPLVL.ToString(), $"{value}");
		}

		public sbyte getPlayerOrgID() {return this.organization;}
		public Organization getPlayerOrg() {return ServerData.getOrg(this.organization);}
		public void setPlayerOrg(Organization org, OrgLevels level) {this.setPlayerOrgByID((sbyte) org.getID(), level);}
		public void setPlayerOrgByID(sbyte value, OrgLevels level)
		{
			this.organization = value;
			this.playerHandler.addValue(PlayerInfo.ORG.ToString(), $"{value}");
			this.playerHandler.addValue(PlayerInfo.ORGLVL.ToString(), $"{(byte) level}");

			this.savePlayerData();
		}

		public byte getPlayerOrgLevel() {return this.orgLevel;}
		public void setPlayerOrgLevel(OrgLevels level) {if(Enum.IsDefined(typeof(OrgLevels), (byte) level)) this.orgLevel = (byte) level;}

		public bool isRequestingToJoinOrg(sbyte orgID) {return (this.requestingOrg == orgID);}
		public bool isRequestingToJoinOrg() {return (this.requestingOrg != -1);}
		public void updateRequestToJoinOrg(sbyte orgID) {this.requestingOrg = orgID;}

		//* Business related
		public byte getPlayerBusinessID() {return this.business;}
		public Business getPlayerBusiness() 
		{
			if(this.business == Business.getMaxBusinesses()) return null;
			return ServerData.getBusiness(this.business);
		}

		public void setPlayerBusiness(byte id) {this.business = id;}
		public bool doesPlayerOwnBusiness() {return this.business != Business.getMaxBusinesses();} // Business IDs are not inclusive of the max value.

		// Punishments

		public void kickPlayer()
		{
			this.timesKicked++;
			this.player.Kick();
		}

		public void mutePlayer(bool value, ushort time=0)
		{
			this.timesMuted++;

			this.muted = value;
			if(value) this.mutedTime = time;
		}

		public void freezePlayer(bool value)
		{
			this.timesFrozen++;

			NAPI.ClientEventThreadSafe.TriggerClientEvent(this.player.Handle, "FreezePlayer", value);
			this.frozen = value;
		}

		public void banPlayer()
		{
			this.banned = true;
			this.player.Kick();
		}
	}
}