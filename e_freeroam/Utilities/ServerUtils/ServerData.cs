using e_freeroam.Objects;
using e_freeroam.Utilities.PlayerUtils;
using GTANetworkAPI;
using System;
using System.Collections.Generic;
using Checkpoint2 = e_freeroam.Objects.Checkpoint2;

namespace e_freeroam.Utilities.ServerUtils
{
    public abstract class ServerData
    {
        public static Color COLOR_WHITE = new Color(0xFFFFFF);
        public static Color COLOR_YELLOW = new Color(0xFFFF00);
        public static Color COLOR_RED = new Color(0xFF0000);
        public static Color COLOR_LRED = new Color(0xFF4747);
        public static Color COLOR_GREEN = new Color(0x00FF00);
        public static Color COLOR_BLUE = new Color(0x0000FF);
        public static Color COLOR_LBLUE = new Color(0xADD8E6);
        public static Color COLOR_MEDIUM_BLUE = new Color(0x3399FF);
        public static Color COLOR_ORANGE = new Color(0xFF9900);
        public static Color COLOR_PURPLE = new Color(0xCC00DD);
        public static Color COLOR_PEACH = new Color(0xFFDAB9);
        public static Color COLOR_SILVER = new Color(0xC0C0C0);
        public static Color COLOR_WANTED_BLUE = new Color(0x0055CC);
        public static Color COLOR_GOLD = new Color(0xD4AF37);
        public static Color COLOR_DEEP_PINK = new Color(0xFF1493);
        public static Color COLOR_LIGHT_CYAN = new Color(0xE0FFFF);
        public static Color COLOR_LIGHT_PINK = new Color(0xFFB6C1);
        public static Color COLOR_SUPPORT = new Color(0x00E5EE);
        public static Color COLOR_ADMIN = new Color(0xCC0000);
        public static Color COLOR_UPPER_ADMIN = new Color(0x339933);
        public static Color COLOR_ADMIN_NOTES_LOG = new Color(0xBAFFFD);
        public static Color COLOR_FREEROAM = new Color(0xFFFF33);
        public static Color COLOR_ROLEPLAY = new Color(0xFFFFFF); // COLOR_WHITE
        public static Color COLOR_LISTEN = new Color(0xFFFF99);
        public static Color COLOR_VIPL1 = new Color(0xFFB7E6);
        public static Color COLOR_VIPL2 = new Color(0xFF84D5);
        public static Color COLOR_VIPL3 = new Color(0xFF51C3);
        public static Color COLOR_VIPL4 = new Color(0xFF1EB2);

