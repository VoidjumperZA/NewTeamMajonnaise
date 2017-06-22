using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hook : general
{
    // States
    private Dictionary<HookState, AbstractHookState> _stateCache = new Dictionary<HookState, AbstractHookState>();
    private AbstractHookState _abstractState = null;
    public enum HookState { None, FollowBoat, Fish, Reel, SetFree }
    [SerializeField] private HookState _hookState = HookState.None;

    [HideInInspector] public List<fish> FishOnHook = new List<fish>();
    [HideInInspector] public List<trash> TrashOnHook = new List<trash>();

    // Class references
    private GameObject _manager;
    private InputTimer _inputTimer;
    [SerializeField]
    private Rope rope;
    [SerializeField]
    private GameObject scanCone;
    [SerializeField]
    private GameObject waterdropEffect;

    private GlobalUI _globalUI;
    private Camera _mainCamera;
    // Components
    [SerializeField] private Rigidbody _rigidBody;
    
    // Fishing
    private boat _boat;
    float fishRotationAngle = 25.0f;

    // Movement
    [SerializeField] private float _sideSpeed;
    [SerializeField] private float _downSpeed;
    [SerializeField] private float _fallSpeed;
    [SerializeField] private int _reelSpeedLinksPerFrame;
    [SerializeField] private float _xOffsetDamping;

    private Vector3 _xyOffset;
    private Vector3 _velocity;
    // X Velocity damping

    public ParticleSystem JellyAttackEffect2;
    public Transform HookTip;
	public ParticleSystem JellyAttackEffect;
    [SerializeField] private GameObject _sandSplash;
    private bool _splashInstantiated = false;

    private bool valid;
    public override void Start()
    {
        DontDestroyOnLoad(gameObject);
        valid = true;
        InitializeStateMachine();
        //basic.GlobalUI.GetOceanCleanUpBar().gameObject.GetComponent<OceanCleanUpUIAnimation>().SetBarPosition();
        //Debug.Log("Hook - Start();");
    }

    //
    public override void FixedUpdate()
    {
        _abstractState.Update();
        float distanceToBoat = Vector3.Distance(gameObject.transform.position, GameManager.Boat.gameObject.transform.position);
        float boatTrailingLinkDiff = Vector3.Distance(rope.GetLinks()[rope.GetNumberOfLinks() - 1].position, GameManager.Boat.gameObject.transform.position);
        //Debug.Log("Line length: " + rope.GetLineLength() + "(" + Mathf.FloorToInt(rope.GetLineLength()) + ")\t|\tNumber of Links: " + rope.GetNumberOfLinks());
        if (/*(Mathf.FloorToInt(rope.GetLineLength()) +*/ boatTrailingLinkDiff / rope.GetNumberOfLinks() > rope.GetLinkDistance() / 10.0f)
        {
            if (_abstractState is FishHookState)
            {
                rope.AddLink();
                rope.ResetLastLinkPosition();
                //Debug.Log("Adding links!");
            }
            else if (_abstractState is ReelHookState)
            {
                //rope.RemoveLink();
                //Debug.Log("Removing links!");
            }

        }
    }
    public void SetState(HookState pState)
    {
        if (_abstractState != null) _abstractState.Refresh();
        _abstractState = _stateCache[pState];
        _abstractState.Start();
    }
    private void InitializeStateMachine()
    {
        _stateCache.Clear();
        _stateCache[HookState.None] = new NoneHookState(this);
        _stateCache[HookState.FollowBoat] = new FollowBoatHookState(this, GameManager.Boat);
        _stateCache[HookState.Fish] = new FishHookState(this, _sideSpeed, _downSpeed, _fallSpeed);
        _stateCache[HookState.Reel] = new ReelHookState(this, GameManager.Boat, _reelSpeedLinksPerFrame);
        _stateCache[HookState.SetFree] = new SetFreeHookState(this);
        SetState(_hookState);
    }
    
 
    private void OnTriggerEnter(Collider other)
    {
        if (other && _abstractState != null) _abstractState.OnTriggerEnter(other);
        /*if (_hookState == HookState.Fish)
        { 

            //On contact with a fish
            if (other.gameObject.CompareTag("Fish"))
            {
                //ATTACH FISH TO HOOK
                //Rotate the fish by a small degree
                float fishAngle = Random.Range(-fishRotationAngle, fishRotationAngle);
                other.gameObject.transform.Rotate(fishAngle, 0.0f, 0.0f);
            }
        }*/
    }
    private void OnTriggerExit(Collider other)
    {
        if (other && _abstractState != null) _abstractState.OnTriggerExit(other);
    }
    public void AssignBoat(boat pBoat)
    {
        _boat = pBoat;
    }
    public GameObject GetCone()
    {
        return scanCone;
    }
    public GameObject GetWaterDropEffect()
    {
        return waterdropEffect;
    }
    public bool IsInState(HookState pState)
    {
        return _abstractState.StateType() == pState;
    }
	public void EnableJellyAttackEffect() 
	{
		StartCoroutine(JellyAttackCoroutine());
        StartCoroutine(JellyAttackCoroutine2());
    }

	private IEnumerator JellyAttackCoroutine()
	{
		JellyAttackEffect.gameObject.SetActive (true);
        

        yield return new WaitForSeconds (0.95f);
        
        JellyAttackEffect.gameObject.SetActive (false);
        

    }
    private IEnumerator JellyAttackCoroutine2()
    {
        JellyAttackEffect2.gameObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, (GameManager.Boat.transform.position - JellyAttackEffect2.gameObject.transform.position).normalized);
        JellyAttackEffect2.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.95f);

        
        JellyAttackEffect2.gameObject.SetActive(false);

    }
    public void CreateSandSplash()
    {
        if (!_splashInstantiated)
        {
            _splashInstantiated = true;
            _sandSplash = Instantiate(_sandSplash, HookTip.position, Quaternion.identity);
        }
        StartCoroutine(SandSplash());
    }
    private IEnumerator SandSplash()
    {
        _sandSplash.SetActive(true);
        _sandSplash.transform.position = HookTip.position;
        yield return new WaitForSeconds(2);
        _sandSplash.SetActive(false);
    }


}
