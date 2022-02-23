namespace ET
{
    public static class CTrigonometric
    {
        private static int[] SinTable = new int[91]
        {
            0,
            17,
            34,
            52,
            69,
            87,
            104,
            121,
            139,
            156,
            173,
            190,
            207,
            224,
            241,
            258,
            275,
            292,
            309,
            325,
            342,
            358,
            374,
            390,
            406,
            422,
            438,
            453,
            469,
            484,
            500,
            515,
            529,
            544,
            559,
            573,
            587,
            601,
            615,
            629,
            642,
            656,
            669,
            681,
            694,
            707,
            719,
            731,
            743,
            754,
            766,
            777,
            788,
            798,
            809,
            819,
            829,
            838,
            848,
            857,
            866,
            874,
            882,
            891,
            898,
            906,
            913,
            920,
            927,
            933,
            939,
            945,
            951,
            956,
            961,
            965,
            970,
            974,
            978,
            981,
            984,
            987,
            990,
            992,
            994,
            996,
            997,
            998,
            999,
            999,
            1000
        };

        public static int IntSin(int value)
        {
            bool flag = false;
            if (value < 0)
            {
                flag = true;
                value = -value;
            }

            value %= 360;
            int num = value <= 270
                ? (value <= 180
                    ? (value <= 90 ? CTrigonometric.SinTable[value] : CTrigonometric.SinTable[180 - value])
                    : -CTrigonometric.SinTable[value - 180])
                : -CTrigonometric.SinTable[360 - value];
            if (flag)
                num = -num;
            return num;
        }

        public static int IntCos(int value)
        {
            return CTrigonometric.IntSin(90 + value);
        }

        public static int IntASin(int value)
        {
            if (1000 <= value)
                return 90;
            if (-1000 >= value)
                return -90;
            bool flag = false;
            if (value < 0)
            {
                flag = true;
                value = -value;
            }

            int num1 = 0;
            int num2 = 90;
            for (int index = (num1 + num2) / 2; index > num1; index = (num1 + num2) / 2)
            {
                if (CTrigonometric.IntSin((num1 + num2) / 2) > value)
                    num2 = index;
                else
                    num1 = index;
            }

            int num3 = num1;
            if (CTrigonometric.IntSin(num2) <= value)
                num3 = num2;
            if (flag)
                num3 = -num3;
            return num3;
        }

        public static int IntACos(int value)
        {
            if (1000 <= value)
                return 0;
            return -1000 >= value ? 180 : 90 - CTrigonometric.IntASin(value);
        }
    }
}