        private static int[] keys =
        {
            0x01, // LEFT_MOUSE_BUTTON (#:0)
            0x02, // RIGHT_MOUSE_BUTTON (#:1)
            0x03, // CONTROL-BREAK_PROCESSING (#:2)
            0x04, // MIDDLE_MOUSE_BUTTON (#:3)
            0x05, // X1_MOUSE_BUTTON (#:4)
            0x06, // X2_MOUSE_BUTTON (#:5)
            0x07, // UNDEFINED (#:6)
            0x08, // BACKSPACE_KEY (#:7)
            0x09, // TAB_KEY (#:8)
            0x0A, // RESERVED (#:9)
            0x0C, // CLEAR_KEY (#:10)
            0x0D, // ENTER_KEY (#:11)
            0x0E, // UNDEFINED (#:12)
            0x10, // SHIFT_KEY (#:13)
            0x11, // CTRL_KEY (#:14)
            0x12, // ALT_KEY (#:15)
            0x13, // PAUSE_KEY (#:16)
            0x14, // CAPS_LOCK_KEY (#:17)
            0x15, // IME_KANA_MODE (#:18)
            0x15, // IME_HANGUEL_MODE (#:19)
            0x15, // IME_HANGUL_MODE (#:20)
            0x16, // IME_ON (#:21)
            0x17, // IME_JUNJA_MODE (#:22)
            0x18, // IME_FINAL_MODE (#:23)
            0x19, // IME_HANJA_MODE (#:24)
            0x19, // IME_KANJI_MODE (#:25)
            0x1A, // IME_OFF (#:26)
            0x1B, // ESC_KEY (#:27)
            0x1C, // IME_CONVERT (#:28)
            0x1D, // IME_NONCONVERT (#:29)
            0x1E, // IME_ACCEPT (#:30)
            0x1F, // IME_MODE_CHANGE_REQUEST (#:31)
            0x20, // SPACEBAR (#:32)
            0x21, // PAGE_UP_KEY (#:33)
            0x22, // PAGE_DOWN_KEY (#:34)
            0x23, // END_KEY (#:35)
            0x24, // HOME_KEY (#:36)
            0x25, // LEFT_ARROW_KEY (#:37)
            0x26, // UP_ARROW_KEY (#:38)
            0x27, // RIGHT_ARROW_KEY (#:39)
            0x28, // DOWN_ARROW_KEY (#:40)
            0x29, // SELECT_KEY (#:41)
            0x2A, // PRINT_KEY (#:42)
            0x2B, // EXECUTE_KEY (#:43)
            0x2C, // PRINT_SCREEN_KEY (#:44)
            0x2D, // INS_KEY (#:45)
            0x2E, // DEL_KEY (#:46)
            0x2F, // HELP_KEY (#:47)
            0x30, // 0_KEY (#:48)
            0x31, // 1_KEY (#:49)
            0x32, // 2_KEY (#:50)
            0x33, // 3_KEY (#:51)
            0x34, // 4_KEY (#:52)
            0x35, // 5_KEY (#:53)
            0x36, // 6_KEY (#:54)
            0x37, // 7_KEY (#:55)
            0x38, // 8_KEY (#:56)
            0x39, // 9_KEY (#:57)
            0x3A, // UNDEFINED (#:58)
            0x41, // A_KEY (#:59)
            0x42, // B_KEY (#:60)
            0x43, // C_KEY (#:61)
            0x44, // D_KEY (#:62)
            0x45, // E_KEY (#:63)
            0x46, // F_KEY (#:64)
            0x47, // G_KEY (#:65)
            0x48, // H_KEY (#:66)
            0x49, // I_KEY (#:67)
            0x4A, // J_KEY (#:68)
            0x4B, // K_KEY (#:69)
            0x4C, // L_KEY (#:70)
            0x4D, // M_KEY (#:71)
            0x4E, // N_KEY (#:72)
            0x4F, // O_KEY (#:73)
            0x50, // P_KEY (#:74)
            0x51, // Q_KEY (#:75)
            0x52, // R_KEY (#:76)
            0x53, // S_KEY (#:77)
            0x54, // T_KEY (#:78)
            0x55, // U_KEY (#:79)
            0x56, // V_KEY (#:80)
            0x57, // W_KEY (#:81)
            0x58, // X_KEY (#:82)
            0x59, // Y_KEY (#:83)
            0x5A, // Z_KEY (#:84)
            0x5B, // LEFT_WINDOWS_KEY (#:85)
            0x5C, // RIGHT_WINDOWS_KEY (#:86)
            0x5D, // APPLICATIONS_KEY (#:87)
            0x5E, // RESERVED (#:88)
            0x5F, // COMPUTER_SLEEP_KEY (#:89)
            0x60, // NUMERIC_KEYPAD_0_KEY (#:90)
            0x61, // NUMERIC_KEYPAD_1_KEY (#:91)
            0x62, // NUMERIC_KEYPAD_2_KEY (#:92)
            0x63, // NUMERIC_KEYPAD_3_KEY (#:93)
            0x64, // NUMERIC_KEYPAD_4_KEY (#:94)
            0x65, // NUMERIC_KEYPAD_5_KEY (#:95)
            0x66, // NUMERIC_KEYPAD_6_KEY (#:96)
            0x67, // NUMERIC_KEYPAD_7_KEY (#:97)
            0x68, // NUMERIC_KEYPAD_8_KEY (#:98)
            0x69, // NUMERIC_KEYPAD_9_KEY (#:99)
            0x6A, // MULTIPLY_KEY (#:100)
            0x6B, // ADD_KEY (#:101)
            0x6C, // SEPARATOR_KEY (#:102)
            0x6D, // SUBTRACT_KEY (#:103)
            0x6E, // DECIMAL_KEY (#:104)
            0x6F, // DIVIDE_KEY (#:105)
            0x70, // F1_KEY (#:106)
            0x71, // F2_KEY (#:107)
            0x72, // F3_KEY (#:108)
            0x73, // F4_KEY (#:109)
            0x74, // F5_KEY (#:110)
            0x75, // F6_KEY (#:111)
            0x76, // F7_KEY (#:112)
            0x77, // F8_KEY (#:113)
            0x78, // F9_KEY (#:114)
            0x79, // F10_KEY (#:115)
            0x7A, // F11_KEY (#:116)
            0x7B, // F12_KEY (#:117)
            0x7C, // F13_KEY (#:118)
            0x7D, // F14_KEY (#:119)
            0x7E, // F15_KEY (#:120)
            0x7F, // F16_KEY (#:121)
            0x80, // F17_KEY (#:122)
            0x81, // F18_KEY (#:123)
            0x82, // F19_KEY (#:124)
            0x83, // F20_KEY (#:125)
            0x84, // F21_KEY (#:126)
            0x85, // F22_KEY (#:127)
            0x86, // F23_KEY (#:128)
            0x87, // F24_KEY (#:129)
            0x88, // UNASSIGNED (#:130)
            0x90, // NUM_LOCK_KEY (#:131)
            0x91, // SCROLL_LOCK_KEY (#:132)
            0x92, // OEM_SPECIFIC (#:133)
            0x97, // UNASSIGNED (#:134)
            0xA0, // LEFT_SHIFT_KEY (#:135)
            0xA1, // RIGHT_SHIFT_KEY (#:136)
            0xA2, // LEFT_CONTROL_KEY (#:137)
            0xA3, // RIGHT_CONTROL_KEY (#:138)
            0xA4, // LEFT_MENU_KEY (#:139)
            0xA5, // RIGHT_MENU_KEY (#:140)
            0xA6, // BROWSER_BACK_KEY (#:141)
            0xA7, // BROWSER_FORWARD_KEY (#:142)
            0xA8, // BROWSER_REFRESH_KEY (#:143)
            0xA9, // BROWSER_STOP_KEY (#:144)
            0xAA, // BROWSER_SEARCH_KEY (#:145)
            0xAB, // BROWSER_FAVORITES_KEY (#:146)
            0xAC, // BROWSER_START_AND_HOME_KEY (#:147)
            0xAD, // VOLUME_MUTE_KEY (#:148)
            0xAE, // VOLUME_DOWN_KEY (#:149)
            0xAF, // VOLUME_UP_KEY (#:150)
            0xB0, // NEXT_TRACK_KEY (#:151)
            0xB1, // PREVIOUS_TRACK_KEY (#:152)
            0xB2, // STOP_MEDIA_KEY (#:153)
            0xB3, // PLAY/PAUSE_MEDIA_KEY (#:154)
            0xB4, // START_MAIL_KEY (#:155)
            0xB5, // SELECT_MEDIA_KEY (#:156)
            0xB6, // START_APPLICATION_1_KEY (#:157)
            0xB7, // START_APPLICATION_2_KEY (#:158)
            0xB8, // RESERVED (#:159)
            0xBA, // ;:_KEY (#:160)
            0xBB, // +_KEY (#:161)
            0xBC, // ,_KEY (#:162)
            0xBD, // -_KEY (#:163)
            0xBE, // ._KEY (#:164)
            0xBF, // /?_KEY (#:165)
            0xC0, // `~_KEY (#:166)
            0xC1, // RESERVED (#:167)
            0xD8, // UNASSIGNED (#:168)
            0xDB, // [{_KEY (#:169)
            0xDC, // \|_KEY (#:170)
            0xDD, // ]}_KEY (#:171)
            0xDE, // SINGLE-QUOTE/DOUBLE-QUOTE_KEY (#:172)
            0xDF, // MISCELLANEOUS_CHARACTERS_KEY (#:173)
            0xE0, // RESERVED (#:174)
            0xE1, // OEM_SPECIFIC (#:175)
            0xE2, // ANGLE_BRACKET_KEY (#:176)
            0xE3, // OEM_SPECIFIC (#:177)
            0xE5, // IME_PROCESS_KEY (#:178)
            0xE6, // OEM_SPECIFIC (#:179)
            0xE7, // VK_PACKET_KEY (#:180)
            0xE8, // UNASSIGNED (#:181)
            0xE9, // OEM_SPECIFIC (#:182)
            0xF6, // ATTN_KEY (#:183)
            0xF7, // CRSEL_KEY (#:184)
            0xF8, // EXSEL_KEY (#:185)
            0xF9, // ERASE_EOF_KEY (#:186)
            0xFA, // PLAY_KEY (#:187)
            0xFB, // ZOOM_KEY (#:188)
            0xFC, // RESERVED (#:189)
            0xFD, // PA1_KEY (#:190)
            0xFE // CLEAR_KEY (#:191)
        };

