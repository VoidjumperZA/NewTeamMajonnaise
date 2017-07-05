using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq.Expressions;
using System.Linq;
using System.IO;

public static class Leaderboard
{
    private const int MAX_LOCAL_DATABASE_ENTRIES = 100;
    private static List<KeyValuePair<string, int>> sortedScores = new List<KeyValuePair<string, int>>();
    private static string playerName;
    private static Dictionary<string, int> leaderboard = new Dictionary<string, int>();
    private static bool initialised = false;
    private static TextAsset saveFile;
    private static char endlineSpacer = '~';
    private static char tabSpacer = '^';
    private static int playerPosition;
    //
    public static void Initialise(TextAsset pSaveFile)
    {
        if (initialised == false)
        {
            initialised = true;
            loadLeaderboardFromFile(pSaveFile);
        }

    }

    public static void SetName(string pName)
    {
        if (GameManager.Boat.GetComponent<Arguments>().getUsername() != "§")
        {
            playerName = pName;
        }
        else
        {
            playerName = GameManager.Boat.GetComponent<Arguments>().getUsername();
        }
    }

    public static void AddScore(int pScore)
    {
        if (leaderboard.ContainsKey(playerName))
        {
            leaderboard[playerName] = pScore;
        }
        else
        {
            //if we have less than a constantly set number of entries: prevents sort from slowing down
            if (leaderboard.Count < MAX_LOCAL_DATABASE_ENTRIES)
            {
                leaderboard.Add(playerName, pScore);
            }
            else
            {
                //Remove the last element in the leaderboard, i.e. the lowest score
                leaderboard.Remove(sortedScores[sortedScores.Count].Key);
            }
        }
        if (leaderboard.Count > 1)
        {
            sortScores();
        }
        saveToFile();
        GameManager.Boat.StartCoroutine(GameManager.Boat.GetComponent<DBconnection>().UploadScore(GameManager.Boat.GetComponent<Arguments>().getUserID(), GameManager.Boat.GetComponent<Arguments>().getGameID(), pScore));
        //search if username exists
        //if so replace score
        //if nnot check entries < 100
        //if so add
        //if not delete last index (unless last index = TOP) then delete second last
        //save this as text file
        //read text file
        //send backbone score

    }

    private static void sortScores()
    {
        Debug.Log("UNSORTED");
        sortedScores = leaderboard.ToList();
        for (int i = 0; i < sortedScores.Count; i++)
        {
            Debug.Log("score: " + sortedScores[i].Value);
        }

        Debug.Log("SORTED");
        sortedScores.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
        List<KeyValuePair<string, int>> temp = new List<KeyValuePair<string, int>>();
        for (int i = 0; i < sortedScores.Count; i++)
        {
            temp.Add(sortedScores[i]); 
        }

        for (int i = 0; i < sortedScores.Count; i++)
        {
            sortedScores[i] = temp[(temp.Count - 1) - i];
            Debug.Log("" + sortedScores[i].Key + ": " + sortedScores[i].Value);
            if (sortedScores[i].Key == playerName)
            {
                playerPosition = i;
            }
        }
    }

    private static void saveToFile()
    {
        string path = "Assets/Resources/saveData.txt";        
        string data = "";
        for (int i = 0; i < sortedScores.Count; i++)
        {
            data += sortedScores[i].Key + tabSpacer + sortedScores[i].Value + endlineSpacer;
        }
        File.WriteAllText(path, data);

        //UnityEditor.AssetDatabase.ImportAsset(path);
        TextAsset file = (TextAsset)Resources.Load("saveData");
        Debug.Log(file.text);
        
    }

    private static void loadLeaderboardFromFile(TextAsset pSaveFile)
    {
        //string path = "Assets/Resources/saveData.txt";
        //UnityEditor.AssetDatabase.ImportAsset(path);
        TextAsset file = pSaveFile;
        if (file.text != "")
        {
            Debug.Log("Reading from file");
            string[] lines = file.text.Split(endlineSpacer);
            for (int i = 0; i < lines.Length - 1; i++)
            {
                Debug.Log(lines[i]);
                string[] elements = lines[i].Split(tabSpacer);
                int value = 0;
                System.Int32.TryParse(elements[1], out value);
                leaderboard.Add(elements[0], value);
            }
        }

    }

    public static string[] GetTopEntries(int pAmount, out int[] pValues)
    {
        if (pAmount >= MAX_LOCAL_DATABASE_ENTRIES)
        {
            pAmount = MAX_LOCAL_DATABASE_ENTRIES;
        }
        if (sortedScores.Count < pAmount)
        {
            pAmount = sortedScores.Count;
        }

        string[] topNames = new string[pAmount];
        int[] topScores = new int[pAmount];
        for (int i = 0; i < pAmount; i++)
        {
            topNames[i] = sortedScores[i].Key;
            topScores[i] = sortedScores[i].Value;
        }
        pValues = topScores;
        return topNames;
    }

    public static string GetPlayerName()
    {
        return playerName;
    }

    public static int GetPlayerPosition()
    {
        return playerPosition;
    }
}
