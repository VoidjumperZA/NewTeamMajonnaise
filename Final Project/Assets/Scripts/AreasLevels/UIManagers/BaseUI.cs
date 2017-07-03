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
    [SerializeField] public Text HookScoreText;
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
    protected Text[] cleanUpScoreText;
    [SerializeField]
    protected Text sumTotalHighScoreText;

    [Header("Ocean Bar")]
    [SerializeField]
    protected Slider oceanCleanUpProgressBar;    
    /*[SerializeField]
    protected GameObject oceanCleanUpBarChildFill;
    [SerializeField]
    protected GameObject oceanCleanUpBarChildBackground;
    [SerializeField]
    protected Image oceanCleanUpBarChildStripe;
    [SerializeField]
    protected Image oceanCleanUpBarChildFrameBackground;
    [SerializeField]
    protected GameObject oceanCleanUpBarChildText;*/
    /*[SerializeField]*/
    protected float timeOceanBarIsShown = 3;
    [SerializeField]
    protected float oceanBarFadeInSpeed;
    [SerializeField]
    protected float oceanBarFadeOutSpeed;
    [SerializeField]
    protected float oceanBarMovementSpeed;
    [SerializeField]
    protected Color oceanDirtyColour;
    [SerializeField]
    protected Color oceanCleanColour;
    [SerializeField]
    protected GameObject bubbleParticleEffect;
    //It is false when a piece of trash is collected
    protected float sliderValue = 100;
    [Header("WaterDistortion")]
    protected GameObject waterDistortion;


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
    public virtual bool GetTouchedDeployHook()
    {
        return false;
    }
    public virtual bool GetSecondTimeFishing()
    {
        return false;
    }
    
    public virtual void HandClickToggle(bool pBool)
    {
      
    }
    public virtual void IntroduceCombo()
    {

    }
    public virtual void StopFishGlow(bool pBool)
    {

    }
    public virtual void StopTrashGlow(bool pBool)
    {

    }
    public virtual void OnDropHook()
    {

    }
    public virtual void OnReelHook(bool pClickedButton = true)
    {

    }
    public virtual void OnHookFloorTouch()
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
        return HookScoreText;
    }
    public Text GetTotalScoreText()
    {
        return _totalScoreText;
    }
    public virtual void RestoreGlow(List<fish> fishes)
    {

    }
    public virtual void RestoreGlow(List<trash> trashes)
    {

    }
    public virtual void MakeGlow(float rate, List<trash> trashes)
    {

    }
    public virtual void MakeGlow(float rate, List<fish> fishes)
    {

    }
    public virtual bool GetTouchedReelUp()
    {
        return true;
    }
    public virtual void SetMovedBoat(bool moved)
    {

    }
    public void UpdateOceanProgressBar(bool pFirstTimeAnim)
    {
        //Get the percentage, set the bar value and the helper text
        int percentage = GameManager.Scorehandler.CalculatePercentageOceanCleaned(true);
        oceanCleanUpProgressBar.gameObject.transform.parent.gameObject.SetActive(true);
        sliderValue = 100 - percentage;
        //oceanCleanUpProgressBar.GetComponent<Slider>().value = 100 - percentage;
        oceanCleanUpProgressBar.GetComponentInChildren<Text>().text = percentage + "%";

        //Start a coroutine to disable after a while     
        StartCoroutine(ShowThenFadeOceanBar());
    }

   
    private IEnumerator ShowThenFadeOceanBar()
    {
        //GameObject go = Instantiate(bubbleParticleEffect, GameObject.FindGameObjectWithTag("BubbleParticleSpawn").transform.position, Quaternion.identity);
        //go.transform.SetParent(Camera.main.transform);
        //ParticleSystem.ShapeModule shapeModule = go.GetComponent<ParticleSystem>().shape;
        //shapeModule.radius -= Time.deltaTime;
        //Destroy(go, 4);
        yield return new WaitForSeconds(timeOceanBarIsShown);
        oceanCleanUpProgressBar.gameObject.transform.parent.gameObject.SetActive(false);
    }
}
