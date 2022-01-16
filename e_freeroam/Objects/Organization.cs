using e_freeroam.Utilities;
using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;

namespace e_freeroam.Objects
{
    public class Organization
    {
        private FileHandler orgHandler = null;
        private string name = null;
        private Vector3 cpLocation = null;
        private Color color;
        private int orgID = -1, cpID = -1;
        private bool exists = false;

        public Organization(int id, Player beingCreated=null, string orgName=null)
        {
            orgHandler = new FileHandler(FileTypes.ORG, ServerData.getDefaultServerDir() + "Organizations", $"ORG{id}");

            this.exists = orgHandler.loadFile();
            if(!this.exists && beingCreated == null) return;
            if (this.exists)
            {
                this.orgID = id;

                string value = null;

                value = orgHandler.getValue(Utilities.ServerUtils.orgInfo.NAME.ToString());
                this.name = value;

                float x, y, z;
                value = orgHandler.getValue(Utilities.ServerUtils.orgInfo.X.ToString());
                x = NumberUtils.parseInt(value, value.Length);
                value = orgHandler.getValue(Utilities.ServerUtils.orgInfo.Y.ToString());
                y = NumberUtils.parseInt(value, value.Length);
                value = orgHandler.getValue(Utilities.ServerUtils.orgInfo.Z.ToString());
                z = NumberUtils.parseInt(value, value.Length);

                this.cpLocation = new Vector3(x, y, z);

                value = orgHandler.getValue(Utilities.ServerUtils.orgInfo.COLOR.ToString());
                this.color = ChatUtils.getHexAsColor(value);
            }
            else if(beingCreated == null) return;
            else
            {
                if(orgName == null) orgName = "UNNAMED ORG";
                this.name = orgName;

                this.cpLocation = beingCreated.Position;

                this.exists = true;
            }

            this.cpID = ServerData.addCP(cpLocation, CPType.ORG);
        }

        public bool doesOrgExist() {return this.exists;}

        public int getID() {return this.orgID;}

        public void setColor(Color newColor) {this.color = newColor;}

        public Checkpoint getCheckpoint() {return ServerData.getCP(this.cpID);}
    }
}
