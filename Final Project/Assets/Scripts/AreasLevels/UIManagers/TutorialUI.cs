using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : BaseUI
{

    [Header("Animations")]
    [SerializeField]
    private Image _handClick;
    [SerializeField]
    private Image _arrows;
    [SerializeField]
    private Image _handMove;
    [SerializeField]
    private Image _handClickNoDrops;
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
    private static bool firstTimeReelUp;
    private static bool touchedReelUpHook;
    private static bool movedBoat;

    //Activate/Deactivate reel up and deploy hook
    public static bool ReelUpActive;
    public static bool DeployActive;

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

        firstTimeReelUp = true;
        touchedReelUpHook = false;
        movedBoat = false;
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
        SetActive(false, _handMove.gameObject,_handClick.gameObject, _arrows.gameObject,_handClickNoDrops.gameObject);
        SetActive(false, _bubbleMoving.gameObject);
       
        //Debug.Log("TutorialUI - Start();");
    }
    public override void Update()
    {
        SetActive(DeployActive, _dropHook.gameObject);
        SetActive(ReelUpActive, _reelHook.gameObject);

        if (boatStopped) { 
        // Game Time
        _gameTimerText.text = GameManager.Gametimer.GetFormattedTimeLeftAsString();
        // Score
        _totalScoreText.text = GameManager.Scorehandler.BankedScore + "";

        HookScoreText.text = GameManager.Scorehandler.HookScore + "";
        HookScoreText.transform.position = GameManager.Scorehandler.HookScorePosition();

        //Steps
        if (!touchedScreen)
        { if(Input.GetMouseButton(0) || mouse.Touching())
          {
                    touchedScreen = true;
                    SetActive(false, _handClickNoDrops.gameObject);
          }
          else
          {
                //Show hand for activating the radar
                SetScreenPosition(_handClickNoDrops.gameObject, GameManager.Boat.gameObject, new Vector3(0, -50, 0));
                SetActive(true, _handClickNoDrops.gameObject);
           }
        }
        else if (!touchedDeployHook)
        {
             Debug.Log("Step 2");
             SetActive(false, _handClickNoDrops.gameObject);
             DeployActive = true;//SetActive(true, _dropHook.gameObject);
             _deployHookImage.color = transparent;
             SetActive(true, _bubbleMoving.gameObject);
            _handClick.transform.position = _dropHook.transform.position + new Vector3(10,-10,0);
             SetActive(true, _handClick.gameObject);
        }
        else if (firstTimeReelUp)
        {
            SetScreenPosition(_arrows.gameObject, GameManager.Hook.gameObject, new Vector3(10, -20, 0));
            //SetActive(true, _arrows.gameObject);
            //SetScreenPosition(_handMove.gameObject, GameManager.Hook.gameObject, new Vector3(0, 0, 0));
            //SetActive(true, _handMove.gameObject);
            //AnimateSwipeHand(GetScreenPosition(GameManager.Hook.gameObject, new Vector3(0,0,0)).x);

            Debug.Log("Step 3");
            SetActive(false, _handClick.gameObject);

            if (Input.GetMouseButton(0) || mouse.Touching())
            {
                SetActive(false, _arrows.gameObject);
            }
        }
        else if (touchedReelUpHook && !movedBoat )
        {
            Debug.Log("Step 4");
                //_reelHookAnim.enabled = false;
                SetActive(false, _bubbleMoving.gameObject);
                _reelHookImage.color = opaque;
                
                //_reelHook.GetComponent<Image>().sprite = _bubbleImage;
                SetScreenPosition(_arrows.gameObject, GameManager.Boat.gameObject, new Vector3(0, 0, 0));
            SetActive(true, _arrows.gameObject);
        }
        else
        {
                SetActive(false, _arrows.gameObject);
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
        }
        DeployActive = false;
        //SetActive(false, _dropHook.gameObject);
        
        if(!firstTimeReelUp)
        {
            if (!touchedReelUpHook)
            {
                //_reelHookAnim.enabled = true;
                SetActive(true, _bubbleMoving.gameObject);
                _reelHookImage.color = transparent;
                SetActive(true,_handClick.gameObject);
            }
           
            ReelUpActive = true;/*SetActive(true, _reelHook.gameObject);*/
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

        _onLeaveScene = false;
    }
    public override void HookScoreToggle(bool pBool)
    {
        HookScoreText.gameObject.SetActive(pBool);
    }

    public override void HookSwipeAnimToggle(bool pBool)
    {
        _arrows.gameObject.SetActive(pBool);
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

    public static bool GetFirstTimeReelUp()
    {
        return firstTimeReelUp;
    }
    public static void SetFirstTimeReelUp(bool firstTime)
    {
        firstTimeReelUp = firstTime;
    }
    public static void SetReelUpHook(bool reelup)
    {
        touchedReelUpHook = reelup;
    }
    public static void SetMovedBoat(bool moved)
    {
        movedBoat = moved;
    }
    public override void LeaveSceneTransition()
    {
        TransitionCurtain.GetComponent<Transition>().DownWards();
    }
    public override void EnterSceneTransition()
    {
        TransitionCurtain.GetComponent<Transition>().UpWards();
    }
    /*private void AnimateSwipeHand(float position)
    {
        //Creating vector of positions
        List<float> positions = new List<float>(2);
        float offset = 5;
        float speed = 3;
        positions.Add(position - offset);
        positions.Add(position + offset);

        if(_handMove.transform.position.x == positions[index])
        {
            if(index == 0) { index = 1; }
            else { index = 0; }
        }

        if(index == 0)
        {
            //_handMove.position.x -= _handMove.gameObject.transform.position.x * Time.deltaTime * speed;
        }
        else
        {
            //_handMove.gameObject.transform.position.x += _handMove.gameObject.transform.position.x * Time.deltaTime * speed;
        }
        


    }*/

}