using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private GameObject LeaderboardObject;
    [SerializeField] private Text numberToInstantiate;
    [SerializeField] private Text nameToInstantiate;
    [SerializeField] private Text playerNumberText;
    [SerializeField] private Text playerNameText;
    [SerializeField] private Transform numberPosition;
    [SerializeField] private Transform namePosition;
    [SerializeField] private float verticalOffset;
    [SerializeField] private Color playerPosInLeaderboard;
    [SerializeField] private Color playerNameColour;
    [SerializeField] private int entries;
    [SerializeField] private GameObject entireKeyboard;
    [SerializeField] private GameObject entireLeaderboard;
    [SerializeField] private Text outputDisplay;
    private string[] names;
    private int[] scores;
    private Text[] numbers;
    // Use this for initialization
    void Start()
    {
        LeaderboardObject.GetComponent<VirtualKeyboard>().SetKeyboard(entireKeyboard);
        LeaderboardObject.GetComponent<VirtualKeyboard>().SetLeaderboard(entireLeaderboard);
        LeaderboardObject.gameObject.GetComponent<VirtualKeyboard>().SetOutputDisplay(outputDisplay);

        PopulateLeaderboardUI();
    }

    // Update is called once per frame
    void Update() {

    }

    public void BeginLeaderboardSequence()
    {
        GameManager.Levelmanager.UI.HideHighScoreBoard();
        LeaderboardObject.GetComponent<VirtualKeyboard>().ActivateKeyboard(true);
    }

    public void PopulateLeaderboardUI()
    {
        names = Leaderboard.GetTopEntries(entries, out scores);
        numbers = new Text[names.Length];
        for (int i = 0; i < names.Length; i++)
        {
            string numberText = "" + (i + 1) + ".";
            createElement(i, numberToInstantiate, numberText, numberPosition, false);
            string spaces = "   ";
            for (int j = 0; j < (10 - names[i].Length); j++)
            {
                spaces += " ";
            }
            string name = names[i] + spaces + scores[i];
            createElement(i, nameToInstantiate, name, namePosition, true);
        }
        playerNameText.color = playerNameColour;
        playerNumberText.color = playerNameColour;
        playerNameText.text = Leaderboard.GetPlayerName() + "   " + GameManager.Scorehandler.GetHighscoreAchieved();
        playerNumberText.text = "" + (Leaderboard.GetPlayerPosition() + 1) + ".";
        numberToInstantiate.enabled = false;
        nameToInstantiate.enabled = false;
    }

    private void createElement(int i, Text original, string pText, Transform pPos, bool checkColour)
    {
        Text newElement = Instantiate(original, entireLeaderboard.transform);      
        Vector3 pos = new Vector3();
        pos = pPos.position;
        pos.y -= verticalOffset * i;
        newElement.transform.position = pos;
        newElement.text = pText;
        newElement.GetComponent<Text>().enabled = true;
        
        if (checkColour == true)
        {
            if (names[i] == Leaderboard.GetPlayerName())
            {
                newElement.color = playerPosInLeaderboard;
                numbers[i].color = playerPosInLeaderboard;
            }
        }
        else
        {
            if (numbers != null)
            {
                numbers[i] = newElement;
            }
        }
    }


}
