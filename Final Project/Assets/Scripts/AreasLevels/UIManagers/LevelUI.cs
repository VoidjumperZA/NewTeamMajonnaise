using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelUI : BaseUI
{
    private bool highscoreShown = false;
    public override void Start()
    {

        waterDistortion = GameManager.Camerahandler.GetWaterDropEffect();
        oceanCleanUpProgressBar.GetComponentInChildren<Text>().text = 0 + "%";
        oceanCleanUpProgressBar.gameObject.transform.parent.gameObject.SetActive(false);
        /*oceanCleanUpBarChildFill.GetComponent<Image>().CrossFadeAlpha(0.0f, 0.0f, false);
        oceanCleanUpBarChildBackground.GetComponent<Image>().CrossFadeAlpha(0.0f, 0.0f, false);
        oceanCleanUpBarChildFrameBackground.CrossFadeAlpha(0.0f, 0.0f, false);
        oceanCleanUpBarChildStripe.CrossFadeAlpha(0.0f, 0.0f, false);
        oceanCleanUpBarChildText.GetComponent<Text>().CrossFadeAlpha(0.0f, 0.0f, false);*/
        // Controls
        SetActive(false, _dropHook.gameObject, _reelHook.gameObject);
        // Game Time
        SetActive(false, _gameTimerText.gameObject);
        // Score
        SetActive(false, _totalScoreBoard.gameObject, _totalScoreText.gameObject, HookScoreText.gameObject);
        // Shopping List
        SetActive(false, _shoppingList.gameObject);
        // Combo
        SetActive(false, ComboUI);
        //Debug.Log("LevelUI - Start();");
        GameManager.Scorehandler.SetTextColours(HookScoreText, _totalScoreText);
    }

    //
    public override void Update()
    {
        // Game Time
        _gameTimerText.text = GameManager.Gametimer.GetFormattedTimeLeftAsString();
        // Score
        _totalScoreText.text = GameManager.Scorehandler.BankedScore + "";

        HookScoreText.text = GameManager.Scorehandler.HookScore + "";
        HookScoreText.transform.position = GameManager.Scorehandler.HookScorePosition();

        if (GameManager.Levelmanager.HasGameEnded() == true && highscoreShown == false)
        {
            highscoreShown = true;
            if (SceneManager.GetActiveScene().buildIndex != 4)
            {
                GameManager.Scorehandler.SavePersitentStats(GameManager.NextScene - 1);
            }
            else
            {
                GameManager.Scorehandler.SavePersitentStats(GameManager.NextScene);
            }
            
            displayHighScoreBoard();
        }
    }

    //
    public override void OnDropHook()
    {
        if (GameManager.Levelmanager.HasGameEnded()) return;

        if (!GameManager.Boat.CanDropHook()) return;

        SetActive(false, _dropHook.gameObject);
        SetActive(true, _reelHook.gameObject/*, HookScoreText.gameObject*/);
        if (waterDistortion.activeSelf == true)
        {
            waterDistortion.GetComponent<WaterdropDistortion>().Deactivate();
        }

        GameManager.Boat.SetState(boat.BoatState.Fish);
    }

    //
    public override void OnReelHook()
    {
        if (GameManager.Levelmanager.HasGameEnded()) return;

        SetActive(true, _dropHook.gameObject);
        SetActive(false, _reelHook.gameObject/*, HookScoreText.gameObject*/);
        GameManager.Hook.SetState(hook.HookState.Reel);
    }

    //
    public override void OnEnterScene()
    {
        if (!_onEnterScene)
        {
            Debug.Log("OnEnterScene was already called once for current instance");
            return;
        }
        // Controls
        SetActive(true, _dropHook.gameObject);
        // Game Time
        GameManager.Gametimer.BeginCountdown();
        SetActive(true, _gameTimerText.gameObject);

        // Score
        SetActive(true, _totalScoreBoard.gameObject, _totalScoreText.gameObject);
        // Shopping List
        //SetActive(true, _shoppingList.gameObject);
        // Combo
        SetActive(true, ComboUI);
        // Scene Transition Curtain
        EnterSceneTransition();

        _onEnterScene = false;
    }

    //
    private void displayHighScoreBoard()
    {
        //First clean the defaults
        for (int i = 0; i < cleanUpScoreText.Length; i++)
        {
            cleanUpScoreText[i].text = "";
        }

        //Possibly skip the tutorial feild0
        int textIndex = 0;
        rawHighScoreText.text = "" + GameManager.Scorehandler.GetBankedScore();
        int[] percentage = new int[cleanUpScoreText.Length];
        int[] cleanUpScore = new int[cleanUpScoreText.Length];
        int cleanUpSum = 0;
        for (int i = 0; i < cleanUpScoreText.Length; i++)
        {
            percentage[i] = PersistentStats.GetPercentForArea(i + 1);// GameManager.Scorehandler.CalculatePercentageOceanCleaned(true);
            Debug.Log("For text " + (i + 1) + " I got a percent from Persitent of " + percentage[i]);
            if (percentage[i] != -1)
            {
                cleanUpScore[i] = percentage[i] * GameManager.Scorehandler.GetTrashScoreModifier();
                Color colour = Color.Lerp(oceanDirtyColour, oceanCleanColour, percentage[i] * 0.01f);
                cleanUpScoreText[textIndex].text = " (" + (textIndex + 1) + ") " + percentage[i] + "%  x" + GameManager.Scorehandler.GetTrashScoreModifier();
                cleanUpScoreText[textIndex].color = colour;
                cleanUpSum += cleanUpScore[i];
                textIndex++;
            }

        }
        sumTotalHighScoreText.text = "" + (GameManager.Scorehandler.GetBankedScore() + cleanUpSum);
        highScoreBoard.SetActive(true);
    }

    //
    public override void OnLeaveScene()
    {
        if (!_onLeaveScene)
        {
            Debug.Log("OnLeaveScene was already called once for current instance");
            return;
        }
        // Controls
        SetActive(false, _dropHook.gameObject);
        // Game Time
        GameManager.Gametimer.PauseCountdown();
        SetActive(false, _gameTimerText.gameObject);
        // Score
        SetActive(false, _totalScoreBoard.gameObject, _totalScoreText.gameObject, HookScoreText.gameObject);
        // Shopping List
        SetActive(false, _shoppingList.gameObject);
        // Combo
        SetActive(false, ComboUI);
        // Scene Transition Curtain
        LeaveSceneTransition();
        _onLeaveScene = false;
    }

    /*
    public void UpdateOceanProgressBar(bool pFirstTimeAnim)
    {
        //Get the percentage, set the bar value and the helper text
        int percentage = GameManager.Scorehandler.CalculatePercentageOceanCleaned(true);
        oceanCleanUpProgressBar.gameObject.transform.parent.gameObject.SetActive(true);
        oceanCleanUpProgressBar.GetComponent<Slider>().value = 100 - percentage;
        oceanCleanUpProgressBar.GetComponentInChildren<Text>().text = percentage + "%";

        //Start a coroutine to disable after a while     
        StartCoroutine(ShowThenFadeOceanBar());
        /*if (pFirstTimeAnim)
        {
            oceanCleanUpProgressBar.GetComponent<OceanCleanUpUIAnimation>().AnimateFirstTimeMovement();
        }
    }*/

    //
    public override void HookScoreToggle(bool pBool)
    {
        HookScoreText.gameObject.SetActive(pBool);
    }

    //
    public override void LeaveSceneTransition()
    {
        TransitionCurtain.GetComponent<Transition>().DownWards();
    }

    //
    public override void EnterSceneTransition()
    {
        TransitionCurtain.GetComponent<Transition>().UpWards();
    }
    /*
    private IEnumerator ShowThenFadeOceanBar()
    {
        GameObject go = Instantiate(bubbleParticleEffect, GameObject.FindGameObjectWithTag("BubbleParticleSpawn").transform.position, Quaternion.identity);
        go.transform.SetParent(Camera.main.transform);
        //ParticleSystem.ShapeModule shapeModule = go.GetComponent<ParticleSystem>().shape;
        //shapeModule.radius -= Time.deltaTime;
        Destroy(go, 4);
        yield return new WaitForSeconds(timeOceanBarIsShown);
        oceanCleanUpProgressBar.gameObject.transform.parent.gameObject.SetActive(false);
        /*
        //Immediately show the bar
        oceanCleanUpBarChildFill.GetComponent<Image>().CrossFadeAlpha(1.0f, oceanBarFadeInSpeed, false);
        oceanCleanUpBarChildBackground.GetComponent<Image>().CrossFadeAlpha(1.0f, oceanBarFadeInSpeed, false);
        oceanCleanUpBarChildFrameBackground.CrossFadeAlpha(1.0f, oceanBarFadeInSpeed, false);
        oceanCleanUpBarChildStripe.CrossFadeAlpha(1.0f, oceanBarFadeInSpeed, false);
        oceanCleanUpBarChildText.GetComponent<Text>().CrossFadeAlpha(1.0f, oceanBarFadeInSpeed, false);

        //Show for a small time
        yield return new WaitForSeconds(timeOceanBarIsShown);

        //Fade out
        oceanCleanUpBarChildFill.GetComponent<Image>().CrossFadeAlpha(0.0f, oceanBarFadeOutSpeed, false);
        oceanCleanUpBarChildBackground.GetComponent<Image>().CrossFadeAlpha(0.0f, oceanBarFadeOutSpeed, false);
        oceanCleanUpBarChildFrameBackground.CrossFadeAlpha(0.0f, oceanBarFadeOutSpeed, false);
        oceanCleanUpBarChildStripe.CrossFadeAlpha(0.0f, oceanBarFadeOutSpeed, false);
        oceanCleanUpBarChildText.GetComponent<Text>().CrossFadeAlpha(0.0f, oceanBarFadeOutSpeed, false);
    }*/

}
