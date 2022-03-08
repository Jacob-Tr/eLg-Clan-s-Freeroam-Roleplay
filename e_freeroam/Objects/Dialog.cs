using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;

namespace e_freeroam.Objects
{
	public class Dialog
	{
		private static ushort maxDialogs = (ushort.MaxValue - 1);
		private static byte maxDialogListItems = 24;

		private int id = ushort.MaxValue;
		private DialogType type = 0;
		private string name = null, caption = null, info = null;
		private string[] buttons = null;

		public Dialog(DialogID dialogid, DialogType dialogType, string dialogName="", string dialogCaption="", string dialogInfo="", string[] dialogButtons=null)
		{
			this.id = (int) dialogid;
			this.type = dialogType;
			this.name = dialogName;
			this.caption = dialogCaption;
			this.info = dialogInfo;
			this.buttons = dialogButtons;

			if(this.buttons == null) this.buttons = new string[] {"", null};
			if(this.buttons.Length == 1) this.buttons = new string[] {buttons[0], null};
		}

		// Static dialog properties

		public static ushort getMaxDialogs() {return maxDialogs;}

		public static byte getMaxListItems() {return maxDialogListItems;}
		
		// General dialog utilities

		public ushort getDialogID() {return (ushort) this.id;}

		// Player related

		public void destroyPlayerDialog(Player player) {NAPI.ClientEventThreadSafe.TriggerClientEvent(player.Handle, "DestroyDialog");}

		public void showDialogForPlayer(Player player) {NAPI.ClientEventThreadSafe.TriggerClientEvent(player.Handle, "CreateDialog", this.id, (int) this.type, this.name, this.caption, this.info, this.buttons[0], this.buttons[1]);}

		// Dialog information

		public string getNameText() {return this.name;}
		public void setNameText(string newText) {this.name = newText;}
		public string getCaption() {return this.caption;}
		public void setCaption(string newText) {this.caption = newText;}

		public string getButtonText(byte index) {return this.buttons[index];}
		public void setButtonText(byte buttonID, string newText) 
		{
			if(buttonID >= this.buttons.Length) return;
			this.buttons[buttonID] = newText;
		}

		public string getInfo() {return this.info;}
		public void setInfo(string dialogInfo) {this.info = dialogInfo;}

		public void addListItem(string item) {this.info = $"{this.getInfo()}\n{item}";}

		public byte getListItemCount() {return (byte) this.info.Length;}
	}
}