        public const ushort maxKeys = 100, maxVehicles = 1000, maxOrgs = 12, maxCPs = 1000;
        private static ushort vehicles = 0, serverVehicleCount = 0; // vehicles: Total of all combined vehicle types, serverVehicleCount: File loaded static vehicles.
        private static ushort cpCount = 0;

        private static int orgCount = 0, worldTime = 0;

        private static string serverDir = "scriptfiles/";

        private static Dictionary<string, string> serverData = null;

        private static List<Vehicle2> serverVehicles = new List<Vehicle2>(maxVehicles);
        private static List<Organization> orgList = new List<Organization>(maxOrgs);
        private static List<Checkpoint2> cpList = new List<Checkpoint2>(maxCPs);

        private static FileHandler vehicleHandler = null;
        private static FileHandler serverDataHandler = null;

        public static int getKeyValue(Utilities.ServerUtils.KeyRef refID) {return keys[(int) refID];}

        public static List<Vehicle2> getVehiclePool() {return serverVehicles;}
        public static List<Organization> getOrgPool() {return orgList;}
        public static List<Checkpoint2> getCPPool() {return cpList;}

        public static void loadServerData()
        {
            serverDataHandler = new FileHandler(FileTypes.SERVER, getDefaultServerDir() + "ServerData", "Server");

            if(!serverDataHandler.loadFile())
            {
                serverDataHandler.addValue(ServerUtils.ServerDataInfo.WRLDTIME.ToString(), "0");
                serverDataHandler.addValue(ServerUtils.ServerDataInfo.ORGS.ToString(), "0");

                serverDataHandler.saveFile();
                serverDataHandler.loadFile();
                return;
            }

            serverData = serverDataHandler.getInfo();
            string value = null;

            serverData.TryGetValue(ServerUtils.ServerDataInfo.WRLDTIME.ToString(), out value);
            if(serverData.ContainsKey(value)) worldTime = NumberUtils.parseInt(value, (byte) value.Length);

            for(sbyte i = 0; i < maxOrgs; i++)
            {
                orgList.Insert(i, new Organization(i));

                if(orgList[i].doesOrgExist()) orgCount++;
                else orgList.Insert(i, null);
            }

            string orgKey = ServerUtils.ServerDataInfo.ORGS.ToString();

            if(serverData.ContainsKey(orgKey)) serverData.Remove(orgKey);
            serverData.Add(orgKey, $"{orgCount}");

            Console.WriteLine($"{orgCount} organizations loaded.");
            return;
        }

