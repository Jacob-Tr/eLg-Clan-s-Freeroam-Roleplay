using e_freeroam.Utilities.ServerUtils;
using GTANetworkAPI;

namespace e_freeroam.Utilities
{
    public abstract class NumberUtils
    {
        public static bool isNumeric(string str, int length)
        {
            if(length < 1) return false;

            int character = -1;
            for(int i = 0; i < length; i++)
            {
                character = (int) str[i];
                if((character < 48 || character > 57) && character != 46) return false;
            }

            return true;
        }

		public static int exp(int numOne, int numTwo)
		{
			int num = numOne;
			for(int i = 0; i < numTwo; i++) num *= numOne;

			return num;
		}

		public static int getNum(char ch)
		{
			if (!isNumeric("" + ch, 1)) return 0;
			return ((int)ch - 48);
		}

		public static float decimalize(int number, int place)
		{
			float temp = (float)number;
			for (int i = 0; i < place; i++) temp *= 0.1F;

			return temp;
		}

		public static float parseFloat(string str, int length)
		{
			if (length < 1) return 0;

			bool deci = false;

			if (str.LastIndexOf('.') != -1)
			{
				length = str.LastIndexOf('.');
				deci = true;
			}

			bool negative = false;
			if (str[0] == '-')
			{
				negative = true;

				str = str.Substring(1);
				--length;
			}

			if (!isNumeric(str, str.Length)) return 0;

			int endPos = (length - 2), charInt = -1, index = 0;
			float sum = 0;

			for (int i = 0; i < length; i++)
			{
				charInt = getNum(str[i]);
				if (endPos > -1) charInt *= exp(10, endPos--);

				sum += (float)charInt;
				index = i;
			}

			if (deci && (index != str.Length - 1))
			{
				string deciStr = str.Substring(index + 2);
				int num = (int)parseInt(deciStr, deciStr.Length);

				sum += decimalize(num, deciStr.Length);
			}
			return (negative) ? (0 - sum) : sum;
		}

		public static int parseInt(string str, int length)
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

			int endPos = length - 2, charInt = -1, sum = 0;

			for(int i = 0; i < length; i++)
			{
				charInt = ((int)str[i] - 48);
				if(endPos > -1) charInt *= exp(10, endPos--);

				sum += charInt;
			}

			return (negative) ? (0 - sum) : sum;
		}

		public static short parseShort(string str, int length)
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

			int endPos = length - 2;
			short charInt = -1, sum = 0;

			for(int i = 0; i < length; i++)
			{
				charInt = ((short) (str[i] - 48));
				if(endPos > -1) charInt *= ((short) exp(10, endPos--));

				sum += charInt;
			}

			return (negative) ? ((short) (0 - sum)) : sum;
		}

		public static sbyte parseByte(string str, int length)
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

			int endPos = length - 2;
			sbyte charInt = ((sbyte) -1), sum = ((sbyte) 0);

			for(int i = 0; i < length; i++)
			{
				charInt = ((sbyte) (str[i] - 48));
				if(endPos > -1) charInt *= ((sbyte) exp(10, endPos--));

				sum += charInt;
			}

			return (negative) ? ((sbyte) (0 - sum)) : sum;
		}
	}
}
