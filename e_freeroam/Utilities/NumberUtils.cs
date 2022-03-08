using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;
using System;

namespace e_freeroam.Utilities
{
    public abstract class NumberUtils
    {
		// Time 

		public static string processTime(double delay, ref short hours, ref byte minutes, ref byte seconds)
		{
			double hoursTotal = 0, minutesTotal = 0;
			hours = 0;
			minutes = 0;
			seconds = 0;

			minutesTotal = (delay / 60);
			hoursTotal = (minutesTotal / 60);

			if(hoursTotal != 1)
			{
				hours = (short) Math.Floor(hoursTotal);
				minutesTotal = (hoursTotal - Math.Floor(hoursTotal)) * 60;
			}

			minutes += (byte) Math.Floor(minutesTotal);
			seconds += (minutesTotal != 0) ? (byte) Math.Round((minutesTotal - minutes) * 60) : (byte) Math.Round(delay);

			if(minutes == 60)
			{
				minutes = 0;
				hours++;
			}

			string secStr = (seconds < 10) ? $"0{seconds}" : $"{seconds}", minStr = (minutes < 10) ? $"0{minutes}" : $"{minutes}", hourStr = (hours < 10) ? $"0{hours}" : $"{hours}";

			return $"{hourStr}:{minStr}:{secStr}";
		}

		public static string processTimeStr(double delay)
		{
			double hoursTotal = 0, minutesTotal = 0;
			short hours = 0;
			byte minutes = 0, seconds = 0;

			minutesTotal = (delay / 60);
			hoursTotal = (minutesTotal / 60);

			if(hoursTotal != 1)
			{
				hours = (short) Math.Floor(hoursTotal);
				minutesTotal = (hoursTotal - Math.Floor(hoursTotal)) * 60;
			}

			minutes += (byte) Math.Floor(minutesTotal);
			seconds += (minutesTotal != 0) ? (byte) Math.Round((minutesTotal - minutes) * 60) : (byte) Math.Round(delay);

			if(minutes == 60)
			{
				minutes = 0;
				hours++;
			}

			string secStr = (seconds < 10) ? $"0{seconds}" : $"{seconds}", minStr = (minutes < 10) ? $"0{minutes}" : $"{minutes}", hourStr = (hours < 10) ? $"0{hours}" : $"{hours}";

			return $"{hourStr}:{minStr}:{secStr}";
		}

		// Math solutions

		public static uint unsignedExp(uint numOne, int numTwo)
		{
			uint num = numOne;
			for(uint i = 1; i < numTwo; i++) num *= numOne;

			return num;
		}

		public static float floatExp(float numOne, float numTwo)
		{
			float num = numOne;
			for(int i = 1; i < numTwo; i++) num *= numOne;

			return num;
		}

		public static int signedExp(int numOne, int numTwo)
		{
			int num = numOne;
			for(int i = 1; i < numTwo; i++) num *= numOne;

			return num;
		}

		public static float decimalize(int number, byte place)
		{
			double temp = (double)number;
			for(byte i = 0; i < place; i++) temp *= 0.1D;
			temp = Math.Round(temp, place);

			return (float) temp;
		}

		// String/number checks

		public static bool isNumber(char character)
		{
			if((character < '0' || character > '9') && character != '.' && character != '-') 
			{
				ServerData.logToConsole($"Character {character}(#{(byte) character}) was not numeric."); // Possible charset differences
				return false;
			}
			return true;
		}

        public static bool isNumeric(string str)
        {
			byte length = (byte) str.Length;
            if(length < 1) return false;

			if(length == 1 && str[0] == '-') return false; 
			if(length > 1 && str.IndexOf("-", 1) != -1) return false;

			int deciIndex = str.IndexOf('.');
			if(deciIndex != -1 && (str.Length > deciIndex && str.IndexOf('.', deciIndex + 1) != -1)) return false; 

            for(byte i = 0; i < length; i++) if(!isNumber(str[i]))return false;

            return true;
        }

		public static bool isNumericStringNegative(string str) {return (str.Length > 0) ? (isNumeric(str) ? (str[0] == '-') : false) : false;}

		public static bool isStrFloat(string str)
		{
			if(!isNumeric(str)) return false;
			bool value = (str.IndexOf('.') != -1);
			return value;
		}

		public static bool isNumericStringLessThan(string str, string value) {return numberParsableCheck(ref str, $"{value}");}

		public static void strAbs(ref string str)
		{
			if(!isNumeric(str)) return;
			if(isNumericStringNegative(str)) str = str.Substring(1);
		}

		public static byte getNum(char ch)
		{
			if(!isNumber(ch)) return 0;
			return ((byte) (ch - 48));
		}