        public static sbyte addOrg(string name, Color color, Player creatingPlayer)
        {
            if((orgCount + 1) >= maxOrgs) return -1;

            sbyte orgID = ((sbyte) -1);
            for(sbyte i = 0; i < maxOrgs; i++)
            {
                if(!orgList[i].doesOrgExist())
                {
                    orgID = i;
                    break;
                }
            }
            Organization newOrg = new Organization(orgID, creatingPlayer, name);
			newOrg.setColor(color);

            orgList.Insert(orgID, newOrg);

            int count = 0;
            foreach(Organization org in orgList) {if(org != null) count++;}

            orgCount = count;

            return orgID;
        }
        public static Organization getOrg(ushort id) {return orgList[id];}

        public static Checkpoint2 addCP(Vector3 location, CPType cpType=CPType.NULL)
        {
            if((cpCount + 1) > maxCPs) return null;
            cpCount++;

            Checkpoint2 newCP = new Checkpoint2(location, cpType);
            ushort id = newCP.getID();

            cpList.Insert(id, newCP);
            return newCP;
        }
        public static Checkpoint2 getCP(ushort id) {return cpList[id];}
        public static void removeCP(ushort id) 
        {
            Checkpoint2 cp = cpList[id];
            cpList[id] = null;
            cp.getCheckpoint().Delete();
        }

