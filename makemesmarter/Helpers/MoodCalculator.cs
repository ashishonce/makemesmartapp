using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace makemesmarter.Helpers
{
    enum PossibleMoods
    {
        GRIEF,
        SAD,
        AWFULL,
        NONE,
        NEUTRAL,
        HAPPY,
        VERYHAPPY,
        JUBLIENT,
    }

    public static class MoodCalculator
    {
       public static string getMoodString(float moodvalue)
        {
            var floatindex = (int)moodvalue / 10;
            var possibleMood = (PossibleMoods)floatindex;
            return GetNextMessageString(possibleMood);
        }

        private static string GetNextMessageString(PossibleMoods mood)
        {
            switch (mood)
            {
                case PossibleMoods.SAD:
                    return "SORRY TO HEAR THAT";
                case PossibleMoods.GRIEF:
                    return "Oh my god ! that's terrible ";
                case PossibleMoods.AWFULL:
                    return "That's awfull";
                case PossibleMoods.NONE:
                    return "OK !!";
                case PossibleMoods.NEUTRAL:
                    return "OK..";
                case PossibleMoods.HAPPY:
                    return "Great!!!!";
                case PossibleMoods.VERYHAPPY:
                    return "AWESOME !!!";
                case PossibleMoods.JUBLIENT:
                    return "I am on heaven!!! party!!!";
            }

            return "NONE";
        }
    }
}