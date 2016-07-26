using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace makemesmarter.Helpers
{
    public static class MoodCalculator
    {
       public static string getMoodString(double moodvalue)
        {
            var floatindex = (int)moodvalue / 10;
            var possibleMood = (Constants.PossibleMoods)floatindex;
            return GetNextMessageString(possibleMood);
        }

        private static string GetNextMessageString(Constants.PossibleMoods mood)
        {
            switch (mood)
            {
                case Constants.PossibleMoods.SAD:
                    return "SORRY TO HEAR THAT";
                case Constants.PossibleMoods.GRIEF:
                    return "Oh my god ! that's terrible ";
                case Constants.PossibleMoods.AWFULL:
                    return "That's awful";
                case Constants.PossibleMoods.NONE:
                    return "OK !!";
                case Constants.PossibleMoods.NEUTRAL:
                    return "OK..";
                case Constants.PossibleMoods.HAPPY:
                    return "Great!!!!";
                case Constants.PossibleMoods.VERYHAPPY:
                    return "AWESOME !!!";
                case Constants.PossibleMoods.JUBLIENT:
                    return "I am on heaven!!! party!!!";
            }

            return "NONE";
        }
    }
}