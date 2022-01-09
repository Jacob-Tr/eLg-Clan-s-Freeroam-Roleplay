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

		public static int parseInt(string str, int length)
		{

			if(length < 1) return 0;
			if(str.LastIndexOf('.') != -1) return 0;

			bool negative = false;
			if (str[0] == '-')
			{
				negative = true;

				str = str.Substring(1);
				--length;
			}

			if (!isNumeric(str, length)) return 0;

			int endPos = length - 2, charInt = -1, sum = 0;

			for (int i = 0; i < length; i++)
			{
				charInt = ((int)str[i] - 48);
				if(endPos > -1) charInt *= exp(10, endPos--);

				sum += charInt;
			}

			return (negative) ? (0 - sum) : sum;
		}
	}
}
