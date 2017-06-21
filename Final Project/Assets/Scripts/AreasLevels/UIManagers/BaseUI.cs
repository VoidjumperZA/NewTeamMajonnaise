using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseUI : MonoBehaviour {
    public Canvas canvas;
    protected bool _onEnterScene = true;
    protected bool _onLeaveScene = true;
    [Header("Controls")]
    [SerializeField] protected Button _dropHook;
    [SerializeField] protected Button _reelHook;
    [Header("Game Time")]
    [SerializeField] protected Text _gameTimerText;
    [Header("Score")]
    [SerializeField] protected Image _totalScoreBoard;
    [SerializeField] protected Text _totalScoreText;
    [SerializeField] protected Text _hookScoreText;
    public GameObject ScoreUI; //the appearing score ui
    public Transform ScoreUIPosition; //where are we spawning that ui
    [Header("Shopping List")]
    [SerializeField] protected Image _shoppingList;
    [Header("Combo")]
    public GameObject ComboUI;
    public Image[] FiveFills;
    public Image[] FiveIcons;
    public Image[] FourFills;
    public Image[] FourIcons;
    public Image[] ThreeFills;
    public Image[] ThreeIcons;
    [Header("Transition")]
    public GameObject TransitionCurtain;
    [Header("HighScore")]
    [SerializeField]
    protected GameObject _totalScore;
    [SerializeField]
    protected GameObject highScoreBoard;
    [SerializeField]
    protected Image _replayLevelExplode;
    [SerializeField]
    protected Text rawHighScoreText;
    [SerializeField]
    protected Text cleanUpScoreText;
    [SerializeField]
    protected Text sumTotalHighScoreText;
    [Header("Ocean Bar")]
    [SerializeField]
    protected Slider oceanCleanUpProgressBar;
    [SerializeField]
    protected GameObject oceanCleanUpBarChildFill;
    [SerializeField]
    protected GameObject oceanCleanUpBarChildBackground;
    [SerializeField]
    protected Image oceanCleanUpBarChildStripe;
    [SerializeField]
    protected Image oceanCleanUpBarChildFrameBackground;
    [SerializeField]
    protected GameObject oceanCleanUpBarChildText;
    [SerializeField]
    protected float timeOceanBarIsShown;
    [SerializeField]
    protected float oceanBarFadeInSpeed;
    [SerializeField]
    protected float oceanBarFadeOutSpeed;
    [SerializeField]
    protected float oceanBarMovementSpeed;



    public virtual void Start ()
    {

        //Debug.Log("BaseUI - Start();");
    }
    public virtual void Update()
    {

    }
    public virtual void OnEnterScene()
    {

    }
    public virtual void OnLeaveScene()
    {

    }
    public static void SetActive(bool pBool, params GameObject[] pGameObjects)
    {
        foreach (GameObject gO in pGameObjects) gO.SetActive(pBool);
    }
    public void SetActiveButtons(bool pBool, params Button[] pButtons)
    {
        foreach (Button button in pButtons) button.gameObject.SetActive(pBool);
    }
    public virtual void HookScoreToggle(bool pBool)
    {

    }
    public virtual void HookSwipeAnimToggle(bool pBool)
    {

    }
    public virtual void HandClickToggle(bool pBool)
    {
      
    }
    public virtual void OnDropHook()
    {

    }
    public virtual void OnReelHook()
    {

    }
    public virtual void EnterSceneTransition()
    {

    }
    public virtual void LeaveSceneTransition()
    {

    }
    public Text GetHookScoreText()
    {
        return _hookScoreText;
    }
    public Text GetTotalScoreText()
    {
        return _totalScoreText;
    }
}
