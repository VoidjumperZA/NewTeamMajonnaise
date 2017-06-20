using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : LevelManager
{

    public override void Start()
    {
        SetUpCamera();
        SetUpBoat();

        GameManager.Fishspawner = _fishSpawner;
        GameManager.ShopList = _shoppingList;
        GameManager.JellyFishSpawner = _jellyFishSpawn;
        GameManager.combo = _combo;
        GameManager.Levelmanager = this;
        GameManager.Scorehandler.SetOriginalColours();

        UI.EnterSceneTransition();
        _shoppingList.GenerateShoppingList();
        _combo.CreateNewCombo();
        //Debug.Log("LevelManager - Start();");
    }
    public override void Update()
    {

    }
    /*protected void SetUpCamera()
    {
        GameManager.Camerahandler.StartMiddleEndCameraHolder(_startCamHolder, _middleCamHolder, _endCamHolder);
        GameManager.Camerahandler.SetViewPoint(CameraHandler.FocusPoint.Start, true);
        GameManager.Camerahandler.SetViewPoint(CameraHandler.FocusPoint.Middle, false);
    }*/
    /*protected void SetEnterLeaveBoatStateDestinations()
    {
        // Set Enter and Leave state destination position
        GameManager.Boat.SetEnterStateDestination(_enterBoatPoint.position);
        GameManager.Boat.SetLeaveStateDestination(_leaveBoatPoint.position);
    }*/
    public override void UIOnEnterScene()
    {
        if (UI) UI.OnEnterScene();
        else Debug.Log("LevelUI not assigned to LevelManager script");
    }
    public override void UIOnLeaveScene()
    {
        base.UIOnLeaveScene();
    }
}
