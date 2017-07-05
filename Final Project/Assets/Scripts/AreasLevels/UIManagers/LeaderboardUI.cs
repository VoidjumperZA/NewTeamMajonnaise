using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Text numberToInstantiate;
    [SerializeField] private Text nameToInstantiate;
    [SerializeField] private Text playerNumberText;
    [SerializeField] private Text playerNameText;
    [SerializeField] private Transform numberPosition;
    [SerializeField] private Text namePosition;
    [SerializeField] private float verticalOffset;
    [SerializeField] private Color playerPosInLeaderboard;
    [SerializeField] private Color playerNameColour;
    [SerializeField] private int entries;
    private string[] names;
    private int[] scores;
    private Text[] numbers;
    // Use this for initialization
    void Start ()
    {
        numbers = new Text[entries];
        names = Leaderboard.GetTopEntries(entries, out scores);
        for (int i = 0; i < entries; i++)
        {
            string numberText = "" + (i + 1) + ".";
            createElement(i, numberToInstantiate, numberText, false);
            createElement(i, nameToInstantiate, names[i] + "   " + scores[i], true);
        }
        playerNameText.color = playerNameColour;
        playerNumberText.color = playerNameColour;
        playerNameText.text = Leaderboard.GetPlayerName();
        playerNumberText.text = "" + Leaderboard.GetPlayerPosition() + "."; 
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void createElement(int i, Text original, string pText, bool checkColour)
    {
        Text newElement = Instantiate(original, canvas.transform);
        Vector3 pos = new Vector3();
        pos = numberPosition.position;
        pos.y -= verticalOffset * i;
        newElement.transform.position = pos;
        newElement.text = pText;
        
        if (checkColour == true)
        {
            if (pText == Leaderboard.GetPlayerName())
            {
                newElement.color = playerPosInLeaderboard;
                numbers[i].color = playerPosInLeaderboard;
            }
        }
        else
        {
            if (numbers[i] != null)
            {
                numbers[i] = newElement;
            }
        }
    }


}
