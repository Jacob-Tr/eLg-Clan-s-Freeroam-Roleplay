using e_freeroam.Utilities;
using e_freeroam.Utilities.PlayerUtils;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;

namespace e_freeroam.Objects
{
    public class Organization
    {
		private static byte maxOrgs = 12, maxOrgMembers = 24;
		private static byte orgCount = 0;

		private FileHandler orgHandler = null;
		private FileHandler oMemberHandler = null;

		private Checkpoint2 orgCP = null;
        private Vector3 cpLocation = null;
        private Color orgColor, defaultColor = new Color(0x000000);

		private string oDirectory = null, name = null;

        private byte orgID = byte.MaxValue;
		private byte memberCount = 0, memberCap = 12;
		private ushort cpID = ushort.MaxValue;
		private uint dimension = 0;

        private bool exists = false;

        public Organization(byte id, Player beingCreated=null, string orgName=null)
        {
			oDirectory = $"{ServerData.getDefaultServerDir()}ServerData/Organizations/ORG{id}";

            orgHandler = new FileHandler(FileTypes.ORGMETA, oDirectory, "META");
			oMemberHandler = new FileHandler(FileTypes.OMEMBERS, oDirectory, "Members");

            this.exists = orgHandler.loadFile();
            if(!this.exists && beingCreated == null) return;
            if(this.exists)
            {
                this.orgID = id;

                string value = null;

                this.name = orgHandler.getValue(orgInfo.NAME.ToString());

				bool failed = false;

                float x, y, z;
                value = orgHandler.getValue(orgInfo.X.ToString());
                x = NumberUtils.parseFloat(value, ref failed);
                value = orgHandler.getValue(orgInfo.Y.ToString());
                y = NumberUtils.parseFloat(value, ref failed);
                value = orgHandler.getValue(orgInfo.Z.ToString());
                z = NumberUtils.parseFloat(value, ref failed) + 0.5F;

				value = orgHandler.getValue(orgInfo.DIMENSION.ToString());
				this.dimension = NumberUtils.parseUnsignedInt(value, ref failed);

                this.cpLocation = new Vector3(x, y, z);

                value = orgHandler.getValue(orgInfo.COLOR.ToString());
                this.orgColor = ChatUtils.getHexAsColor(value);
            }
            else
            {
				this.orgID = id;

                this.name = orgName;
                this.cpLocation = beingCreated.Position;
				this.orgColor = defaultColor;
				this.dimension = beingCreated.Dimension;

				this.orgHandler.addValue(orgInfo.NAME.ToString(), this.name);

				this.orgHandler.addValue(orgInfo.X.ToString(), $"{this.cpLocation.X}");
				this.orgHandler.addValue(orgInfo.Y.ToString(), $"{this.cpLocation.Y}");
				this.orgHandler.addValue(orgInfo.Z.ToString(), $"{this.cpLocation.Z}");

				this.orgHandler.addValue(orgInfo.DIMENSION.ToString(), $"{this.dimension}");

				this.orgHandler.addValue(orgInfo.COLOR.ToString(), ChatUtils.getColorAsHex(this.orgColor));

				this.orgHandler.saveFile();
				this.oMemberHandler.saveFile();

                this.exists = true;
            }

            this.orgCP = ServerData.addCP(this.cpLocation, 0, CPType.ORG);
			this.cpID = this.orgCP.getID();
        }

		// Static organization properties

		public static byte getMaxOrgs() {return maxOrgs;}

		public static byte getMaxOrgMembers() {return maxOrgMembers;}

		public static byte getOrgCount() {return orgCount;}
		public static void updateOrgCount(byte value) {orgCount = value;}

		// General organization utilities

        public bool doesOrgExist() {return this.exists;}

        public byte getID() {return this.orgID;}

		public void moveOrg(Vector3 location)
		{
			this.orgCP.destroyCheckpoint();
			this.cpLocation = location;

			this.orgCP = new Checkpoint2(location, 0, CPType.ORG);
		}