        public static FileHandler getVehicleHandler() {return vehicleHandler;}
        public static void loadVehicles()
        {
            vehicleHandler = new FileHandler(FileTypes.VEHICLE, getDefaultServerDir() + "ServerData", "Vehicles");
            if(!vehicleHandler.loadFile())
            {
                vehicleHandler.saveFile();
                return;
            }

            Dictionary<string, string> vehicleDir = vehicleHandler.getInfo();

            string key = null, value = null;
            string model = null, x = null, y = null, z = null, rotX = null, rotY = null, rotZ = null;

            int index = 0, nextIndex = 0;
            uint MODEL = 0;

            float X = 0.0F, Y = 0.0F, Z = 0.0F, ROT_X = 0.0F, ROT_Y = 0.0F, ROT_Z = 0.0F;

            for(int i = 0; i < maxVehicles; i++)
            {
                key = ("VEH#" + i);
                if(vehicleDir.ContainsKey(key))
                {
                    vehicleDir.TryGetValue(key, out value);

                    index = value.IndexOf(' ');
                    model = value.Substring(0, index++);

                    nextIndex = value.IndexOf(' ', index) ;
                    x = value.Substring(index, (nextIndex++ - index));
                    index = nextIndex;

                    nextIndex = value.IndexOf(' ', index);
                    y = value.Substring(index, (nextIndex++ - index));
                    index = nextIndex;

                    nextIndex = value.IndexOf(' ', index);
                    z = value.Substring(index, (nextIndex++ - index));
                    index = nextIndex;

                    nextIndex = value.IndexOf(' ', index);
                    rotX = value.Substring(index, (nextIndex++ - index));
                    index = nextIndex;

                    nextIndex = value.IndexOf(' ', index);
                    rotY = value.Substring(index, (nextIndex++ - index));
                    index = nextIndex;

                    rotZ = value.Substring(index);

                    MODEL = ((uint)NumberUtils.parseInt(model, (byte) model.Length));
                    X = NumberUtils.parseFloat(x, (byte) x.Length);
                    Y = NumberUtils.parseFloat(y, (byte) y.Length);
                    Z = NumberUtils.parseFloat(z, (byte) z.Length);
                    ROT_X = NumberUtils.parseFloat(rotX, (byte) rotX.Length);
                    ROT_Y = NumberUtils.parseFloat(rotY, (byte) rotY.Length);
                    ROT_Z = NumberUtils.parseFloat(rotZ, (byte) rotZ.Length);

                    Vector3 vect = new Vector3(X, Y, Z);
                    Vector3 angle = new Vector3(ROT_X, ROT_Y, ROT_Z);

                    serverVehicleCount++;
                    addVehicle(MODEL, vect, angle, -1, -1, VehicleType.SERVER_VEHICLE);
                }
            }
            Console.WriteLine($"{serverVehicleCount} vehicles loaded.");
        }

        public static void saveVehicles() {vehicleHandler.saveFile();}

