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
    

    //private Animator _dropHookAnim;
    //private Animator _reelHookAnim;
   // [SerializeField]
   // private Sprite _bubbleImage;
    
    
    //Conditions for showing UI in order
    private bool boatStopped = false;
    public static bool touchedScreen = false;
    private bool touchedDeployHook = false;
    private bool _firstTimeFishing;
    private bool _secondTimefishing;
    private static bool touchedReelUpHook;
    private static bool movedBoat;
    private bool gotTrash;
    private bool gotFish;

    //Activate/Deactivate reel up and deploy hook
    public static bool ReelUpActive;
    public static bool DeployActive;

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

        _firstTimeFishing = false;
        _secondTimefishing = false;
        touchedReelUpHook = false;
        movedBoat = false;
        gotTrash = false;
        gotFish = false;
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
        SetActive(false, handMove.gameObject,_handClick.gameObject, arrows.gameObject,_handClickNoDrops.gameObject);
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
                GameObject[] trash = GameObject.FindGameObjectsWithTag("Trash");
                MakeGlow(_rateOfGlow, trash);
            }
            else
            {
                RestoreGlow(GameObject.FindGameObjectsWithTag("Trash"));
            }
            if (!gotFish)
            {

                GameObject[] fish = GameObject.FindGameObjectsWithTag("Fish");
                MakeGlow(_rateOfGlow, fish);
            }
            else
            {
                RestoreGlow(GameObject.FindGameObjectsWithTag("Fish"));
            }

            //Introducing features step by step
            //Step 1 - Radar introduced
            if (!touchedScreen)
            {
                //Check if the player have touched the screen
                if (Input.GetMouseButton(0) || mouse.Touching())
                {
                    touchedScreen = true;
                }
                else
                {
                    //Show hand tapping the screen
                    SetScreenPosition(_handClickNoDrops.gameObject, GameManager.Boat.gameObject, new Vector3(0, -60, 0));
                    SetActive(true, _handClickNoDrops.gameObject);
                }
            }//Step 2 - Deploy hook button introduced
            else if (!touchedDeployHook)

            {    //Deactivates former animation
                SetActive(false, _handClickNoDrops.gameObject);

                //Activates button and hand animation
                DeployActive = true;
                _deployHookImage.color = transparent;
                SetActive(true, _bubbleMoving.gameObject);
                _handClick.transform.position = _dropHook.transform.position + new Vector3(10, -10, 0);
                SetActive(true, _handClick.gameObject);

                //Positions hand for swiping animation
                SetScreenPosition(handMove.gameObject, GameManager.Hook.gameObject, new Vector3(0, 0, 0));

            }//Step 3 - Fishing for the first time, introducing trash 
            else if (!_firstTimeFishing)
            {
                Debug.Log("first time fishing-tutorial ui");
                //Deactivate animation from last step
                SetActive(false, _handClick.gameObject);

                //Show swiping animation to introduce how to move the hook
                SetScreenPosition(arrows.gameObject, GameManager.Hook.gameObject, new Vector3(10, -20, 0));
                AnimateSwipeHand(GetScreenPosition(GameManager.Hook.gameObject, new Vector3(0, -20, 0)), 0.2f, 50.0f);

                //Make animation disappear when the player touches the screen
                if (Input.GetMouseButton(0) || mouse.Touching())
                {
                    SetActive(false, arrows.gameObject, handMove.gameObject);

                }
            }//Step 4 - Introducing combos
            else if (!_secondTimefishing)
            {
                Debug.Log("second time fishing-tutorial ui");
                SetActive(true, ComboUI);
                //Show combos for the first time
            }
            else if (!movedBoat)
            {
                //_reelHookAnim.enabled = false;
                SetActive(false, _bubbleMoving.gameObject);
                _reelHookImage.color = opaque;

                //_reelHook.GetComponent<Image>().sprite = _bubbleImage;
                SetScreenPosition(arrows.gameObject, GameManager.Boat.gameObject, new Vector3(0, 0, 0));
                AnimateSwipeHand(GetScreenPosition(GameManager.Boat.gameObject, new Vector3(0, -20, 0)), 0.2f, 50.0f);
                SetActive(true, arrows.gameObject,handMove.gameObject);
                
            }
            else
            {
               SetActive(false, arrows.gameObject,handMove.gameObject);
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
        }else if (!_firstTimeFishing)
        {
            Debug.Log("First time fishing true");
            _firstTimeFishing = true;
        }else if (!_secondTimefishing)
        {
            Debug.Log("Second time fishing true");
            _secondTimefishing = true;
        }
        DeployActive = false;
        //SetActive(false, _dropHook.gameObject);
        
        if(_firstTimeFishing &&  _secondTimefishing)
        {
            if (!touchedReelUpHook)
            {
                //_reelHookAnim.enabled = true;
                SetActive(true, _bubbleMoving.gameObject);
                _reelHookImage.color = transparent;
                SetActive(true,_handClick.gameObject);
            }

            ReelUpActive = true;
        }
        GameManager.Boat.SetState(boat.BoatState.Fish);
    }
    public override void OnReelHook()
    {
        if (GameManager.Levelmanager.HasGameEnded()) return;

        touchedReelUpHook = true; 
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

    public static bool GetTouchedReelUp()
    {
        return touchedReelUpHook;
    }

    public override bool GetFirstTimeFishing()
    {
        return _firstTimeFishing;
    }
    public static void SetReelUpHook(bool reelup)
    {
        touchedReelUpHook = reelup;
    }
    public static void SetMovedBoat(bool moved)
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

    public override void MakeGlow(float rate, GameObject[] gameObjects)
    {
        foreach( GameObject obj in gameObjects)
        {
            Renderer renderer = obj.GetComponentInChildren<Renderer>();
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
    public override void RestoreGlow(GameObject[] gameObjects)
    {
        foreach (GameObject obj in gameObjects)
        {
            Renderer renderer = obj.GetComponentInChildren<Renderer>();
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



}