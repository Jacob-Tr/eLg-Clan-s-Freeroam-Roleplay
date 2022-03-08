using System;
using RAGE;
using SAMP_Dialogs.dialog;

namespace SAMP_Dialogs.Test
{
    public class Test : Events.Script
    {
        private Dialog testDialog = null;
		private int id;
		private int type;
		private const int DIALOG_STYLE_INFO = 0, DIALOG_STYLE_LIST = 1, DIALOG_STYLE_INPUT = 2, DIALOG_STYLE_PASSWORD = 3, DIALOG_STYLE_TABLIST = 4; 
		
        public Test()
        {
            RAGE.Events.Add("DialogResponse", OnDialogResponse);
            RAGE.Events.Add("CreateDialog", CreateDialog);
			RAGE.Events.Add("DestroyDialog", destroyDialog);
        }
		
		public void setPlayerActiveDialog(short dialogID) {RAGE.Events.CallRemote("SetPlayerDialogActive", dialogID);}
		private void toggleChat(bool value)
		{
			RAGE.Chat.Activate(value);
			//RAGE.Chat.Show(value);
		}
        
        public void CreateDialog(object[] args)
        {
			if(this.testDialog != null) return;
			
			this.id = (int) args[0];
			this.type = (int) args[1];
			
			string name = args[2].ToString(), caption = args[3].ToString(), info = args[4].ToString();
			string buttonOne = args[5].ToString(), buttonTwo = args[6].ToString();
			
			string[] buttons = new string[2];
			if(buttonTwo == null) buttons = new string[] {buttonOne};
			else buttons = new string[] {buttonOne, buttonTwo};
			
            switch(type)
            {
                case DIALOG_STYLE_INFO:
				{
                    testDialog = new Dialog(id, name, caption, info, buttons, null, null, null);
                    break;
				}
                case DIALOG_STYLE_LIST:
				{
					if(info.Length == 0) return;
					
					bool[] breakAt = new bool[(byte) info.Length];
					byte breaks = 0;
					sbyte begin = 0;
					string[] newString = null;
					for(byte i = (byte) (info.Length - 1); i > 0; i--)
					{
						if(info[i] == '\t')
						{
							string suffix = null;
							suffix = ((i + 1) < info.Length) ? info.Substring((i + 1)) : "";
							info = $"{info.Substring(0, i)}&emsp;{suffix}";
						}
						
						/*if(i == (info.Length - 1)) 
						{
							string[] oldString = new string[0];
							if(newString != null) oldString = newString;
							newString = new string[oldString.Length + 1];
							
							oldString.CopyTo(newString, 0);
							
							if(begin != -1) newString[breaks] = info.Substring(begin);
						}*/
					}
					for(byte i = 0; i < info.Length; i++)
					{
						if(begin == (sbyte) -1) break;
						if(i <= begin) continue;
						
						if(info[i] == '\n' && i != (info.Length - 1))
						{
							breakAt[i] = true;
							
							if(i != (byte) 0)
							{
								string[] oldString = new string[0];
								if(newString != null) oldString = newString;
								newString = new string[oldString.Length + 1];
								
								oldString.CopyTo(newString, 0);
								
								if(i < info.Length) newString[breaks++] = info.Substring(begin, (i - begin));
								else info.Substring(begin);

								begin = ((i + 1) < info.Length) ? (sbyte) (i + 1) : (sbyte) -1;
							}
							continue;
						}
					}
					
                    testDialog = new Dialog(id, name, caption, null, buttons, newString, null, null);
                    break;
				}
                case DIALOG_STYLE_INPUT:
				{
                    testDialog = new Dialog(id, name, caption, info, buttons, null, " ", null);
                    break;
				}
                case DIALOG_STYLE_PASSWORD:
				{
                    testDialog = new Dialog(id, name, caption, info, buttons, null, null, " ");
                    break;
				}
				case DIALOG_STYLE_TABLIST:
				{
					if(info.Length == 0) return;
					
					bool[] breakAt = new bool[(byte) info.Length];
					byte breaks = 0;
					sbyte begin = 0;
					string[] newString = null;
					for(byte i = (byte) (info.Length - 1); i > 0; i--)
					{
						if(info[i] == '\t')
						{
							string suffix = null;
							suffix = ((i + 1) < info.Length) ? info.Substring((i + 1)) : "";
							info = $"{info.Substring(0, i)}&emsp;{suffix}";
						}
						
						/*if(i == (info.Length - 1)) 
						{
							string[] oldString = new string[0];
							if(newString != null) oldString = newString;
							newString = new string[oldString.Length + 1];
							
							oldString.CopyTo(newString, 0);
							
							if(begin != -1) newString[breaks] = info.Substring(begin);
						}*/
					}
					for(byte i = 0; i < info.Length; i++)
					{
						if(begin == (sbyte) -1) break;
						if(i <= begin) continue;
						
						if(info[i] == '\n' && i != (info.Length - 1))
						{
							breakAt[i] = true;
							
							if(i != (byte) 0)
							{
								string[] oldString = new string[0];
								if(newString != null) oldString = newString;
								newString = new string[oldString.Length + 1];
								
								oldString.CopyTo(newString, 0);
								
								if(i < info.Length) newString[breaks++] = info.Substring(begin, (i - begin));
								else info.Substring(begin);

								begin = ((i + 1) < info.Length) ? (sbyte) (i + 1) : (sbyte) -1;
							}
							continue;
						}
					}
					testDialog = new Dialog(id, name, caption, "", buttons, newString, null, null);
					break;
				}
            }
			
			toggleChat(false);
			RAGE.Events.CallRemote("TogglePlayerTyping", false);
        }
		
		public void destroyDialog(object[] args) 
		{
			if(testDialog != null) 
			{
				setPlayerActiveDialog(-1);
				
				testDialog.destroyPlayerDialog();
				testDialog = null;
				
				RAGE.Ui.Cursor.Visible = false;
				
				toggleChat(true);
			}
		}

        public void OnDialogResponse(object[] args)
        {
            /*
				args[0] = dialogid
				args[1] = type
                args[2] = dialog_name
                args[3] = response
                args[4] = listitem || inputtext
            */
			destroyDialog(new object[] {null});
			
            switch(this.type)
			{
				case DIALOG_STYLE_INFO:
				{
					RAGE.Events.CallRemote("OnDialogResponse", (int) args[0], (string) args[1], (int) args[2], (int) args[3]);
					break;
				}
				case DIALOG_STYLE_LIST:
				{
					RAGE.Events.CallRemote("OnDialogResponse", (int) args[0], (string) args[1], (int) args[2], (int) args[3]);
					break;
				}
				case DIALOG_STYLE_INPUT: case DIALOG_STYLE_PASSWORD:
				{
					RAGE.Events.CallRemote("OnDialogResponse", (int) args[0], (string) args[1], (int) args[2], (string) args[3]);
					break;
				}
			}
        }
    }
}
