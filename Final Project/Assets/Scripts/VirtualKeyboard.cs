using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualKeyboard : MonoBehaviour {

    private GameObject entireKeyboard;
    private GameObject entireLeaderboard;
    [SerializeField] private TextAsset saveDataFile;
    private Text outputDisplay;
    [SerializeField] private int maxTextSize;
    private enum Keyboard {Q, W, E, R, T, Y, U, I, O, P, A, S, D, F, G, H, J, K, L, Z, X, C, V, B, N, M, BkSpace, Enter};
    private Keyboard key;
    private string currentText;
	// Use this for initialization
	void Start () {
        Leaderboard.Initialise(saveDataFile);
        currentText = "";
	}
	
	// Update is called once per frame
	void Update () {
        updateDisplay();
	}

    public void KeyPress(int pKey)
    {
        key = (Keyboard)pKey;
        if (key != Keyboard.BkSpace && key != Keyboard.Enter && currentText.Length < maxTextSize)
        {
            currentText += key.ToString();
        }
        if (key == Keyboard.BkSpace && currentText.Length > 0)
        {
            currentText = currentText.Substring(0, currentText.Length - 1);
        }
        if (key == Keyboard.Enter)
        {
            Leaderboard.SetName(currentText);
            passScoreToLeaderboard();
        }
        
        Debug.Log(currentText);
    }

    private void passScoreToLeaderboard()
    {
        Leaderboard.AddScore(GameManager.Scorehandler.GetHighscoreAchieved());
        ActivateKeyboard(false);
        ActivateLeaderboard(true);
    }

    public void AnimateKey(Image pAnimatedImage)
    {
        StartCoroutine(animate(pAnimatedImage));
    }

    private IEnumerator animate(Image pAnimatedImage)
    {
        pAnimatedImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.6f);

        pAnimatedImage.gameObject.SetActive(false);
    }

    private void updateDisplay()
    {
        if (outputDisplay != null)
        {
            outputDisplay.text = currentText;
        }
    }

    public void ActivateKeyboard(bool pState)
    {

        if (pState == true && GameManager.Boat.gameObject.GetComponent<Arguments>().getUsername() != "§")
        {
            entireKeyboard.SetActive(pState);
        }
        else if (pState == true && GameManager.Boat.gameObject.GetComponent<Arguments>().getUsername() == "§")
        {
            passScoreToLeaderboard();
        }
        else if (pState == false && GameManager.Boat.gameObject.GetComponent<Arguments>().getUsername() != "§")
        {
            entireKeyboard.SetActive(pState);
        }
       
    }

    private void ActivateLeaderboard(bool pState)
    {
        entireLeaderboard.SetActive(pState);
        if (pState == true)
        {
            GameManager.Levelmanager.GetComponent<LeaderboardUI>().PopulateLeaderboardUI();
        }
    }

    public void SetKeyboard(GameObject pKeyboard)
    {
        entireKeyboard = pKeyboard;
    }

    public void SetLeaderboard(GameObject pLeaderboard)
    {
        entireLeaderboard = pLeaderboard;
    }

    public void SetOutputDisplay(Text pOutputDisplay)
    {
        outputDisplay = pOutputDisplay;
    }

}
