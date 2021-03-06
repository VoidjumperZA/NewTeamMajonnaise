﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    [SerializeField] private GameObject _seaSurface;
    [Header("PostProcessingProfiles")]
    [SerializeField] private int _aboveProfile;
    [SerializeField] private int _underProfile;
    [Header("BoatMovementAreaBoundaries")]
    [SerializeField] private Transform _leftDetector;
    [SerializeField] private Transform _rightDetector;
    [Header("SceneTransitionBoatPoints")]
    [SerializeField] protected Transform _enterBoatPoint;
    [SerializeField] protected Transform _leaveBoatPoint;
    [Header("SceneTransitionCameraHolders")]
    [SerializeField] protected Transform _startCamHolder;
    [SerializeField] protected Transform _middleCamHolder;
    [SerializeField] protected Transform _endCamHolder;
    [Header("References")]

    public BaseUI UI;
    [SerializeField] protected FishSpawn _fishSpawner;

    [SerializeField] protected ShoppingList _shoppingList;
    [SerializeField] protected JellyFishSpawn _jellyFishSpawn;
    [SerializeField] protected Combo _combo;
    protected Light[] bouyLights;
    protected GameObject[] bouyObjects;
    [SerializeField] protected Color bouyColourPass;
    private bool gameEnded;
    [SerializeField]
    private List<GameObject> _objectsToDeactivateUnderWater;
    public virtual void Start()
    {
        bouyObjects = GameObject.FindGameObjectsWithTag("BouyLight");
        bouyLights = new Light[bouyObjects.Length];
        for (int i = 0; i < bouyObjects.Length; i++)
        {
            bouyLights[i] = bouyObjects[i].GetComponent<Light>();
        }
        Debug.Log("BouyLights has " + bouyLights.Length + " elements.");
        SetUpCamera();
        SetUpBoat();

        GameManager.Fishspawner = _fishSpawner;
        GameManager.ShopList = _shoppingList;
        GameManager.JellyFishSpawner = _jellyFishSpawn;
        GameManager.combo = _combo;

        GameManager.Levelmanager = this;
        if (UI) GameManager.Scorehandler.SetOriginalColours();

        UI.EnterSceneTransition();
        _shoppingList.GenerateShoppingList();
        _combo.CreateNewCombo();
        GameManager.Gametimer.ResetClockForNewArea();
        //Debug.Log("LevelManager - Start();");
    }
	public virtual void Update () {

    }
    protected void SetUpCamera()
    {
        GameManager.Camerahandler.ObjectsToDeactivateUnderWater = _objectsToDeactivateUnderWater;
        GameManager.Camerahandler.SeaSurface = _seaSurface.transform;
        GameManager.Camerahandler.aboveWater = _aboveProfile;
        GameManager.Camerahandler.underWater = _underProfile;
        GameManager.Camerahandler.ToggleBelowWater(false);
        GameManager.Camerahandler.StartMiddleEndCameraHolder(_startCamHolder, _middleCamHolder, _endCamHolder);
        GameManager.Camerahandler.SetViewPoint(CameraHandler.FocusPoint.Start, true);
        GameManager.Camerahandler.SetViewPoint(CameraHandler.FocusPoint.Middle, false);
    }
    public virtual void UIOnEnterScene()
    {
        if (UI) UI.OnEnterScene();
        else Debug.Log("LevelUI not assigned to LevelManager script");
    }
    public virtual void UIOnLeaveScene()
    {
        if (UI)
        {
            UI.OnLeaveScene();
            UI.LeaveSceneTransition();
        }
    }

    public void SetBouyToLightsToPass()
    {
        for (int i = 0; i < bouyLights.Length; i++)
        {
            bouyLights[i].GetComponent<Light>().color = bouyColourPass;
        }
    }
    protected void SetUpBoat()
    {
        SetEnterLeaveBoatStateDestinations();
        // Set Move state boundaries
        if (_leftDetector && _rightDetector) GameManager.Boat.SetMoveBoatStateBoundaries(new Vector3[] { _leftDetector.position, _rightDetector.position });
    }
    protected void SetEnterLeaveBoatStateDestinations()
    {
        // Set Enter and Leave state destination position
        GameManager.Boat.SetEnterStateDestination(_enterBoatPoint.position);
        GameManager.Boat.SetLeaveStateDestination(_leaveBoatPoint.position);
    }
    public Canvas Canvas()
    {
        if (UI.canvas) return UI.canvas;
        return null;
    }

    public void EndGame()
    {
        gameEnded = true;
    }

    public bool HasGameEnded()
    {
        return gameEnded;
    }
}
