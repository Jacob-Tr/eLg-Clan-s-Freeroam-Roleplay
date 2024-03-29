﻿namespace e_freeroam.Utilities.ServerUtils
{
	// Server related
    public enum ServerDataInfo
    {
		ACCOUNTS,
		IPS_BANNED,
        WRLDTIME,
        ORGS,
		BUSES,
		PROPS,
        NULL
    }

    public enum FileTypes
    {
        PLAYER,
        SERVER,
		BAN,
		POSITION,
        VEHICLE,
        ORGMETA,
		OMEMBERS,
		BUSINESS,
		STORE,
		PROPERTY,
		GASSTATION,
		ENEX,
        NULL
    }

	public enum MuteCMDS
	{
		ME,
		DO,
		AC
	}

	// Player related

    public enum PlayerInfo
    {
        PASSWORD,
		ACCID,
        ADLVL,
		VIPLVL,
		STLVL,
        HEALTH,
        ARMOR,
        X,
        Y,
        Z,
        ANGLE,
		MUTED,
		MUTEDTIME,
		FROZEN,
		BANNED,
		TIMES_MUTED,
		TIMES_KICKED,
		TIMES_FROZEN,
		TIMES_BANNED,
		ORG,
		ORGLVL,
		MONEY,
		SCORE,
		SCORECOUNTER,
		BUSINESS,
        NULL
    }

	// Vehicle related

    public enum VehicleType
    {
        SERVER_VEHICLE,
        CMD_VEHICLE,
        JOB_VEHICLE,
        SCHOOL_VEHICLE,
        ORG_VEHICLE
    }

    public enum serverVehicleData
    {
        MODEL,
        VEH_X,
        VEH_Y,
        VEH_Z,
        VEH_ROT
    }

	// Organization related

    public enum orgInfo
    {
        NAME,
        X,
        Y,
        Z,
        COLOR,
		DIMENSION,
        VEHICLES
    }

	public enum OrgMemberInfo
	{
		LEADER,
		COLEADER,
		MEMBER
	}

	public enum OrgType
	{
		GANG,
		LAW,
		PRIVATE
	}

	public enum LawSubtype
	{
		POLICE,
		SWAT,
		ARMY,
		PARAMEDIC
	}

	public enum GangSubtype
	{
		GANG,
		MAFIA,
		TERRORIST
	}

	public enum PBSubtype
	{
		TRANSPORT,
		HITMAN,
		QUARRY,
		RACING,
		MECHANIC,
		OTHER
	}

	public enum OrgLevels
	{
		LEADER,
		COLEADER,
		MEMBER
	}

	// Checkpoint related

    public enum CPType
    {
        ORG,
		BUSINESS,
		BANK,
		FOOD,
		STORE,
		GAS,
		MISSION,
        NULL
    }

	// Business related

	public enum BusInfo
	{
		OWNER,
		NAME,
		X,
		Y,
		Z,
		PRICE,
		PROFIT,
		INTERIOR,
		WORLD
	}

	// Property related

	public enum PropInfo
	{
		OWNER,
		X,
		Y,
		Z,
		ENEX
	}

	// Store related

	// Dialog related

	public enum StoreInfo
	{
		X,
		Y,
		Z,
		WORLD
	}

	public enum DialogType
	{
		INFO,
		LIST,
		INPUT_TEXT,
		PASSWORD,
		TABLIST
	}

	public enum DialogID
	{
		DIALOG_REGISTER,
		DIALOG_CONFIRM,
		DIALOG_LOGIN,
		DIALOG_ORG_MENU,
		DIALOG_ORG_MEMBERS,
		DIALOG_ORG_COLEADER_CLICK_MEMBER,
		DIALOG_ORG_REQUEST
	}

    public enum KeyRef
    {
        LEFT_MOUSE_BUTTON,
        RIGHT_MOUSE_BUTTON,
        CONTROLBREAK_PROCESSING,
        MIDDLE_MOUSE_BUTTON,
        X1_MOUSE_BUTTON,
        X2_MOUSE_BUTTON,
        UNDEFINED,
        BACKSPACE_KEY,
        TAB_KEY,
        RESERVED,
        CLEAR_KEY,
        ENTER_KEY,
        UNDEFINED2,
        SHIFT_KEY,
        CTRL_KEY,
        ALT_KEY,
        PAUSE_KEY,
        CAPS_LOCK_KEY,
        IME_KANA_MODE,
        IME_HANGUEL_MODE,
        IME_HANGUL_MODE,
        IME_ON,
        IME_JUNJA_MODE,
        IME_FINAL_MODE,
        IME_HANJA_MODE,
        IME_KANJI_MODE,
        IME_OFF,
        ESC_KEY,
        IME_CONVERT,
        IME_NONCONVERT,
        IME_ACCEPT,
        IME_MODE_CHANGE_REQUEST,
        SPACEBAR,
        PAGE_UP_KEY,
        PAGE_DOWN_KEY,
        END_KEY,
        HOME_KEY,
        LEFT_ARROW_KEY,
        UP_ARROW_KEY,
        RIGHT_ARROW_KEY,
        DOWN_ARROW_KEY,
        SELECT_KEY,
        PRINT_KEY,
        EXECUTE_KEY,
        PRINT_SCREEN_KEY,
        INS_KEY,
        DEL_KEY,
        HELP_KEY,
        ZERO_KEY,
        ONE_KEY,
        TWO_KEY,
        THREE_KEY,
        FOUR_KEY,
        FIVE_KEY,
        SIX_KEY,
        SEVEN_KEY,
        EIGHT_KEY,
        NINE_KEY,
        UNDEFINED3,
        A_KEY,
        B_KEY,
        C_KEY,
        D_KEY,
        E_KEY,
        F_KEY,
        G_KEY,
        H_KEY,
        I_KEY,
        J_KEY,
        K_KEY,
        L_KEY,
        M_KEY,
        N_KEY,
        O_KEY,
        P_KEY,
        Q_KEY,
        R_KEY,
        S_KEY,
        T_KEY,
        U_KEY,
        V_KEY,
        W_KEY,
        X_KEY,
        Y_KEY,
        Z_KEY,
        LEFT_WINDOWS_KEY,
        RIGHT_WINDOWS_KEY,
        APPLICATIONS_KEY,
        RESERVED2,
        COMPUTER_SLEEP_KEY,
        NUMERIC_KEYPAD_0_KEY,
        NUMERIC_KEYPAD_1_KEY,
        NUMERIC_KEYPAD_2_KEY,
        NUMERIC_KEYPAD_3_KEY,
        NUMERIC_KEYPAD_4_KEY,
        NUMERIC_KEYPAD_5_KEY,
        NUMERIC_KEYPAD_6_KEY,
        NUMERIC_KEYPAD_7_KEY,
        NUMERIC_KEYPAD_8_KEY,
        NUMERIC_KEYPAD_9_KEY,
        MULTIPLY_KEY,
        ADD_KEY,
        SEPARATOR_KEY,
        SUBTRACT_KEY,
        DECIMAL_KEY,
        DIVIDE_KEY,
        F1_KEY,
        F2_KEY,
        F3_KEY,
        F4_KEY,
        F5_KEY,
        F6_KEY,
        F7_KEY,
        F8_KEY,
        F9_KEY,
        F10_KEY,
        F11_KEY,
        F12_KEY,
        F13_KEY,
        F14_KEY,
        F15_KEY,
        F16_KEY,
        F17_KEY,
        F18_KEY,
        F19_KEY,
        F20_KEY,
        F21_KEY,
        F22_KEY,
        F23_KEY,
        F24_KEY,
        UNASSIGNED,
        NUM_LOCK_KEY,
        SCROLL_LOCK_KEY,
        OEM_SPECIFIC,
        UNASSIGNED2,
        LEFT_SHIFT_KEY,
        RIGHT_SHIFT_KEY,
        LEFT_CONTROL_KEY,
        RIGHT_CONTROL_KEY,
        LEFT_MENU_KEY,
        RIGHT_MENU_KEY,
        BROWSER_BACK_KEY,
        BROWSER_FORWARD_KEY,
        BROWSER_REFRESH_KEY,
        BROWSER_STOP_KEY,
        BROWSER_SEARCH_KEY,
        BROWSER_FAVORITES_KEY,
        BROWSER_START_AND_HOME_KEY,
        VOLUME_MUTE_KEY,
        VOLUME_DOWN_KEY,
        VOLUME_UP_KEY,
        NEXT_TRACK_KEY,
        PREVIOUS_TRACK_KEY,
        STOP_MEDIA_KEY,
        PLAY_MEDIA_KEY,
        START_MAIL_KEY,
        SELECT_MEDIA_KEY,
        START_APPLICATION_1_KEY,
        START_APPLICATION_2_KEY,
        RESERVED3,
        COLON_KEY,
        PLUS_KEY,
        COMMA_KEY,
        MINUS_KEY,
        PERIOD_KEY,
        SLASH_KEY,
        TILD_KEY,
        RESERVED4,
        UNASSIGNED3,
        OCURLBRKT_KEY,
        BACKSLASH_KEY,
        CCURLBRKT_KEY,
        QUOTE_KEY,
        MISCELLANEOUS_CHARACTERS_KEY,
        RESERVED5,
        OEM_SPECIFIC2,
        ANGLE_BRACKET_KEY,
        OEM_SPECIFIC3,
        IME_PROCESS_KEY,
        OEM_SPECIFIC4,
        VK_PACKET_KEY,
        UNASSIGNED4,
        OEM_SPECIFIC5,
        ATTN_KEY,
        CRSEL_KEY,
        EXSEL_KEY,
        ERASE_EOF_KEY,
        PLAY_KEY,
        ZOOM_KEY,
        RESERVED6,
        PA1_KEY,
        CLEAR_KEY2
    }
}
