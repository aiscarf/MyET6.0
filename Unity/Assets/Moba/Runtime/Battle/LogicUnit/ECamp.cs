namespace Scarf.Moba
{
    public enum ECamp
    {
        ENeutral = 0,
        EBlue = 1,
        ERed = 2,
        ETeam3 = 3,
        ETeam4 = 4,
        ETeam5 = 5,
        ETeam6 = 6,
        ETeam7 = 7,
        ETeam8 = 8,
        ETeam9 = 9,
        ETeam10 = 10,
    }

    public static class CampTool
    {
        public static ECamp GetEnemyCamp(ECamp eCamp)
        {
            switch (eCamp)
            {
                case ECamp.EBlue:
                    return ECamp.ERed;
                case ECamp.ERed:
                    return ECamp.EBlue;
                case ECamp.ENeutral:
                    return ECamp.ENeutral;
            }

            return ECamp.ENeutral;
        }
    }
}