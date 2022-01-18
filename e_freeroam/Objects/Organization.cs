using e_freeroam.Utilities;
using e_freeroam.Utilities.PlayerUtils;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;

namespace e_freeroam.Objects
{
    public class Organization
    {
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
			oDirectory = $"{ServerData.getDefaultServerDir()}Organizations/ORG{id}";

            orgHandler = new FileHandler(FileTypes.ORGMETA, oDirectory, "META");
			oMemberHandler = new FileHandler(FileTypes.OMEMBERS, oDirectory, "Members");

            this.exists = orgHandler.loadFile();
            if(!this.exists && beingCreated == null) return;
            if(this.exists)
            {
                this.orgID = id;

                string value = null;

                this.name = orgHandler.getValue(orgInfo.NAME.ToString());

                float x, y, z;
                value = orgHandler.getValue(orgInfo.X.ToString());
                x = NumberUtils.parseFloat(value, (byte) value.Length);
                value = orgHandler.getValue(orgInfo.Y.ToString());
                y = NumberUtils.parseFloat(value, (byte) value.Length);
                value = orgHandler.getValue(orgInfo.Z.ToString());
                z = NumberUtils.parseFloat(value, (byte) value.Length);

				value = orgHandler.getValue(orgInfo.DIMENSION.ToString());
				this.dimension = NumberUtils.parseUnsignedInt(value, (byte) value.Length);

                this.cpLocation = new Vector3(x, y, z);

                value = orgHandler.getValue(orgInfo.COLOR.ToString());
                this.orgColor = ChatUtils.getHexAsColor(value);
            }
            else if(beingCreated == null) return;
            else
            {
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

        public bool doesOrgExist() {return this.exists;}

        public byte getID() {return this.orgID;}

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

		public void moveOrg(Vector3 location)
		{
			this.orgCP.destroyCheckpoint();
			this.cpLocation = location;

			this.orgCP = new Checkpoint2(location, 0, CPType.ORG);
		}

		public void removeOrg()
		{
			this.orgCP.destroyCheckpoint();
			this.orgHandler.deleteFile();
		}

        public Checkpoint2 getCheckpoint2() {return this.orgCP;}

		public byte countOrgMembers(ref byte coLeaders)
		{
			byte members = 0;
			for(byte i = 1; i < ServerData.maxOrgMembers; i++)
			{
				if(oMemberHandler.containsKey($"{OrgMemberInfo.COLEADER.ToString()}{i}")) coLeaders++;
				if(oMemberHandler.containsKey($"{OrgMemberInfo.MEMBER.ToString()}{i}")) members++;
			}

			return members;
		}

		public bool isPlayerInOrg(Player player)
		{
			string name = player.Name;
			return this.oMemberHandler.getInfo().ContainsValue(name);
		}

		public void kickMember(string playerName, bool save=true)
		{
			string key = null;

			for(byte i = 1; i <= this.memberCap; i++)
			{
				if(i == 1) key = $"{OrgMemberInfo.LEADER.ToString()}";
				if(this.oMemberHandler.containsKey(key) && this.oMemberHandler.getValue(key) == playerName) break; 

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
				data.setPlayerOrg(-1);
			}
			else
			{
				FileHandler handler = new FileHandler(FileTypes.PLAYER, $"{ServerData.getDefaultServerDir()}PlayerData/{playerName[0]}", $"{playerName}");
				if(!handler.loadFile()) return;

				handler.addValue(PlayerInfo.ORG.ToString(), $"{-1}");
				handler = null;
			}

			this.memberCount--;

			this.oMemberHandler.removeValue(key);
			if(save) this.oMemberHandler.saveFile();
		}

		public void kickAllMembers()
		{
			string key = null, playerName = null;

			for(int i = 1; i < this.memberCap; i++)
			{
				key = $"{OrgMemberInfo.MEMBER.ToString()}{i}";
				if(this.oMemberHandler.containsKey(key))
				{
					playerName = this.oMemberHandler.getValue(key);
					this.kickMember(playerName, false);
				}
			}

			this.oMemberHandler.saveFile();
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

				for(byte i = this.memberCap; i >= capacity; i--)
				{
					if(removeCOs > 0) key = $"{OrgMemberInfo.COLEADER.ToString()}{i}";
					else key = $"{OrgMemberInfo.MEMBER.ToString()}{i}";
					if(i == 1) this.kickMember(oMemberHandler.getValue(OrgMemberInfo.LEADER.ToString()), false);
					
					if(this.oMemberHandler.containsKey(key)) this.kickMember(this.oMemberHandler.getValue(key), false);
				}
			}

			this.memberCap = capacity;
			this.oMemberHandler.saveFile();
		}
		public byte getOrgMemberCount(ref byte capacity) 
		{
			capacity = this.memberCap;
			return this.memberCount;
		}

		public bool addOrgMember(Player player) 
		{
			if(this.memberCount >= this.memberCap) return false;

			string key = $"MEMBER{++this.memberCount}";
			this.oMemberHandler.addValue(key, player.Name);

			PlayerDataInfo.getPlayerData(player).setPlayerOrg((sbyte) this.orgID);

			this.oMemberHandler.saveFile();
			return true;
		}
    }
}