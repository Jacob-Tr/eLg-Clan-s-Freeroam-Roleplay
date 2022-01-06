using RAGE;
using RAGE.Game;
using System.Collections.Generic;

namespace e_freeroam_client
{
    enum KeyRef
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

    class Utilities : Events.Script
    {
        static int[] keys =
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
        int[] acceptableKeys =
        {
            keys[(int) KeyRef.N_KEY],
            keys[(int) KeyRef.TWO_KEY]
        };
        bool[] keyPressed = new bool[191];

        public Utilities()
        {
            Events.Tick += detectKeyPress;
            Events.Tick += detectKeyRelease;
        }

        public void detectKeyPress(List<Events.TickNametagData> nametag)
        {
            foreach (int key in acceptableKeys)
            {
                if(RAGE.Input.IsDown(key) && !keyPressed[key])
                {
                    RAGE.Events.CallRemote("OnKeyPress", key);
                    keyPressed[key] = true;
                }
            }
        }

        public void detectKeyRelease(List<Events.TickNametagData> nametag)
        {
            foreach (int key in acceptableKeys)
            {
                if (RAGE.Input.IsUp(key) && keyPressed[key])
                {
                    RAGE.Events.CallRemote("OnKeyRelease", key);
                    keyPressed[key] = false;
                }
            }
        }
    }
}
