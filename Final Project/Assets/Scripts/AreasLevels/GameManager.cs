using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance = null;
    
    public static boat Boat;
    public static hook Hook;
   // public static radar Radar;
    public static trailer Trailer;
    // Same per Scene
    private static LevelLoader _levelloader;
    public static CameraHandler Camerahandler;
    public static GameTimer Gametimer;
    public static ScoreHandler Scorehandler;
    // Different per Scene
    //public static TutorialManager Tutorialmanager { get; set; }
    public static LevelManager Levelmanager { get; set; }
    public static FishSpawn Fishspawner { get; set; }
    public static ShoppingList ShopList { get; set; }
    public static JellyFishSpawn JellyFishSpawner { get; set; }
    public static Combo combo { get; set; }

    public static int NextScene = 2;
    public static bool GotSpecialFish = false;

    private void Start () {
        DontDestroyOnLoad(gameObject);
        AssignReferences();
        //Debug.Log("GameManager - Start();");
	}
	private void Update ()
    {
        
	}
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.M)) GoToNextScene();
        Camerahandler.ClassUpdate();
    }
    private void AssignReferences()
    {
        _levelloader = GetComponent<LevelLoader>(); if (!_levelloader) Debug.Log("GameManager - LevelLoader script = NULL");
        Camerahandler = GetComponent<CameraHandler>(); if (!Camerahandler) Debug.Log("GameManager - CameraHandler script = NULL");
        Gametimer = GetComponent<GameTimer>(); if (!Gametimer) Debug.Log("GameManager - GameTimer = NULL");
        Scorehandler = GetComponent<ScoreHandler>(); if (!Scorehandler) Debug.Log("GameManager - ScoreHandler = NULL");
    }
    public static void GoToNextScene()
    {
        GotSpecialFish = false;
        // Taking care of the previous scene 
        if (!(Levelmanager is MenuManager)) UIOnLeaveScene();
        // For new scene requirements
        LoadSceneAsync(NextScene, 4);
        if (NextScene < 4) NextScene += 1;
        Camerahandler.SetViewPoint(CameraHandler.FocusPoint.End);
        Camerahandler.Play();
        Boat.SetState(boat.BoatState.LeaveScene);
    }
    public void InitializeReferenceInstances()
    {
        Camerahandler.InitializeCameraHandler();
        
        Instance = this;
    }
    public static void LoadScene(int pIndex)
    {
        _levelloader.LoadScene(pIndex);
    }
    public static void LoadSceneAsync(int pIndex, float pWaitTime)
    {
        _levelloader.LoadSceneAsync(pIndex, pWaitTime);
    }
    public static void UIOnEnterScene()
    {
        //if(Levelmanager is TutorialManager) { }
        if (Levelmanager) Levelmanager.UIOnEnterScene();
    }
    public static void UIOnLeaveScene()
    {
        if (Levelmanager) Levelmanager.UIOnLeaveScene();
    }
}