        public static bool addVehicleToServer(Vehicle2 vehicle)
        {
            if((vehicles + 1) >= maxVehicles) return false;

            Vehicle veh = vehicle.getVehicle();
            string model = veh.Model.ToString();
            string line = $"{model} {veh.Position.X} {veh.Position.Y} {veh.Position.Z} {veh.Rotation.X} {veh.Rotation.Y} {veh.Rotation.Z}";

            vehicleHandler.addValue("VEH#" + ++serverVehicleCount, line);
            return true;
        }

        public static int addVehicle(uint hashKey, Vector3 vect, Vector3 rot, int color1=-1, int color2=-1, VehicleType type=VehicleType.CMD_VEHICLE) 
        {
            if((vehicles + 1) >= maxVehicles) return -1;
            vehicles++;

            Vehicle newVehicle = NAPI.Vehicle.CreateVehicle(hashKey, vect, rot.Z, color1, color2);
            NAPI.Vehicle.SpawnVehicle(newVehicle, vect, rot.Z);

            Vehicle2 vehicle = new Vehicle2(newVehicle, type);
            serverVehicles.Insert(newVehicle.Id, vehicle);

            Console.WriteLine($"addV ~ Mod: {hashKey} X: {vect.X} Y: {vect.Y} Z: {vect.Z} Rot: {rot.X} + {rot.Y} + {rot.Z}");

            return newVehicle.Id;
        }
        public static void removeVehicle(int vehicleid) 
        {
            Vehicle2 vehicle = serverVehicles[vehicleid];
            vehicle.getVehicle().Delete();

            serverVehicles.RemoveAt(vehicleid);
        }
        public static int getVehicleID(Vehicle2 vehicle) {return vehicle.getVehicle().Id;}

        public static Vehicle2 getVehicleObject(Vehicle vehicle)
        {
            foreach(Vehicle2 veh in serverVehicles) if(veh.getVehicle().Equals(vehicle)) return veh;
            return null;
        }

        public static string getDefaultServerDir() {return serverDir;}

        public static bool commandCheck(Player player, out string output, ushort adminLevel=0, string[] parameters=null, byte size=((byte) 0))
        {
            PlayerData data = PlayerDataInfo.getPlayerData(player);
            bool failed = false;
            output = null;

            if(data.getPlayerAdminLevel() < adminLevel)
            {
                output = ChatUtils.colorString("ERROR: Command not found.", ChatUtils.getColorAsHex(ServerData.COLOR_WHITE));
                failed = true;
            }
            if(!failed && !data.isPlayerRegistered())
            {
                output = ChatUtils.colorString("You must register to play! Type /register to continue.", ChatUtils.getColorAsHex(ServerData.COLOR_RED));
                failed = true;
            }
            if(!failed && !data.isPlayerLoggedIn())
            {
                output = ChatUtils.colorString("You must login to play! Type /login to continue.", ChatUtils.getColorAsHex(ServerData.COLOR_RED));
                failed = true;
            }
			if((!failed && parameters != null) && !parameterCheck(parameters, size))
			{
				output = $"Usage: /{parameters[0]}";
				for(byte i = 1; i <= (size - 1) / 2; i++) output = $"{output} [{parameters[i]}]";
				output = ChatUtils.colorString(output, ChatUtils.getColorAsHex(ServerData.COLOR_RED));
				failed = true;
			}

            return failed;
        }

		private static bool parameterCheck(string[] checkParams, byte size)
		{
			for(byte i = 0; i < size; i++) if(checkParams[i] == "NullCMDStr") return false;
			return true;
		}

		public static short getTargetPlayerID(string target)
		{
			short playerid = -1;

			foreach(Player player in NAPI.Pools.GetAllPlayers()) if(player.Name.Equals(target)) playerid = ((short) player.Value);
			if(playerid == -1 && NumberUtils.isNumeric(target, (byte) target.Length)) playerid = NumberUtils.parseShort(target, (byte) target.Length);

			return playerid;
		}
    }
}