		public static bool numberParsableCheck(ref string str, string maxValue=null)
		{
			if(!isNumeric(str)) return false;

			bool negative = false;

			if(isNumericStringNegative(str)) 
			{
				negative = true;
				strAbs(ref str);
			}

			if(maxValue == null) maxValue = (isStrFloat(str)) ? $"{float.MaxValue}" : ((negative) ? $"{int.MaxValue}" : $"{uint.MaxValue}"); // If it's a float, max value is max float; if not then if it's negative the max is max int. Otherwise max unsigned int.

			if(str.Length > maxValue.Length) str = maxValue;
			if(str.Length == maxValue.Length)
			{
				byte strNum = byte.MaxValue, maxNum = byte.MaxValue;
				for(byte i = 0; i < str.Length; i++) 
				{
					strNum = getNum(str[i]);
					maxNum = getNum(maxValue[i]);

					if(maxNum == '.' && strNum != '.')
					{
						str = maxValue;
						return false;
					}
					if(strNum == '.' && maxNum != '.') return true;
					if(strNum == '.' || maxNum == '.') continue;

					if(strNum > maxNum)
					{
						str = maxValue;
						return false;
					}
					else if(strNum < maxNum) break;
				}
			}

			if(negative) str = $"-{str}";
			return true;
		}

		// String parsing

		public static float parseFloat(string str, ref bool failed, byte length=0)
		{
			if(str.Length < 1) goto FAILED;

			bool deci = false, negative = (str[0] == '-');

			if(str.LastIndexOf('.') != -1)
			{
				length = (byte) (str.LastIndexOf('.'));
				deci = true;
			}
			else length = (byte) str.Length;

			int decim = (deci) ? parseInt(str.Substring(length + 1), ref failed) : 0;
			float sum = (float) parseInt(str.Substring(0, length), ref failed), dec = decimalize(decim, (byte) (str.Length - (length + 1)));

			if(deci && length < (str.Length)) sum += (negative) ? (0 - dec) : dec;
			if(failed || sum > float.MaxValue) goto FAILED;

			if($"{sum}" != str) ServerData.logToConsole($"(parseFloat) Input {str} is not equal to result {sum}");
			return sum;

			FAILED: 
			failed = true;
			ServerData.sendServerFatalError($"ParseFloat failed on input {str}.");
			return 0;
		}

		public static uint parseUnsignedInt(string str, ref bool failed, byte length=0)
		{
			if(str.Length < 1) goto FAILED;

			uint sum = 0;

			try {sum = uint.Parse(str);}
			catch(FormatException) {goto FAILED;}

			if($"{sum}" != str) ServerData.logToConsole($"(parseUnsignedInt) Input {str} is not equal to result {sum}");
			return sum;

			FAILED: 
			failed = true;
			ServerData.sendServerFatalError($"parseUnsignedInt failed on input {str}.");
			return 0;
		}

		public static int parseInt(string str, ref bool failed)
		{
			if(str.Length < 1) goto FAILED;

			int sum = 0;
			try {sum = int.Parse(str);}
			catch(FormatException) {goto FAILED;}

			if($"{sum}" != str) ServerData.logToConsole($"(parseInt) Input {str} is not equal to result {sum}");
			return sum;

			FAILED: 
			failed = true;
			ServerData.sendServerFatalError($"parseInt failed on input {str}.");
			return 0;
		}

		public static ushort parseUnsignedShort(string str, ref bool failed, byte length=0)
		{
			if(str.Length < 1) goto FAILED;

			ushort sum = 0;
			try {sum = ushort.Parse(str);}
			catch(FormatException) {goto FAILED;}

			if($"{sum}" != str) ServerData.sendServerFatalError($"(parseUnsignedShort) Input {str} is not equal to result {sum}.");
			return sum;

			FAILED: 
			failed = true;
			ServerData.sendServerFatalError($"parseUnsignedShort failed on input {str}.");
			return 0;
		}

		public static short parseShort(string str, ref bool failed, byte length=0)
		{
			if(str.Length < 1) goto FAILED;

			short sum = 0;
			try {sum = short.Parse(str);}
			catch(FormatException) {goto FAILED;}

			if($"{sum}" != str) ServerData.sendServerFatalError($"(parseShort) Input {str} is not equal to result {sum}.");
			return sum;

			FAILED: 
			failed = true;
			ServerData.sendServerFatalError($"parseShort failed on input {str}.");
			return 0;
		}

		public static sbyte parseSignedByte(string str, ref bool failed, byte length=0)
		{	
			if(str.Length < 1) goto FAILED;

			sbyte sum = 0;
			try {sum = sbyte.Parse(str);}
			catch(FormatException) {goto FAILED;}

			if($"{sum}" != str) ServerData.sendServerFatalError($"(parseSignedByte) Input {str} is not equal to result {sum}.");
			return sum;

			FAILED: 
			failed = true;
			ServerData.sendServerFatalError($"parseSignedByte failed on input {str}.");
			return 0;
		}

		public static byte parseByte(string str, ref bool failed, byte length=0)
		{
			if(str.Length < 1) goto FAILED;

			byte sum = 0;
			try {sum = byte.Parse(str);}
			catch(FormatException) {goto FAILED;}

			if($"{sum}" != str) ServerData.sendServerFatalError($"(parseShort) Input {str} is not equal to result {sum}.");
			return sum;

			FAILED: 
			failed = true;
			ServerData.sendServerFatalError($"parseByte failed on input {str}.");
			return 0;
		}
	}
}
