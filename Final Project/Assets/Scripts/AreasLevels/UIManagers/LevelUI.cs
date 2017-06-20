using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : BaseUI {

    public override void Start () {
        // Controls
        SetActive(false, _dropHook.gameObject, _reelHook.gameObject);
        // Game Time
        SetActive(false, _gameTimerText.gameObject);
        // Score
        SetActive(false, _totalScoreBoard.gameObject, _totalScoreText.gameObject, _hookScoreText.gameObject);
        // Shopping List
        SetActive(false, _shoppingList.gameObject);
        // Combo
        SetActive(false, ComboUI);
        //Debug.Log("LevelUI - Start();");
        GameManager.Scorehandler.SetTextColours(_hookScoreText, _totalScoreText);
    }
    public override void Update()
    {
        // Game Time
        _gameTimerText.text = GameManager.Gametimer.GetFormattedTimeLeftAsString();
        // Score
        _totalScoreText.text = GameManager.Scorehandler.BankedScore + "";

        _hookScoreText.text = GameManager.Scorehandler.HookScore + "";
        _hookScoreText.transform.position = GameManager.Scorehandler.HookScorePosition();
        if (basic.HasGameEnded() == true)
        {
            displayHighScoreBoard();
        }
    }
    public override void OnDropHook()
    {
        if (!GameManager.Boat.CanDropHook()) return;

        SetActive(false, _dropHook.gameObject);
        SetActive(true, _reelHook.gameObject, _hookScoreText.gameObject);
        GameManager.Boat.SetState(boat.BoatState.Fish);
    }
    public override void OnReelHook()
    {
        SetActive(true, _dropHook.gameObject);
        SetActive(false, _reelHook.gameObject, _hookScoreText.gameObject);
        GameManager.Hook.SetState(hook.HookState.Reel);
    }
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
        SetActive(true, _shoppingList.gameObject);
        // Combo
        SetActive(true, ComboUI);
        // Scene Transition Curtain
        EnterSceneTransition();


        _onEnterScene = false;
    }
    private void displayHighScoreBoard()
    {
        rawHighScoreText.text = "" + GameManager.Scorehandler.GetBankedScore();
        int percentage = GameManager.Scorehandler.CalculatePercentageOceanCleaned(true);
        int cleanUpScore = percentage * GameManager.Scorehandler.GetTrashScoreModifier();
        cleanUpScoreText.text = "" + percentage + "% x" + GameManager.Scorehandler.GetTrashScoreModifier();
        sumTotalHighScoreText.text = "" + (GameManager.Scorehandler.GetBankedScore() + cleanUpScore);
        highScoreBoard.SetActive(true);
    }
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
        SetActive(false, _gameTimerText.gameObject);
        // Score
        SetActive(false, _totalScoreBoard.gameObject, _totalScoreText.gameObject, _hookScoreText.gameObject);
        // Shopping List
        SetActive(false, _shoppingList.gameObject);
        // Combo
        SetActive(false, ComboUI);
        // Scene Transition Curtain
        LeaveSceneTransition();
        _onLeaveScene = false;
    }
    public override void HookScoreToggle(bool pBool)
    {
        _hookScoreText.gameObject.SetActive(pBool);
    }
    public override void LeaveSceneTransition()
    {
        TransitionCurtain.GetComponent<Transition>().DownWards();
    }
    public override void EnterSceneTransition()
    {
        TransitionCurtain.GetComponent<Transition>().UpWards();
    }

}