		public void removeOrg()
		{
			this.orgCP.destroyCheckpoint();

			this.oMemberHandler.deleteFile();
			this.orgHandler.deleteFile();

			ServerData.removeOrg(this.orgID);
		}

        public Checkpoint2 getCheckpoint2() {return this.orgCP;}

		// Member related

		public bool isPlayerInOrg(Player player)
		{
			string name = player.Name;
			return this.oMemberHandler.containsValue(name);
		}

		public void kickMember(string playerName, bool save=true)
		{
			string key = null;

			for(byte i = 1; i <= this.memberCap; i++)
			{
				if(i == 1) key = OrgMemberInfo.LEADER.ToString();
				if(this.oMemberHandler.containsKey(key) && this.oMemberHandler.getValue(key) == playerName)
				{
					this.kickAllMembers();
					break;
				}

				if(i <= 3)
				{
					key = $"{OrgMemberInfo.COLEADER.ToString()}{(i)}";
					if(this.oMemberHandler.containsKey(key) && this.oMemberHandler.getValue(key) == playerName) break; 
				}

				key = $"{OrgMemberInfo.MEMBER.ToString()}{i}";
				if(this.oMemberHandler.containsKey(key) && this.oMemberHandler.getValue(key) == playerName) break; 
			}

			if(key == null) return;

			Player player = NAPI.Player.GetPlayerFromName(playerName);
			PlayerData data = null;

			if(player != null) 
			{
				data = PlayerDataInfo.getPlayerData(player);
				data.setPlayerOrgByID(-1, OrgLevels.MEMBER);
			}
			else
			{
				FileHandler handler = new FileHandler(FileTypes.PLAYER, $"{ServerData.getDefaultServerDir()}PlayerData/{playerName[0]}", $"{playerName}");
				if(!handler.loadFile()) return;

				handler.addValue(PlayerInfo.ORG.ToString(), $"{-1}");
				handler.saveFile();
				handler = null;
			}

			this.memberCount--;

			this.oMemberHandler.removeValue(key);
			if(save) this.oMemberHandler.saveFile();
		}

		public string getMemberName(byte position)
		{
			string key = $"{OrgMemberInfo.MEMBER.ToString()}{position}";
			if(oMemberHandler.containsKey(key)) return oMemberHandler.getValue(key);
			return null;
		}

		public Player getMember(byte position) 
		{
			string name = getMemberName(position);
			if(name == null) return null;
			return NAPI.Player.GetPlayerFromName(name);
		}

		public void kickAllMembers()
		{
			string key = null, playerName = null;

			for(int i = 1; i < this.memberCap; i++)
			{
				if(i <= 3)
				{
					key = $"{OrgMemberInfo.COLEADER.ToString()}{i}";
					if(this.oMemberHandler.containsKey(key))
					{
						playerName = this.oMemberHandler.getValue(key);
						this.kickMember(playerName, false);
					}
				}
				key = $"{OrgMemberInfo.MEMBER.ToString()}{i}";
				if(this.oMemberHandler.containsKey(key))
				{
					playerName = this.oMemberHandler.getValue(key);
					this.kickMember(playerName, false);
				}
			}

			this.memberCount = 1;
			this.oMemberHandler.saveFile();
		}

		public bool addOrgMember(Player player) 
		{
			if(this.memberCount >= this.memberCap) return false;

			string key = $"MEMBER{++this.memberCount}";
			this.oMemberHandler.addValue(key, player.Name);

			PlayerDataInfo.getPlayerData(player).setPlayerOrg(this, OrgLevels.MEMBER);

			this.oMemberHandler.saveFile();
			return true;
		}

