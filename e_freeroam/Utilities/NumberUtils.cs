﻿using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;
using System;

namespace e_freeroam.Utilities
{
    public abstract class NumberUtils
    {
		public static bool isNumber(char character)
		{
			if((character < 48 || character > 57) && character != 46) return false;
			return true;
		}

        public static bool isNumeric(string str, byte length)
        {
            if(length < 1) return false;
            for(byte i = 0; i < length; i++) if(!isNumber(str[i])) return false;

            return true;
        }

		public static string processTime(double delay)
		{
			double hoursTotal = 0, minutesTotal = 0;
			int hours = 0, minutes = 0, seconds = 0;

			minutesTotal = (double) (delay / 60);
			hoursTotal = (double) (minutesTotal / 60);

			if(hoursTotal != 1)
			{
				hours = (int) Math.Floor(hoursTotal);
				minutesTotal = (hoursTotal - Math.Floor(hoursTotal)) * 60;
			}

			minutes += (int) Math.Floor(minutesTotal);
			seconds += (minutesTotal != 0) ? (int) Math.Round((minutesTotal - minutes) * 60) : (int) Math.Round(delay);

			string secStr = (seconds < 10) ? $"0{seconds}" : $"{seconds}", minStr = (minutes < 10) ? $"0{minutes}" : $"{minutes}", hourStr = (hours < 10) ? $"0{hours}" : $"{hours}";

			return $"{hourStr}:{minStr}:{secStr}";
		}

		public static uint unsignedExp(uint numOne, int numTwo)
		{
			uint num = numOne;
			for(uint i = 0; i < numTwo; i++) num *= numOne;

			return num;
		}

		public static int exp(int numOne, int numTwo)
		{
			int num = numOne;
			for(int i = 0; i < numTwo; i++) num *= numOne;

			return num;
		}

		public static byte getNum(char ch)
		{
			if(!isNumber(ch)) return 0;
			return ((byte) (ch - 48));
		}

		public static float decimalize(int number, byte place)
		{
			float temp = (float)number;
			for(byte i = 0; i < place; i++) temp *= 0.1F;

			return temp;
		}

		public static float parseFloat(string str, byte length)
		{
			if (length < 1) return 0;

			bool deci = false;

			if (str.LastIndexOf('.') != -1)
			{
				length = ((byte) str.LastIndexOf('.'));
				deci = true;
			}

			bool negative = false;
			if (str[0] == '-')
			{
				negative = true;

				str = str.Substring(1);
				--length;
			}

			if(!isNumeric(str, (byte) str.Length)) return 0;

			int charInt = -1;
			sbyte endPos = (sbyte) (length - 2), index = 0;
			float sum = 0;

			for(sbyte i = 0; i < length; i++)
			{
				charInt = getNum(str[i]);
				if (endPos > -1) charInt *= exp(10, endPos--);

				sum += (float)charInt;
				index = i;
			}

			if(deci && (index != str.Length - 1))
			{
				string deciStr = str.Substring(index + 2);
				int num = (int)parseInt(deciStr, (byte) deciStr.Length);

				sum += decimalize(num, (byte) deciStr.Length);
			}
			return (negative) ? (0 - sum) : sum;
		}

		public static uint parseUnsignedInt(string str, byte length)
		{

			if(length < 1) return 0;
			if(str.LastIndexOf('.') != -1) return 0;

			bool negative = false;
			if(str[0] == '-')
			{
				negative = true;

				str = str.Substring(1);
				--length;
			}

			if(!isNumeric(str, length)) return 0;

			sbyte endPos = (sbyte) (length - 2);
			uint sum = 0, charInt = 0;

			for(byte i = 0; i < length; i++)
			{
				charInt = (uint) (str[i] - 48);
				if(endPos > -1) charInt *= unsignedExp(10, endPos--);

				sum += charInt;
			}

			return (negative) ? (0 - sum) : sum;
		}

		public static int parseInt(string str, byte length)
		{

			if(length < 1) return 0;
			if(str.LastIndexOf('.') != -1) return 0;

			bool negative = false;
			if(str[0] == '-')
			{
				negative = true;

				str = str.Substring(1);
				--length;
			}

			if(!isNumeric(str, length)) return 0;

			sbyte endPos = (sbyte) (length - 2);
			int sum = 0, charInt = -1;

			for(byte i = 0; i < length; i++)
			{
				charInt = ((sbyte) (str[i] - 48));
				if(endPos > -1) charInt *= exp(10, endPos--);

				sum += charInt;
			}

			return (negative) ? (0 - sum) : sum;
		}

		public static short parseShort(string str, byte length)
		{

			if(length < 1) return 0;
			if(str.LastIndexOf('.') != -1) return 0;

			bool negative = false;
			if(str[0] == '-')
			{
				negative = true;

				str = str.Substring(1);
				--length;
			}

			if(!isNumeric(str, length)) return 0;

			sbyte endPos = (sbyte) (length - 2);
			short charInt = -1, sum = 0;

			for(byte i = 0; i < length; i++)
			{
				charInt = ((short) (str[i] - 48));
				if(endPos > -1) charInt *= ((short) exp(10, endPos--));

				sum += charInt;
			}

			return (negative) ? ((short) (0 - sum)) : sum;
		}

		public static sbyte parseSignedByte(string str, byte length)
		{
			if(length < 1) return 0;
			if(str.LastIndexOf('.') != -1) return 0;

			bool negative = false;
			if(str[0] == '-')
			{
				negative = true;

				str = str.Substring(1);
				--length;
			}

			if(!isNumeric(str, length)) return 0;

			sbyte endPos = (sbyte) (length - 2);
			sbyte charInt = ((sbyte) -1), sum = ((sbyte) 0);

			for(byte i = 0; i < length; i++)
			{
				charInt = ((sbyte) (str[i] - 48));
				if(endPos > -1) charInt *= ((sbyte) exp(10, endPos--));

				sum += charInt;
			}

			return (negative) ? ((sbyte) (0 - sum)) : sum;
		}

		public static byte parseByte(string str, byte length)
		{
			if(length < 1) return 0;
			if(str.LastIndexOf('.') != -1) return 0;

			bool negative = false;
			if(str[0] == '-')
			{
				negative = true;

				str = str.Substring(1);
				--length;
			}

			if(!isNumeric(str, length)) return 0;

			sbyte endPos = (sbyte) (length - 2), charInt = ((sbyte) -1);
			byte sum = ((byte) 0);

			for(byte i = 0; i < length; i++)
			{
				charInt = ((sbyte) (str[i] - 48));
				if(endPos > -1) charInt *= ((sbyte) exp(10, endPos--));

				sum += (byte) charInt;
			}

			return (negative) ? ((byte) (0 - sum)) : sum;
		}
	}
}
