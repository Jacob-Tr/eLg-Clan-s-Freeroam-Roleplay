using e_freeroam.Utilities;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;

namespace e_freeroam.Objects
{
    public class Organization
    {
		private Color defaultColor = new Color(0x000000);

        private FileHandler orgHandler = null;
		private Checkpoint2 orgCP = null;
        private string name = null;
        private Vector3 cpLocation = null;
        private Color orgColor;
        private sbyte orgID = sbyte.MaxValue;
		private ushort cpID = ushort.MaxValue;
        private bool exists = false;

        public Organization(sbyte id, Player beingCreated=null, string orgName=null)
        {
            orgHandler = new FileHandler(FileTypes.ORG, ServerData.getDefaultServerDir() + $"Organizations/ORG{id}", $"META");

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

				this.orgHandler.addValue(orgInfo.NAME.ToString(), this.name);

				this.orgHandler.addValue(orgInfo.X.ToString(), $"{this.cpLocation.X}");
				this.orgHandler.addValue(orgInfo.Y.ToString(), $"{this.cpLocation.Y}");
				this.orgHandler.addValue(orgInfo.Z.ToString(), $"{this.cpLocation.Z}");

				this.orgHandler.addValue(orgInfo.COLOR.ToString(), ChatUtils.getColorAsHex(this.orgColor));

				this.orgHandler.saveFile();
                this.exists = true;
            }

            this.orgCP = ServerData.addCP(this.cpLocation, CPType.ORG);
			this.cpID = this.orgCP.getID();
        }

        public bool doesOrgExist() {return this.exists;}

        public int getID() {return this.orgID;}

        public void setColor(Color newColor) 
		{
			this.orgColor = newColor;
			this.orgHandler.addValue(orgInfo.COLOR.ToString(), ChatUtils.getColorAsHex(newColor));

			this.orgHandler.saveFile();
		}

		public void moveOrg(Vector3 location)
		{
			this.orgCP.destroyCheckpoint();
			this.cpLocation = location;

			this.orgCP = new Checkpoint2(location, CPType.ORG);
		}

		public void removeOrg()
		{
			this.orgCP.destroyCheckpoint();
			this.orgHandler.deleteFile();
		}

        public Checkpoint2 getCheckpoint() {return this.orgCP;}
    }
}
