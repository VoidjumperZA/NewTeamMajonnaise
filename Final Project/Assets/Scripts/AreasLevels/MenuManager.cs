using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : LevelManager
{
    [SerializeField] protected GameManager _gameManager;
    [Header("Boat")]
    [SerializeField] private boat _boat;
    [Header("Rope")]
    [SerializeField] private Rope _rope;
    [Header("Hook")]
    [SerializeField] private hook _hook;
    /*[Header("SceneTransitionBoatPoints")]
    [SerializeField] protected Transform _enterBoatPoint;
    [SerializeField] protected Transform _leaveBoatPoint;
    [Header("SceneTransitionCameraHolders")]
    [SerializeField] protected Transform _startCamHolder;
    [SerializeField] protected Transform _middleCamHolder;
    [SerializeField] protected Transform _endCamHolder;*/

    public override void Start () {
        GameManager.Boat = ReturnBoat();
        GameManager.rope = ReturnRope();
        GameManager.Hook = ReturnHook();

        _gameManager.InitializeReferenceInstances();
        GameManager.Boat.FinalizeInitialization();
        SetEnterLeaveBoatStateDestinations();
        SetUpCamera();

        GameManager.Levelmanager = this;
        //Debug.Log("MenuManager - Start();");
    }
	public override void Update () {
		
	}
    private boat ReturnBoat()
    {
        return _boat;
    }
    private Rope ReturnRope()
    {
        return _rope;
    }
    private hook ReturnHook()
    {
        return _hook;
    }
}
