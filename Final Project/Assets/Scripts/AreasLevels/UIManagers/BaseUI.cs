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
    [SerializeField] protected Image _gameTimerBoard;
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
    public Transform ComboUIPosition;
    public Image[] FiveFills;
    public Image[] FiveIcons;
    public Image[] FourFills;
    public Image[] FourIcons;
    public Image[] ThreeFills;
    public Image[] ThreeIcons;



    public virtual void Start () {
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
}
