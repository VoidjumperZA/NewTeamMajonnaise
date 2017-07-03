using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : BaseUI
{

    [Header("Animations")]
    //Animations for swiping hand
    [SerializeField]
    public Image arrows;
    [SerializeField]
    public Image handMove;
    private bool goLeft = false;
    

    //Animation step 1
    [SerializeField]
    private Image _handClickNoDrops;

    //Animations for deploy//reel up hook buttons
    [SerializeField]
    private Image _handClick;
    [SerializeField]
    private Image _bubbleMoving;
    private Color transparent;
    private Color opaque;
    [SerializeField]
    private Image _deployHookImage;
    [SerializeField]
    private Image _reelHookImage;

    [Header("Hide screen")]
    [SerializeField]
    private Image _hide1;

    //private Animator _dropHookAnim;
    //private Animator _reelHookAnim;
    // [SerializeField]
    // private Sprite _bubbleImage;


    //Conditions for showing UI in order
    private bool boatStopped = false;
    private bool touchedScreen = false;
    private bool touchedDeployHook = false;
    private bool _secondTimefishing;
    private bool touchedReelUpHook;
    private bool movedBoat;
    private bool gotTrash;
    private bool gotFish;
    private bool _fadeIn;

    //Activate/Deactivate reel up and deploy hook
    private  bool ReelUpActive;
    private  bool DeployActive;

    private float _rateOfGlow = 0.4f;
    //SwipeAnimation
    /*int index;
    List<float> positions = new List<float>(2);*/

    public override void Start()
    {
        
        transparent = _deployHookImage.color;
        transparent.a = 0f;

        opaque= _deployHookImage.color;
        opaque.a = 1f;
        
       
        //Buttons
        ReelUpActive = false;
        DeployActive = false;

        /*
        _dropHookAnim = _dropHook.GetComponent<Animator>();
        if (!_dropHookAnim) Debug.Log("Couldn't find animation in deployHook");
        _reelHookAnim = _reelHook.GetComponent<Animator>();
        if (!_reelHookAnim) Debug.Log("Couldn't find animation in deployHook");*/

        _secondTimefishing = false;
        touchedReelUpHook = false;
        movedBoat = false;
        gotTrash = false;
        gotFish = false;
        _fadeIn = true;
        //SwipeAnimation
        //index = 0;
        
        // Controls
        SetActive(false, _dropHook.gameObject, _reelHook.gameObject);
        // Game Time
        SetActive(false, _gameTimerText.gameObject);
        // Score
        SetActive(false, _totalScoreBoard.gameObject, _totalScoreText.gameObject, HookScoreText.gameObject);
        // Shopping List
        SetActive(false, _shoppingList.gameObject);
        //Animations
        SetActive(false, handMove.gameObject,_handClick.gameObject, arrows.gameObject,_handClickNoDrops.gameObject,_hide1.gameObject);
        SetActive(false, _bubbleMoving.gameObject);
       
        //Debug.Log("TutorialUI - Start();");
    }
    public override void Update()
    {
        //Activates or deactivates the buttons for deploting the hook and reeling it up
        SetActive(DeployActive, _dropHook.gameObject);
        SetActive(ReelUpActive, _reelHook.gameObject);

        //The tutorial starts once the boat stops in the middle of the level
        if (boatStopped) { 
        
            //Game Time
            _gameTimerText.text = GameManager.Gametimer.GetFormattedTimeLeftAsString();

            //Score
            _totalScoreText.text = GameManager.Scorehandler.BankedScore + "";
            HookScoreText.text = GameManager.Scorehandler.HookScore + "";
            HookScoreText.transform.position = GameManager.Scorehandler.HookScorePosition();

            //Fish and trash glow until you catch it for the first time
            if (!gotTrash)
            {
                //GameObject[] trash = GameObject.FindGameObjectsWithTag("Trash");
                if (SeafloorSpawning.SpawnedTrash != null) MakeGlow(_rateOfGlow, SeafloorSpawning.SpawnedTrash);
            }
            else
            {
                RestoreGlow(SeafloorSpawning.SpawnedTrash);
            }
            if (!gotFish && !_secondTimefishing)
            {

                //GameObject[] fish = GameObject.FindGameObjectsWithTag("Fish");
                if (FishSpawn.SpawnedFish != null) MakeGlow(_rateOfGlow, FishSpawn.SpawnedFish);
            }
            else
            {
                RestoreGlow(FishSpawn.SpawnedFish);
            }

            //Updating trash slider
            if(oceanCleanUpProgressBar.GetComponent<Slider>().value > sliderValue)
            {
                oceanCleanUpProgressBar.GetComponent<Slider>().value -= Time.deltaTime * 10f;
            }

            //Introducing features step by step
            //Step 1 - Radar introduced
            if (!touchedScreen)
            {
                //Check if the player have touched the screen
                if (Input.GetMouseButton(0) || mouse.Touching())
                {
                    touchedScreen = true;
                    StartCoroutine(FadeOut());
                    
                }
                else
                {
                    if (_fadeIn)
                    {
                        StartCoroutine(FadeIn());
                        _fadeIn = false;
                    }
                    
                }
            }//Step 2 - Deploy hook button introduced
            else if (!touchedDeployHook)

            {    
                //Activates button and hand animation
                DeployActive = true;
                _deployHookImage.color = transparent;
                SetActive(true, _bubbleMoving.gameObject);
                _handClick.transform.position = _dropHook.transform.position + new Vector3(10, -10, 0);
                SetActive(true, _handClick.gameObject);

                //Positions hand for swiping animation
                SetScreenPosition(handMove.gameObject, GameManager.Hook.gameObject, new Vector3(0, 0, 0));

            }//Step 3 - Fishing for the first time, introducing trash 
            //Step 4 - Introducing combos
            else if (!_secondTimefishing)
            {
                SetActive(false, _handClick.gameObject);

                //Show swiping animation to introduce how to move the hook
                SetScreenPosition(arrows.gameObject, GameManager.Hook.gameObject, new Vector3(10, -20, 0));
                AnimateSwipeHand(GetScreenPosition(GameManager.Hook.gameObject, new Vector3(0, -20, 0)), 0.3f, 75.0f);

                //Make animation disappear when the player touches the screen
                if (Input.GetMouseButton(0) || mouse.Touching())
                {
                    SetActive(false, arrows.gameObject, handMove.gameObject);

                }
                
              
                //Show combos for the first time
            }
            else if (!touchedReelUpHook)
            {

                //GameObject[] fish = ComboUI.GetComponent<Combo>().GetCurrentType();

                MakeGlow(_rateOfGlow, FishSpawn.GetFishOfType(GameManager.combo.GetCurrentType()));
                              
            }
            else if (!movedBoat)
            {
                /*
              //_reelHookAnim.enabled = false;
              SetActive(false, _bubbleMoving.gameObject);
              _reelHookImage.color = opaque;

              //_reelHook.GetComponent<Image>().sprite = _bubbleImage;
              SetScreenPosition(arrows.gameObject, GameManager.Boat.gameObject, new Vector3(0, 0, 0));
              AnimateSwipeHand(GetScreenPosition(GameManager.Boat.gameObject, new Vector3(0, -20, 0)), 0.3f, 75.0f);
              SetActive(true, arrows.gameObject,handMove.gameObject);
              */
                
            }
            else
            {
                SetActive(false, arrows.gameObject, handMove.gameObject);
            }
        }
    }
    public override void OnDropHook()
    {
        if (GameManager.Levelmanager.HasGameEnded()) return;

        if (!GameManager.Boat.CanDropHook()) return;

        if (!touchedDeployHook)
        {
            touchedDeployHook = true;
            //_dropHookAnim.enabled = false;
            SetActive(false, _bubbleMoving.gameObject);
            _deployHookImage.color = opaque;
            //_dropHook.GetComponent<Image>().sprite = _bubbleImage;
        }/*else if (!_firstTimeFishing)
        {
            Debug.Log("First time fishing true");
            _firstTimeFishing = true;
        }*/else if (!_secondTimefishing)
        {
            Debug.Log("Second time fishing true");
            _secondTimefishing = true;
        }
        else
        {
            if (!touchedReelUpHook)
            {
                //_reelHookAnim.enabled = true;
                SetActive(true, _bubbleMoving.gameObject);
                _reelHookImage.color = transparent;
                SetActive(true, _handClick.gameObject);
            }

            ReelUpActive = true;
        }
        DeployActive = false;
        //SetActive(false, _dropHook.gameObject);
        
        GameManager.Boat.SetState(boat.BoatState.Fish);
    }
    public override void OnReelHook(bool pClickedButton = true)
    {
        if (GameManager.Levelmanager.HasGameEnded()) return;

        if (pClickedButton && !touchedReelUpHook) touchedReelUpHook = true; 
        DeployActive = true;
        ReelUpActive = false;
        SetActive(false, _handClick.gameObject);
        /*SetActive(true, _dropHook.gameObject);
        SetActive(false, _reelHook.gameObject);*/
        GameManager.Hook.SetState(hook.HookState.Reel);
    }
    public override void OnHookFloorTouch()
    {
        if (GameManager.Levelmanager.HasGameEnded()) return;

        //touchedReelUpHook = true;
        DeployActive = true;
        ReelUpActive = false;
        SetActive(false, _handClick.gameObject);
        /*SetActive(true, _dropHook.gameObject);
        SetActive(false, _reelHook.gameObject);*/
        GameManager.Hook.SetState(hook.HookState.Reel);
    }
    
    public override void OnEnterScene()
    {
        GameManager.inTutorial = true;
        if (!_onEnterScene)
        {
            Debug.Log("OnEnterScene was already called once for current instance");
            return;
        }

        //Trash Slider
        oceanCleanUpProgressBar.GetComponent<Slider>().value = 100;
        // Game Time
        GameManager.Gametimer.BeginCountdown();
        SetActive(true, _gameTimerText.gameObject);
        // Score
        SetActive(true, _totalScoreBoard.gameObject, _totalScoreText.gameObject);
        // Combo
        SetActive(false, ComboUI);
        boatStopped = true;
        _onEnterScene = false;
    }
    public override void OnLeaveScene()
    {
        if (!_onLeaveScene)
        {
            Debug.Log("OnLeaveScene was already called once for current instance");
            return;
        }
        // Controls
        DeployActive = false;
        //SetActive(false, _dropHook.gameObject);
        // Game Time
        GameManager.Gametimer.PauseCountdown();
        SetActive(false, _gameTimerText.gameObject);
        // Score
        SetActive(false, _totalScoreBoard.gameObject, _totalScoreText.gameObject, HookScoreText.gameObject);
        // Shopping List
        SetActive(false, _shoppingList.gameObject);
        // Combo
        SetActive(false, ComboUI);

        GameManager.inTutorial = false;
        _onLeaveScene = false;
    }
    public override void HookScoreToggle(bool pBool)
    {
        HookScoreText.gameObject.SetActive(pBool);
    }

    public override void HookSwipeAnimToggle(bool pBool)
    {
        arrows.gameObject.SetActive(pBool);
        handMove.gameObject.SetActive(pBool);
    }
    public override void HandClickToggle(bool pBool)
    {
        _handClick.gameObject.SetActive(pBool);
    }

    public void SetScreenPosition(GameObject pTheObject, GameObject pAccordingTo, Vector3 pOffset)
    {
        Vector3 accordingTo = Camera.main.WorldToScreenPoint(pAccordingTo.transform.position);
        Vector3 position = accordingTo + pOffset;
        pTheObject.transform.position = position;
    }
    public Vector3 GetScreenPosition(GameObject pAccordingTo, Vector3 pOffset)
    {
        Vector3 accordingTo = Camera.main.WorldToScreenPoint(pAccordingTo.transform.position);
        return accordingTo + pOffset;
    }

    public override bool GetTouchedReelUp()
    {
        return touchedReelUpHook;
    }

    public override bool GetSecondTimeFishing()
    {
       
        return _secondTimefishing;
    }
    public override bool GetTouchedDeployHook()
    {
        return touchedDeployHook;
    }
    /*public static void SetReelUpHook(bool reelup)
    {
        touchedReelUpHook = reelup;
    }*/
    public override void SetMovedBoat(bool moved)
    {
        movedBoat = moved;
    }
    public override void StopTrashGlow(bool pBool)
    {
        gotTrash = pBool;
    }
    public override void StopFishGlow(bool pBool)
    {
        gotFish = pBool;
    }
    public override void LeaveSceneTransition()
    {
        TransitionCurtain.GetComponent<Transition>().DownWards();
    }
    public override void EnterSceneTransition()
    {
        TransitionCurtain.GetComponent<Transition>().UpWards();
    }
    public override void MakeGlow(float rate, List<trash> trahses)
    {
        foreach (trash pTrash in trahses)
        {
            Renderer renderer = pTrash.gameObject.GetComponentInChildren<Renderer>();
            Material mat = renderer.material;

            float floor = 0.0f;
            float ceiling = 0.4f;
            float emission = floor + Mathf.PingPong(Time.time * rate, ceiling - floor);
            //float emission = Mathf.PingPong(Time.time, 1.0f);
            Color baseColor = Color.yellow; //Replace this with whatever you want for your base color at emission level '1'

            Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);

            mat.SetColor("_EmissionColor", finalColor);
        }
    }
    public override void MakeGlow(float rate, List<fish> fishes)
    {
        foreach (fish pFish in fishes)
        {
            Renderer renderer = pFish.gameObject.GetComponentInChildren<Renderer>();
            Material mat = renderer.material;

            float floor = 0.0f;
            float ceiling = 0.4f;
            float emission = floor + Mathf.PingPong(Time.time * rate, ceiling - floor);
            //float emission = Mathf.PingPong(Time.time, 1.0f);
            Color baseColor = Color.yellow; //Replace this with whatever you want for your base color at emission level '1'

            Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);

            mat.SetColor("_EmissionColor", finalColor);
        } 
    }
    public override void RestoreGlow(List<fish> fishes)
    {
        foreach (fish pFish in fishes)
        {
            Renderer renderer = pFish.gameObject.GetComponentInChildren<Renderer>();
            renderer.material.SetColor("_EmissionColor", Color.black);
        }
    }
    public override void RestoreGlow(List<trash> trashes)
    {
        foreach (trash ptrash in trashes)
        {
            Renderer renderer = ptrash.gameObject.GetComponentInChildren<Renderer>();
            renderer.material.SetColor("_EmissionColor", Color.black);
        }
    }
    private void AnimateSwipeHand(Vector3 position, float speed,float offset)
    {
        float newXpos;
        if(handMove.transform.position.x <= position.x - offset)
        {
            goLeft = false;
        }else if (handMove.transform.position.x >= position.x + offset)
        {
            goLeft = true;
        }

        if (goLeft)
        {
            newXpos = handMove.transform.position.x - handMove.gameObject.transform.position.x * Time.deltaTime * speed;
            
        }
        else
        {
            newXpos = handMove.transform.position.x + handMove.gameObject.transform.position.x * Time.deltaTime * speed;
           
        }
        handMove.transform.position = new Vector3(newXpos, position.y, position.z);
        
    }

    private IEnumerator FadeIn()
    {
        //Hide a part of the screen
        SetActive(true, _hide1.gameObject);
        _hide1.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
        _hide1.CrossFadeAlpha(1.0f, 0.5f, false);
        
        yield return new WaitForSeconds(1);

        //Show hand tapping the screen
        SetScreenPosition(_handClickNoDrops.gameObject, GameManager.Boat.gameObject, new Vector3(0, -100, 0));
        SetActive(true, _handClickNoDrops.gameObject);
    }

    private IEnumerator FadeOut()
    {
        _hide1.CrossFadeAlpha(0, 0.5f, false);
        
        yield return new WaitForSeconds(0.5f);

        //Deactivates former animation
        SetActive(false, _handClickNoDrops.gameObject, _hide1.gameObject);
        SetActive(false, _hide1.gameObject);
    }

    public override void IntroduceCombo()
    {
        StartCoroutine(MoveToPoint());
        
    }
    public IEnumerator MoveToPoint()
    {

        yield return new WaitForSeconds(2f);
        Vector3 finalPosition = ComboUI.transform.position;
        Vector3 initialPosition = new Vector3(ComboUI.transform.position.x, Screen.height / 2, 0);
        Vector3 initialSize = ComboUI.transform.localScale * 1.5f;
        Vector3 finalSize = ComboUI.transform.localScale;

        //GameManager.combo.CreateNewCombo();
        ComboUI.transform.position = initialPosition;
        ComboUI.transform.localScale = initialSize;
        SetActive(true, ComboUI);//Change for fade

        yield return new WaitForSeconds(0.5f);

        float waitTime = 0.01f;
        float speed = 0.1f;
        //small number to make it smooth, 0.04 makes it execute 25 times / sec

        while (true)
        {
            Vector3 position = ComboUI.transform.position;
            yield return new WaitForSeconds(waitTime);
            //use WaitForSecondsRealtime if you want it to be unaffected by timescale
            float newY = Time.time * speed;
            if(position.y < finalPosition.y) { ComboUI.transform.position = new Vector3(ComboUI.transform.position.x, ComboUI.transform.position.y + newY, ComboUI.transform.position.z); }
            else{
                Debug.Log("position");
            }
            if (ComboUI.transform.localScale.x >= finalSize.x) { ComboUI.transform.localScale *= 0.993f; } else { Debug.Log("scale"); }
            if (position.y >= finalPosition.y && ComboUI.transform.localScale.x <= finalSize.x)
            {
                ComboUI.transform.position = finalPosition;
                ComboUI.transform.localScale = finalSize;
                break;
            }
        }
    }
    }