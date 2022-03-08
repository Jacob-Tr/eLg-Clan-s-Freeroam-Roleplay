using RAGE;
using RAGE.Ui;

namespace SAMP_Dialogs.dialog
{
    public class Dialog : Events.Script
    {
        private HtmlWindow WindowDialog = null;
        private string dialog_name, dialog_caption, dialog_info, dialog_input, dialog_password_input;
        private string[] dialog_buttons;
        private string[] dialog_list_items;
		private int id = ushort.MaxValue;

        public Dialog()
        {
            RAGE.Ui.Cursor.Visible = false;
        }
		
		public bool doesPlayerHaveActiveDialog() {return WindowDialog != null;}
		public void destroyPlayerDialog() 
		{
			WindowDialog.Destroy();
			WindowDialog = null;
		}

        public Dialog(int dialogid, string dialog_name, string dialog_caption, string dialog_info, string[] dialog_buttons, string[] dialog_list_items = null, string dialog_input = null, string dialog_password_input = null)
        {
            this.dialog_name = dialog_name;
            this.dialog_caption = dialog_caption;
            this.dialog_info = dialog_info;
            this.dialog_buttons = dialog_buttons;
            this.dialog_list_items = dialog_list_items;
            this.dialog_input = dialog_input;
            this.dialog_password_input = dialog_password_input;

            if(WindowDialog != null)
            {
                WindowDialog.Destroy();
                WindowDialog = null;
            }
            WindowDialog = new HtmlWindow("package://html/SAMP_Dialogs/index.html");
            WindowDialog.Active = true;
            RAGE.Ui.Cursor.Visible = true;

            //buttons
            string buttons = "[";
            if(dialog_buttons != null)
            {
                buttons += "'" + dialog_buttons[0] + "'";
                for(byte i = 1; i < dialog_buttons.Length; i++)
                {
                    buttons += ", '" + dialog_buttons[i] + "'";
                }
            }
            buttons += "]";

            //list items
            string list_items = "[";
            if(dialog_list_items != null)
            {
                list_items += "'" + dialog_list_items[0] + "'";
                for(byte i = 1; i < dialog_list_items.Length; i++)
                {
                    list_items += ", '" + dialog_list_items[i] + "'";
                }
            }
            list_items += "]";
			
			RAGE.Events.CallRemote("SetPlayerDialogActive", dialogid);

            WindowDialog.ExecuteJs($"createDialog({dialogid}, '{dialog_name}', '{dialog_caption}', '{dialog_info}', {buttons}, {list_items}, '{dialog_input}', '{dialog_password_input}');");
        }

        public void Destroy()
        {
            if(WindowDialog != null)
            {
                WindowDialog.Destroy();
                WindowDialog = null;
                RAGE.Ui.Cursor.Visible = false;
            }
        }
    }
}
