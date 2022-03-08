using e_freeroam.Objects;
using e_freeroam.Server_Properties;
using e_freeroam.Utilities.PlayerUtils;
using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public const ushort maxKeys = 100;
        private static ushort vehicles = 0, serverVehicleCount = 0; // vehicles: Total of all combined vehicle types, serverVehicleCount: File loaded static vehicles.

        private static uint worldTime = 0, accounts = 0, banned_ips = 0;
		private static ushort businesses = 0, properties = 0;
		private static byte orgs = 0;

		private static bool serverDataListsInitialized = false, debugEnabled = false;

        private static string serverDir = "scriptfiles/";

        private static List<Vehicle2> serverVehicles = null;
        private static List<Organization> orgList = null;
		private static List<GasStation> gasList = null;
		private static List<Store> storeList = null;
		private static List<Business> businessList = null;
		private static List<House> propertyList = null;
        private static List<Checkpoint2> cpList = null;
		private static List<EnterExit> enExList = null;

        private static FileHandler vehicleHandler = null;
        private static FileHandler serverDataHandler = null;


		// General Server Utilities

		public static bool isAnyoneOnline() {return (PlayerDataInfo.getPlayersOnline() != 0);}

		public static bool haveServerDataListsBeenInitialized() {return serverDataListsInitialized;}

		public static string getRealServerTime() {return DateTime.Now.ToString();}

        public static int getKeyValue(Utilities.ServerUtils.KeyRef refID) {return keys[(int) refID];}

		public static void logToConsole(string text) {if(debugEnabled) NAPI.Util.ConsoleOutput($"[{((text.Length != 0) ? getRealServerTime() : "")}] {text}");}

		public static void sendServerFatalError(string text) 
		{
			bool debug = debugEnabled;
			debugEnabled = true;
			logToConsole($"Fatal Error: <{text}>");
			debugEnabled = debug;
		}

		public static void toggleServerDebug(bool value) {debugEnabled = value;}

		public static object getColShapeOwner(ColShape shape) // Should probably make an object 'ColShape2' to store such info.
		{
			logToConsole("getColShapeOwner called.");
			if(shape == null) return null;
			foreach(Business bus in getBusinessPool()) 
			{
				if(bus == null) continue;
				if(bus.getBusCP().getColShape() == shape) return bus;
			}
			logToConsole("Bus is fine");
			foreach(Store store in getStorePool())
			{
				if(store == null) continue;
				if(store.getStoreCP().getColShape() == shape) return store;
			}
			logToConsole("Store is fine");
			foreach(GasStation gas in getGasPool())
			{
				if(gas == null) continue;
				if(gas.getGasStationCP().getColShape() == shape) return gas;
			}
			logToConsole("Gas is fine");
			foreach(Organization org in getOrgPool())
			{
				if(org == null) continue;
				if(org.getCheckpoint2().getColShape() == shape) return org;
			}
			foreach(EnterExit enEx in getEnExPool())
			{
				if(enEx == null) continue;
				if(enEx.getEnExColshape() == shape) return enEx;
			}
			return null;
		}

		public static void saveServerData() 
		{
			serverDataHandler.addValue(ServerDataInfo.ACCOUNTS.ToString(), $"{accounts}");
			serverDataHandler.addValue(ServerDataInfo.IPS_BANNED.ToString(), $"{banned_ips}");
			serverDataHandler.addValue(ServerDataInfo.WRLDTIME.ToString(), $"{(int) NAPI.World.GetTime().TotalSeconds}");
            serverDataHandler.addValue(ServerDataInfo.ORGS.ToString(), $"{orgs}");
			serverDataHandler.addValue(ServerDataInfo.BUSES.ToString(), $"{businesses}");
			serverDataHandler.addValue(ServerDataInfo.PROPS.ToString(), $"{properties}");

			serverDataHandler.saveFile();
		}

		private static void setWorldTime()
		{
			short hours = 0;
			byte minutes = 0, seconds = 0;
			NumberUtils.processTime(worldTime, ref hours, ref minutes, ref seconds);

			NAPI.World.SetTime(hours, minutes, seconds);

			Console.WriteLine($"~ World time set to: {hours}:{minutes}:{seconds}");
		}

		public static uint getAccountCount() {return accounts;}
		public static void updateAccountCount(uint value) 
		{
			accounts = value;
			serverDataHandler.addValue(ServerDataInfo.ACCOUNTS.ToString(), $"{accounts}");

			serverDataHandler.saveFile();
		}

		public static string getDefaultServerDir() {return serverDir;}

		//* Retrieve server data pools

        public static List<Vehicle2> getVehiclePool() {return serverVehicles.ToList<Vehicle2>();}
        public static List<Organization> getOrgPool() {return orgList.ToList<Organization>();}
		public static List<Business> getBusinessPool() {return businessList.ToList<Business>();}
		public static List<Store> getStorePool() {return storeList.ToList<Store>();}
		public static List<House> getPropertyPool() {return propertyList.ToList<House>();}
		public static List<GasStation> getGasPool() {return gasList.ToList<GasStation>();}
        public static List<Checkpoint2> getCPPool() {return cpList.ToList<Checkpoint2>();}
		public static List<EnterExit> getEnExPool() {return enExList.ToList<EnterExit>();}

		//* (Un)banning

		public static uint getBannedIPCount() {return banned_ips;}
		public static void updateBannedIPCount(uint value) 
		{
			banned_ips = value;
			serverDataHandler.addValue(ServerDataInfo.IPS_BANNED.ToString(), $"{banned_ips}");

			saveServerData();
		}

		public static void banIP(string ip)
		{
			FileHandler ipHandler = new FileHandler(FileTypes.BAN, $"{getDefaultServerDir()}ServerData", "BannedIPs");
			if(!ipHandler.loadFile())
			{
				updateBannedIPCount(0);

				ipHandler.saveFile();
				ipHandler.loadFile();
			}
			
			updateBannedIPCount(getBannedIPCount() + 1);

			ipHandler.addValue($"IP{getBannedIPCount()}", ip);
			ipHandler.saveFile();

			ipHandler = null;
		}
		public static bool isIPBanned(string ip)
		{
			FileHandler ipHandler = new FileHandler(FileTypes.BAN, $"{getDefaultServerDir()}ServerData", "BannedIPs");
			if(!ipHandler.loadFile()) return false;

			bool check = ipHandler.containsValue(ip);
			ipHandler = null;

			return check;
		}
		public static void unbanIP(string ip)
		{
			FileHandler ipHandler = new FileHandler(FileTypes.BAN, $"{getDefaultServerDir()}ServerData", "BannedIPs");
			if(!ipHandler.loadFile()) return;

			string key = ipHandler.getKey(ip);
			ipHandler.removeValue(key);

			ipHandler.saveFile();
			ipHandler = null;
		}

		public static void unbanPlayer(string name)
		{
			FileHandler unbanHandle = new FileHandler(FileTypes.PLAYER, $"{ServerData.getDefaultServerDir()}PlayerData/{name[0]}", $"{name}");
			if(!unbanHandle.loadFile()) return;

			unbanHandle.addValue(PlayerInfo.BANNED.ToString(), $"{0}");
			unbanHandle.saveFile();

			unbanHandle = null;
			return;
		}

		// Server Data Initialization

		public static void initializeServerDataLists()
		{
			PlayerDataInfo.initializePlayerList();

			serverVehicles = ObjectUtils.generateListOfNull(Vehicle2.getMaxVehicles()).Cast<Vehicle2>().ToList<Vehicle2>();
			orgList = ObjectUtils.generateListOfNull(Organization.getMaxOrgs()).Cast<Organization>().ToList<Organization>();
			gasList = ObjectUtils.generateListOfNull(GasStation.getMaxGasStations()).Cast<GasStation>().ToList<GasStation>();
			storeList = ObjectUtils.generateListOfNull(Store.getMaxStores()).Cast<Store>().ToList<Store>();
			businessList = ObjectUtils.generateListOfNull(Business.getMaxBusinesses()).Cast<Business>().ToList<Business>();
			propertyList = ObjectUtils.generateListOfNull(House.getMaxHouses()).Cast<House>().ToList<House>();
			cpList = ObjectUtils.generateListOfNull(Checkpoint2.getMaxCheckpoints()).Cast<Checkpoint2>().ToList<Checkpoint2>();
			enExList = ObjectUtils.generateListOfNull(EnterExit.getMaxEnterExits()).Cast<EnterExit>().ToList<EnterExit>();

			serverDataListsInitialized = true;
		}

		private static void createServerDataFile()
		{
			if(serverDataHandler == null) return;

			serverDataHandler.addValue(ServerDataInfo.ACCOUNTS.ToString(), "0");
			serverDataHandler.addValue(ServerDataInfo.IPS_BANNED.ToString(), "0");
            serverDataHandler.addValue(ServerDataInfo.WRLDTIME.ToString(), "0");
            serverDataHandler.addValue(ServerDataInfo.ORGS.ToString(), "0");
			serverDataHandler.addValue(ServerDataInfo.BUSES.ToString(), "0");
			serverDataHandler.addValue(ServerDataInfo.PROPS.ToString(), "0");

            serverDataHandler.saveFile();
            serverDataHandler.loadFile();
		}

        public static void loadServerData()
        {
            serverDataHandler = new FileHandler(FileTypes.SERVER, getDefaultServerDir() + "ServerData", "Server");

            if(!serverDataHandler.loadFile()) createServerDataFile();

			bool failed = false;

			accounts = NumberUtils.parseUnsignedInt(serverDataHandler.getValue(ServerDataInfo.ACCOUNTS.ToString(), true, "0"), ref failed);
			banned_ips = NumberUtils.parseUnsignedInt(serverDataHandler.getValue(ServerDataInfo.IPS_BANNED.ToString(), true, "0"), ref failed);
			worldTime = NumberUtils.parseUnsignedInt(serverDataHandler.getValue(ServerDataInfo.WRLDTIME.ToString(), true, "0"), ref failed);
			orgs = NumberUtils.parseByte(serverDataHandler.getValue(ServerDataInfo.ORGS.ToString(), true, "0"), ref failed);
			businesses = NumberUtils.parseUnsignedShort(serverDataHandler.getValue(ServerDataInfo.BUSES.ToString(), true, "0"), ref failed);
			properties = NumberUtils.parseUnsignedShort(serverDataHandler.getValue(ServerDataInfo.PROPS.ToString(), true, "0"), ref failed);

			loadEnExs(); // Load EnterExits first because they need to exist in-order to be linked.
			loadOrganizations();
			loadBusinesses();
			loadGasStations();
			loadProperties();
			loadStores();

			updateServerCounts();

			setWorldTime();

			logToConsole("Finished Loading Server Data");
            return;
		}

		public static void updateServerCounts()
		{
			orgs = Organization.getOrgCount();
			properties = House.getPropertyCount();
			businesses = Business.getBusCount();
		}

		// EnterExit related

		public static void saveEnExs()
		{
			FileHandler enExHandler = EnterExit.getEnExHandler();

			foreach(EnterExit enEx in getEnExPool())
			{
				if(enEx == null) continue;
				ushort id = enEx.getEnExID();
				Vector3 pos = enEx.getEnExPos();
				float x = pos.X, y = pos.Y, z = pos.Z;
				uint world = enEx.getEnExWorld();

				ushort linkedID = ushort.MaxValue;
				if(enEx.getLinkedEnex() != null) linkedID = enEx.getLinkedEnex().getEnExID();

				string enExStr = $"{x} {y} {z} {linkedID} {world}";
				enExHandler.addValue($"EnEx{id}", enExStr);
			}

			enExHandler.saveFile();
		}

		public static void loadEnExs()
		{
			FileHandler enExHandler = EnterExit.getEnExHandler();
			if(!enExHandler.loadFile()) return;

			string key = null;
			ushort count = 0;
			byte index = 0;
			bool failed = false;

			for(ushort i = 0; i < EnterExit.getMaxEnterExits(); i++)
			{
				key = $"EnEx{i}";
				if(!enExHandler.containsKey(key)) continue;
				string enExStr = enExHandler.getValue(key);

				float x = NumberUtils.parseFloat(ChatUtils.strTok(enExStr, ref index), ref failed), y = NumberUtils.parseFloat(ChatUtils.strTok(enExStr, ref index), ref failed), z = NumberUtils.parseFloat(ChatUtils.strTok(enExStr, ref index), ref failed);
				ushort linkedID = NumberUtils.parseUnsignedShort(ChatUtils.strTok(enExStr, ref index), ref failed);
				uint world = NumberUtils.parseUnsignedInt(ChatUtils.strTok(enExStr, ref index), ref failed);


				EnterExit enEx = new EnterExit(new Vector3(x, y, z), world, linkedID, i);
				addEnEx(enEx, i);

				count++;
				index = 0;
			}

			EnterExit.updateEnExCount(count);
		}

		public static void addEnEx(EnterExit enEx, ushort id=ushort.MaxValue)
		{
			if(enEx != null)
			{
				if(id == ushort.MaxValue) id = enEx.getEnExID();
				if(getEnEx(id) != null) removeEnEx(id);
			}

			if(id >= EnterExit.getMaxEnterExits())
			{
				logToConsole($"Attempt to add EnterExit {id}/{EnterExit.getMaxEnterExits()} failed.");
				return;
			}

			enExList.Insert(id, enEx);
			EnterExit.updateEnExCount((ushort) (EnterExit.getEnExCount() + 1));
		}

		public static void removeEnEx(ushort id)
		{
			EnterExit.getEnExHandler().removeValue($"EnEx{id}");

			List<object> obj = ObjectUtils.removeObjectFromList(businessList.Cast<object>().ToList<object>(), id, byte.MaxValue, EnterExit.getMaxEnterExits());
			enExList = (List<EnterExit>) obj.Cast<EnterExit>().ToList<EnterExit>();

			logToConsole($"Business #{id} was removed.");
			EnterExit.updateEnExCount((ushort) (EnterExit.getEnExCount() - 1));

			saveEnExs();
		}

		public static EnterExit getEnEx(ushort id) {return enExList[id];}

		// Business related

		public static void loadBusinesses()
		{
			for(byte i = 0; i < Business.getMaxBusinesses(); i++)
			{
				bool failed = false;
				Business bus = new Business(ref failed, newID: i);
				if(failed) bus = null;

				addBusiness(bus, i);
			}

			logToConsole($"{Business.getBusCount()} businesses loaded.");
		}

		public static void addBusiness(Business bus, byte id=byte.MaxValue)
		{
			string businessName = "";
			if(bus != null) 
			{ 
				businessName = bus.getBusinessName();
				logToConsole($"Adding business: {businessName} in ID slot {((id == byte.MaxValue) ? bus.getBusinessID() : id)}.");
				if(id == byte.MaxValue) id = bus.getBusinessID();

				if(getBusiness(id) != null) removeBus(id);
			}

			if(id >= Business.getMaxBusinesses())
			{
				logToConsole($"Attempt to add business {id}/{Business.getMaxBusinesses()} failed.");
				return;
			}

			businessList.Insert(id, bus);
			Business.updateBusCount((byte) (Business.getBusCount() + 1));
		}
		public static void removeBus(byte id)
		{
			getBusiness(id).deleteBusiness();

			List<object> obj = ObjectUtils.removeObjectFromList(businessList.Cast<object>().ToList<object>(), id, byte.MaxValue, Business.getMaxBusinesses());
			businessList = (List<Business>) obj.Cast<Business>().ToList<Business>();

			logToConsole($"Business #{id} was removed.");
			Business.updateBusCount((byte) (Business.getBusCount() - 1));
		}

		public static Business getBusiness(byte id) {return businessList[id];}

		// Property(house) related

		public static void saveProperties()
		{
			foreach(House prop in getPropertyPool())
			{
				if(prop == null || prop.getPropertyHandler() == null) continue;
				prop.saveProperty();
			}
		}

		public static void loadProperties()
		{
			House prop = null;
			bool failed = false;

			for(byte i = 0; i < House.getMaxHouses(); i++)
			{
				prop = new House(ref failed, newID: i);
				if(!failed) addProperty(prop, i);
				failed = false;
			}
		}

		public static void addProperty(House prop, byte id=byte.MaxValue)
		{
			if(prop != null)
			{
				if(id == byte.MaxValue) id = prop.getPropertyID();

				if(propertyList[id] != null) removeProperty(id);
			}

			if(id >= House.getMaxHouses())
			{
				logToConsole($"Attempt to add property {id}/{House.getMaxHouses()} failed.");
				return;
			}

			propertyList.Insert(id, prop);
			House.updatePropertyCount((byte) (House.getPropertyCount() + 1));
		}

		public static void removeProperty(byte id)
		{
			getProperty(id).getPropertyHandler().deleteFile();

			List<object> obj = ObjectUtils.removeObjectFromList(propertyList.Cast<object>().ToList<object>(), id, byte.MaxValue, House.getMaxHouses());
			propertyList = (List<House>) obj.Cast<House>().ToList<House>();

			logToConsole($"Property #{id} was removed.");
			House.updatePropertyCount((byte) (House.getPropertyCount() - 1));
		}

		public static House getProperty(byte id) {return propertyList[id];}

		// Store related

		public static void saveStores()
		{
			FileHandler storeHandler = Store.getStoreHandler();

			foreach(Store store in getStorePool())
			{
				if(store == null) continue;
				ushort id = store.getStoreID();
				float x = store.getStorePos().X, y = store.getStorePos().Y, z = store.getStorePos().Z;
				uint world = store.getStoreWorld();

				string storeInfo = $"{x} {y} {z} {world}";
				storeHandler.addValue($"Store{id}" , storeInfo);
			}

			storeHandler.saveFile();
			storeHandler = null;
		}
		public static void loadStores()
		{
			FileHandler storeHandler = Store.getStoreHandler();
			if(!storeHandler.loadFile()) 
			{
				logToConsole("Failed to load stores.");
				return;
			}

			for(ushort i = 0; i < Store.getMaxStores(); i++)
			{
				string key = $"Store{i}";
				if(!storeHandler.containsKey(key)) continue;

				string storeInfo = storeHandler.getValue(key);
				byte index = 0;

				string xStr = ChatUtils.strTok(storeInfo, ref index), yStr = ChatUtils.strTok(storeInfo, ref index), zStr = ChatUtils.strTok(storeInfo, ref index), worldStr = ChatUtils.strTok(storeInfo, ref index);
				bool failed = false;

				float x = NumberUtils.parseFloat(xStr, ref failed), y = NumberUtils.parseFloat(yStr, ref failed), z = NumberUtils.parseFloat(zStr, ref failed);
				uint world = NumberUtils.parseUnsignedInt(worldStr, ref failed);

				Store store = new Store(x, y, z, world, i);
				addStore(store, i);
			}
			storeHandler = null;
		}

		public static void addStore(Store store, ushort id=ushort.MaxValue)
		{
			if(store != null)
			{
				if(id == ushort.MaxValue) id = store.getStoreID();
				if(storeList[id] != null) removeStore(id);
			}

			if(id >= Store.getMaxStores())
			{
				logToConsole($"Attempt to add store {id}/{Store.getMaxStores()} failed.");
				return;
			}

			storeList.Insert(id, store);
			Store.updateStoreCount((ushort) (Store.getStoreCount() + 1));

			store.updateStoreCP();

			saveStores();
		}

		public static void removeStore(ushort id)
		{
			if(id >= Store.getMaxStores()) return;

			Store store = getStore(id);
			if(store == null) return;

			removeCP(store.getStoreCP().getID());

			Store.getStoreHandler().removeValue($"Store{id}");
			List<object> obj = ObjectUtils.removeObjectFromList(storeList.Cast<object>().ToList<object>(), id, ushort.MaxValue, Store.getMaxStores());
			storeList = (List<Store>) obj.Cast<Store>().ToList<Store>();

			logToConsole($"Store #{id} was removed.");
			Store.updateStoreCount((byte) (Store.getStoreCount() - 1));

			saveStores();
		}

		public static Store getStore(ushort id) {return storeList[id];}

		// Gasstation related

		public static void saveGasStations()
		{
			FileHandler gasHandler = GasStation.getGasHandler();
			foreach(GasStation gas in getGasPool())
			{
				if(gas == null) continue;
				ushort id = gas.getGasStationID();

				Vector3 pos = gas.getGasStationPos();
				float x = pos.X, y = pos.Y, z = pos.Z;

				string gasInfo = $"{x} {y} {z}";
				gasHandler.addValue($"Gas{id}", gasInfo);
			}

			gasHandler.saveFile();
			gasHandler = null;
		}

		public static void loadGasStations()
		{
			FileHandler gasHandler = GasStation.getGasHandler();
			if(!gasHandler.loadFile())
			{
				logToConsole("Failed to load Gas Stations.");
				return;
			}

			for(byte i = 0; i < GasStation.getMaxGasStations(); i++)
			{
				string key = $"Gas{i}";
				if(!gasHandler.containsKey(key)) continue;

				string gasInfo = gasHandler.getValue(key);
				byte index = 0;

				string xStr = ChatUtils.strTok(gasInfo, ref index), yStr = ChatUtils.strTok(gasInfo, ref index), zStr = ChatUtils.strTok(gasInfo, ref index);
				bool failed = false;

				float x = NumberUtils.parseFloat(xStr, ref failed), y = NumberUtils.parseFloat(yStr, ref failed), z = NumberUtils.parseFloat(zStr, ref failed);
			
				GasStation gas = new GasStation(x, y, z, i);
				addGasStation(gas, i);
			}
			gasHandler = null;
		}

		public static void addGasStation(GasStation station, byte id=byte.MaxValue)
		{
			if(station != null && id == byte.MaxValue) id = station.getGasStationID();
			else if(station == null) return;

			if(id >= GasStation.getMaxGasStations())
			{
				logToConsole($"Attempt to add gas station {id}/{GasStation.getMaxGasStations()} failed.");
				return;
			}

			if(gasList[id] != null) removeGasStation(id);

			station.updateGasStationCP();

			gasList.Insert(id, station);
			GasStation.updateGasStationCount((byte) (GasStation.getGasStationCount() - 1));
		}

		public static void removeGasStation(byte id)
		{
			GasStation gas = getGastStation(id);
			if(gas == null) return;

			removeCP(gas.getGasStationCP().getID());

			GasStation.getGasHandler().removeValue($"Gas{id}");
			List<object> obj = ObjectUtils.removeObjectFromList(gasList.Cast<object>().ToList<object>(), id, byte.MaxValue, GasStation.getMaxGasStations());
			gasList = (List<GasStation>) obj.Cast<GasStation>().ToList<GasStation>();

			logToConsole($"Gas station #{id} was removed.");
			GasStation.updateGasStationCount((byte) (GasStation.getGasStationCount() - 1));

			saveGasStations();
		}

		public static GasStation getGasStation(byte id) {return gasList[id];}

		// Server vehicle related

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

            for(int i = 0; i < Vehicle2.getMaxVehicles(); i++)
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

					bool failed = false;

                    MODEL = NumberUtils.parseUnsignedInt(model, ref failed, (byte) model.Length);
                    X = NumberUtils.parseFloat(x, ref failed, (byte) x.Length);
                    Y = NumberUtils.parseFloat(y, ref failed, (byte) y.Length);
                    Z = NumberUtils.parseFloat(z, ref failed, (byte) z.Length);
                    ROT_X = NumberUtils.parseFloat(rotX, ref failed, (byte) rotX.Length);
                    ROT_Y = NumberUtils.parseFloat(rotY, ref failed, (byte) rotY.Length);
                    ROT_Z = NumberUtils.parseFloat(rotZ, ref failed, (byte) rotZ.Length);

                    Vector3 vect = new Vector3(X, Y, Z);
                    Vector3 angle = new Vector3(ROT_X, ROT_Y, ROT_Z);

                    serverVehicleCount++;
                    addVehicle(MODEL, vect, angle, -1, -1, VehicleType.SERVER_VEHICLE);
                }
            }
            logToConsole($"{serverVehicleCount} vehicles loaded.");
        }

        public static void saveVehicles() {vehicleHandler.saveFile();}

        public static bool addVehicleToServer(Vehicle2 vehicle)
        {
            if((vehicles + 1) >= Vehicle2.getMaxVehicles()) return false;

            Vehicle veh = vehicle.getVehicle();
            string model = veh.Model.ToString();
            string line = $"{model} {veh.Position.X} {veh.Position.Y} {veh.Position.Z} {veh.Rotation.X} {veh.Rotation.Y} {veh.Rotation.Z}";

            vehicleHandler.addValue("VEH#" + ++serverVehicleCount, line);

			saveVehicles();
            return true;
        }

        public static int addVehicle(uint hashKey, Vector3 vect, Vector3 rot, int color1=-1, int color2=-1, VehicleType type=VehicleType.CMD_VEHICLE) 
        {
            if((vehicles + 1) >= Vehicle2.getMaxVehicles()) return -1;
            vehicles++;

            Vehicle newVehicle = NAPI.Vehicle.CreateVehicle(hashKey, vect, rot.Z, color1, color2);
            NAPI.Vehicle.SpawnVehicle(newVehicle.Handle, vect, rot.Z);

			newVehicle.Rotation = rot;

            Vehicle2 vehicle = new Vehicle2(newVehicle, type);
            serverVehicles.Insert(newVehicle.Value, vehicle);

			logToConsole($"Vehicle {newVehicle.Value} added.");

            return newVehicle.Value;
        }
        public static void removeVehicle(int vehicleid) 
        {
            Vehicle2 vehicle = serverVehicles[vehicleid];
			if(vehicle == null) return;

			vehicles--;

            vehicle.getVehicle().Delete();
            serverVehicles.RemoveAt(vehicleid);

			logToConsole($"Vehicle #{vehicleid} removed.");
        }

		public static Vehicle2 getVehicleFromID(int vehicleid) 
		{
			foreach(Vehicle2 vehicle in getVehiclePool().ToList<Vehicle2>()) if(vehicle.getVehicle().Value == vehicleid) return vehicle;
			return null;
		}

        public static Vehicle2 getVehicleObject(Vehicle vehicle)
        {
            foreach(Vehicle2 veh in serverVehicles.ToList<Vehicle2>()) if(veh.getVehicle().Equals(vehicle)) return veh;
            return null;
        }

		public static void respawnAllVehicles()
		{
			foreach(Vehicle2 vehicle in getVehiclePool().ToList<Vehicle2>()) 
			{
				if(vehicle == null) continue;

				Vehicle veh = vehicle.getVehicle();
				if(veh.Occupants.Count != 0) continue;

				vehicle.respawnVeh();
			}
		}

		// Organization related

		private static void loadOrganizations()
		{
			if(serverDataHandler == null)
			{
				sendServerFatalError("Server Data Handler is null.");
				return;
			}

			byte checkOrgCount = 0;
            for(byte i = 0; i < Organization.getMaxOrgs(); i++)
            {
                orgList.Insert(i, new Organization(i));

                if(orgList[i].doesOrgExist()) checkOrgCount++;
                else orgList.Insert(i, null);
            }

			Organization.updateOrgCount(checkOrgCount);

            serverDataHandler.addValue(ServerUtils.ServerDataInfo.ORGS.ToString(), $"{Organization.getOrgCount()}");
			Console.WriteLine($"{Organization.getOrgCount()} organizations loaded.");
		}

        public static sbyte addOrg(string name, Color color, Player creatingPlayer)
        {
            if((Organization.getOrgCount() + 1) >= Organization.getMaxOrgs()) return -1;

            sbyte orgID = ((sbyte) -1);
            for(sbyte i = 0; i < Organization.getMaxOrgs(); i++)
            {
                if(orgList[i] == null || !orgList[i].doesOrgExist())
                {
                    orgID = i;
					ChatUtils.sendClientMessage(creatingPlayer, ServerData.COLOR_RED, $"Org ID set to {orgID}");
                    break;
                }
            }
            Organization newOrg = new Organization((byte) orgID, creatingPlayer, name);
			newOrg.setColor(color);

            orgList.Insert(orgID, newOrg);

            byte count = 0;
            foreach(Organization org in orgList.ToList<Organization>()) {if(org != null && org.doesOrgExist()) count++;}

            Organization.updateOrgCount(count);

            return orgID;
        }
        public static Organization getOrg(sbyte id) 
		{
			if(id > Organization.getMaxOrgs() || id < 0) return null;
			return orgList[id];
		}

		public static void removeOrg(byte id)
		{
			if(id > Organization.getMaxOrgs() || id < 0) return;
			orgList.Insert(id, null);

	        byte count = 0;
            foreach(Organization org in orgList.ToList<Organization>()) {if(org != null && org.doesOrgExist()) count++;}

            Organization.updateOrgCount(count);
		}

		// Checkpoint related

        public static Checkpoint2 addCP(Vector3 location, uint dimension=0, CPType cpType=CPType.NULL)
        {
            if((Checkpoint2.getCPCount() + 1) > Checkpoint2.getMaxCheckpoints()) 
			{
				logToConsole($"Failed to add checkpoint (type: {cpType.ToString()}) due to max checkpoints loaded.");
				return null;
			}
			Checkpoint2.updateCPCount((ushort) (Checkpoint2.getCPCount() + 1));

            Checkpoint2 newCP = new Checkpoint2(location, dimension, cpType);
            ushort id = newCP.getID();

            cpList.Insert(id, newCP);
            return newCP;
        }
        public static Checkpoint2 getCP(ushort id) 
		{
			if(id < 0 || id > Checkpoint2.getMaxCheckpoints()) return null;
			return cpList[id];
		}
        public static void removeCP(ushort id) 
        {
			if(cpList[id] == null) return;
            Checkpoint2 cp = cpList[id];
            cpList[id] = null;
            cp.getCheckpoint().Delete();
        }

		// Command Utilities

        public static bool commandCheck(Player player, out string output, string[] parameters=null, bool orgCMD=false, byte orgLevel=0, bool registered=true, bool logged=true, bool mustSpawn = true, byte adminLevel=0, byte supportLevel=0, byte vipLevel=0)
        {
            PlayerData data = PlayerDataInfo.getPlayerData(player);

            bool failed = false;
			byte size = (parameters == null) ? (byte) 0 : (byte) parameters.Length;
            output = null;

			if (!serverDataListsInitialized)
			{
                output = ChatUtils.colorString("Server data has not yet been initialized.", ChatUtils.getColorAsHex(ServerData.COLOR_RED));
                goto END;
			}
            failed = (data.getAdminLevel() < adminLevel);
			if(failed) 
			{
				output = ChatUtils.colorString("ERROR: Command not found.", ChatUtils.getColorAsHex(ServerData.COLOR_WHITE));
				goto END;
			}
            else failed = (!failed && registered && !data.isPlayerRegistered());

            if(failed) 
			{
				output = ChatUtils.colorString("You must register to play! Type /register to continue.", ChatUtils.getColorAsHex(ServerData.COLOR_RED));
				goto END;
			}
			else failed = (!failed && logged && !data.isPlayerLoggedIn());

			if(failed)
            {
                output = ChatUtils.colorString("You must login to play! Type /login to continue.", ChatUtils.getColorAsHex(ServerData.COLOR_RED));
                goto END;
            }
			else failed = (!failed && (NAPI.Player.IsPlayerRespawning(player) && mustSpawn));

			if(failed)
			{
				output = ChatUtils.colorString("Error: You must spawn to use this command.", ChatUtils.getColorAsHex(ServerData.COLOR_RED));
				goto END;
			}
			else failed = (!failed && data.getSupportLevel() == 0 && supportLevel > 0);

			if(failed)
			{
				output = ChatUtils.colorString("Error: You aren't a helpdesk operator.", ChatUtils.getColorAsHex(ServerData.COLOR_RED));
				goto END;
			}
			else failed = (!failed && orgCMD && data.getPlayerOrg() == null);

			if(failed)
			{
                output = ChatUtils.colorString("Error: You aren't in an Organization.", ChatUtils.getColorAsHex(ServerData.COLOR_RED));
                goto END;
			}
			else failed = (!failed && orgCMD && orgLevel == 1 && data.getPlayerOrgLevel() == 0);

			if(failed)
			{
				output = ChatUtils.colorString("Error: You aren't the (co)leader of an organization.", ChatUtils.getColorAsHex(ServerData.COLOR_RED));
				goto END;
			}
			else failed = (!failed && orgCMD && orgLevel == 2 && data.getPlayerOrgLevel() < 2);

			if(failed)
			{
				output = ChatUtils.colorString("Error: You aren't the leader of an organization.", ChatUtils.getColorAsHex(ServerData.COLOR_RED));
				goto END;
			}
			else failed = ((!failed && parameters != null) && !parameterCheck(parameters, size));

			if(failed)
			{
				output = $"Usage: /{parameters[0]}";
				for(byte i = 1; i <= (size - 1) / 2; i++) output = $"{output} [{parameters[i]}]";
				output = ChatUtils.colorString(output, ChatUtils.getColorAsHex(ServerData.COLOR_RED));
				failed = true;
			}


            END: return failed;
        }

		private static bool parameterCheck(string[] checkParams, byte size)
		{
			for(byte i = 0; i < size; i++) if(checkParams[i] == "NullCMDStr" || checkParams[i] == "NullCMDText") return false;
			return true;
		}
    }
}