		public void setOrgMemberCap(byte capacity)
		{
			byte members, coLeaders = 0;
			members = this.countOrgMembers(ref coLeaders);

			if(this.memberCount > capacity)
			{
				string key = null;
				sbyte removeCOs = (sbyte) ((coLeaders + 1) - capacity); // CoLeaders +1 to account for leader.

				if(removeCOs > 0) this.kickAllMembers();

				for(byte i = this.memberCap; i > capacity; i--)
				{
					if(removeCOs > 0) key = $"{OrgMemberInfo.COLEADER.ToString()}{i}";
					else key = $"{OrgMemberInfo.MEMBER.ToString()}{i}";
					if(i == 1) this.kickMember(oMemberHandler.getValue(OrgMemberInfo.LEADER.ToString()), false);
					
					if(this.oMemberHandler.containsKey(key)) this.kickMember(this.oMemberHandler.getValue(key), false);
				}
				this.memberCount = capacity;
			}

			this.memberCap = capacity;
			this.oMemberHandler.saveFile();
		}
		public byte getOrgMemberCap() {return this.memberCap;}
		public byte getOrgMemberCount() {return this.memberCount;}

		public byte countOrgMembers(ref byte coLeaders)
		{
			byte members = 0;
			for(byte i = 1; i < maxOrgMembers; i++)
			{
				if(oMemberHandler.containsKey($"{OrgMemberInfo.COLEADER.ToString()}{i}")) coLeaders++;
				if(oMemberHandler.containsKey($"{OrgMemberInfo.MEMBER.ToString()}{i}")) members++;
			}

			return members;
		}

		//* (Co)leader related

		public string getLeaderName() 
		{
			string key = OrgMemberInfo.LEADER.ToString();
			if(oMemberHandler.containsKey(key)) return oMemberHandler.getValue(key);
			return null;
		}
		public string getColeaderName(byte position) 
		{
			string key = $"{OrgMemberInfo.COLEADER.ToString()}{position}";
			if(oMemberHandler.containsKey(key)) return oMemberHandler.getValue(key);
			return null;
		}

		public void setLeader(string playerName)
		{
			string leaderName = this.getLeaderName();
			if(leaderName != null && leaderName != "") this.kickAllMembers();
			if(this.getLeaderName() == playerName) return;

			FileHandler targetHandler = new FileHandler(FileTypes.PLAYER, PlayerDataInfo.getPlayerDir(playerName), playerName);
			if(!targetHandler.loadFile())
			{
				targetHandler = null;
				return;
			}

			targetHandler.addValue(PlayerInfo.ORG.ToString(), $"{this.orgID}");
			targetHandler.addValue(PlayerInfo.ORGLVL.ToString(), $"{(byte) OrgLevels.LEADER}");
			oMemberHandler.addValue(OrgMemberInfo.LEADER.ToString(), playerName);

			oMemberHandler.saveFile();
			targetHandler.saveFile();
		}

		public Player getLeader() 
		{
			string name = this.getLeaderName();
			if(name == null) return null;
			return NAPI.Player.GetPlayerFromName(name);
		}
		public Player getColeader(byte position) 
		{
			string name = getColeaderName(position);
			if(name == null) return null;
			return NAPI.Player.GetPlayerFromName(name);
		}

		public bool isPlayerColeader(Player player)
		{
			if(!this.isPlayerInOrg(player)) return false;

			string playerName = PlayerDataInfo.getPlayerName(player);
			if(this.getLeaderName() == playerName) return true;

			string key = null;
			for(byte i = 0; i <= this.getOrgMemberCount(); i++)
			{
				key = $"{OrgMemberInfo.COLEADER.ToString()}{i}";
				if(this.oMemberHandler.containsKey(key) && this.oMemberHandler.getValue(key) == playerName) return true;
			}

			return false;
		}

		// Organization information

        public void setColor(Color newColor) 
		{
			this.orgColor = newColor;
			this.orgHandler.addValue(orgInfo.COLOR.ToString(), ChatUtils.getColorAsHex(newColor));

			this.orgHandler.saveFile();
		}
		public Color getColor() {return this.orgColor;}

		public bool setName(string newName)
		{
			if(newName.Length < 0 || newName.Length > 48) return false;

			this.name = newName;
			return true;
		}
		public string getName() {return this.name;}
    }
}