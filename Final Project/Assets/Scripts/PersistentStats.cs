using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PersistentStats 
{
    private static Dictionary<int, int> oceanPercentPerLevel = new Dictionary<int, int>();
    private static float timeBank = 0.0f;
    private static int persistentScore = 0;

    /// <summary>
    /// Saves the current time, banked score and ocean clenliness percent
    /// </summary>
    public static void SaveAllStats(float pTimeLeft, int pBankedScore, int pOceanPercent, int pAreaIndex)
    {
        oceanPercentPerLevel.Add(pOceanPercent, pAreaIndex);
        persistentScore += pBankedScore;
        timeBank = 0.0f;
        timeBank += pTimeLeft;
    }

    public static float GetTimeLeftOfPastScene()
    {
        return timeBank;
    }

    public static int GetPersitentScore()
    {
        return persistentScore;
    }
}
