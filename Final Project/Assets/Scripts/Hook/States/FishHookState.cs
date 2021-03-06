﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHookState : AbstractHookState
{
    private Vector3 previousPosition;
    
    private float _fallSpeed;
    private Vector2 _prevInputVelocity;
    private Vector3 _totalVelocity;
    private Vector3 _inputDirection;
    private float _sideSpeed;
    private float _downSpeed;

    private int _touchingCliff = 1;

    public FishHookState(hook pHook, float pSideSpeed, float pDownSpeed, float pFallSpeed) : base(pHook)
    {
        _sideSpeed = pSideSpeed;
        _downSpeed = pDownSpeed;
        _fallSpeed = pFallSpeed;
    }
    public override void Start()
    {
        GameManager.Camerahandler.SetViewPoint(CameraHandler.FocusPoint.Hook);
        GameManager.Hook.GetCone().SetActive(false);
    }
    public override void Update()
    {
        if ((_hook.transform.position - GameManager.Boat.transform.position).magnitude < 10)
        {
            _hook.transform.Translate(-Vector3.up * _downSpeed);
        } 
        else
        {
            ApplyVelocity(-_fallSpeed, mouse.Touching());
        }
        Borders();
    }

    //
    public override void Refresh()
    {
        _inputDirection = Vector2.zero;
        _totalVelocity = Vector3.zero;
    }

    //
    public override hook.HookState StateType()
    {
        return hook.HookState.Fish;
    }
    //
    private void SetInputDirection(Vector3 pPosition)
    {
        _inputDirection = new Vector3(pPosition.x - _hook.HookTip.position.x, pPosition.y - _hook.HookTip.position.y, 0);
        _inputDirection.Normalize();
    }

    //
    private void ApplyVelocity(float pFallSpeed, bool pSteering)
    {
        if (pSteering) SetInputDirection(mouse.GetWorldPoint());
        Vector3 acceleration = new Vector3(_inputDirection.x * _sideSpeed, _inputDirection.y * _downSpeed, 0);

        _totalVelocity.x = (pSteering) ? acceleration.x * _touchingCliff : _totalVelocity.x * 0.9f;
        _totalVelocity.y = (pFallSpeed > acceleration.y) ? acceleration.y : pFallSpeed;
        _totalVelocity.z = acceleration.z;

        previousPosition = _hook.gameObject.transform.position;
        _hook.gameObject.transform.Translate(_totalVelocity);
        
        _inputDirection = Vector3.zero;
    }

    //
    public override void OnTriggerEnter(Collider other)
    {
        if (!_hook || !other) return;
        //Reel the hook in if you touch the floor
        if (other.gameObject.CompareTag("Floor"))
        {
            
            _hook.CreateSandSplash();
            GameManager.Levelmanager.UI.OnReelHook();
            //basic.combo.ClearPreviousCombo(false);
            //GameObject.Instantiate (basic.HookHit, _hook.HookTip.position, Quaternion.identity);

            /* if (!GameManager.Levelmanager.UI.GetFirstTimeFishing() && !TutorialUI.GetTouchedReelUp())
                {
                    TutorialUI.SetReelUpHook(true);

                }*/


        }
        //On contact with a fish
        if (other.gameObject.CompareTag("Fish"))
        {
            fish theFish = other.gameObject.GetComponent<fish>();
            if (!theFish || !theFish.Visible) return;
            theFish.SetState(fish.FishState.FollowHook);
            GameManager.ShopList.CollectFish((int)theFish.GetFishType());

            if(!GameManager.inTutorial || GameManager.Levelmanager.UI.GetSecondTimeFishing())
            {
                GameManager.combo.Collect((int)theFish.GetFishType());
            }
            

            GameManager.Scorehandler.AddScore(theFish.GetFishType(), true, true);

            StopFishGlow(true);
            /*if (!basic.GlobalUI.InTutorial)
            {
                basic.combo.CheckComboProgress(theFish.fishType);
            }
            if (!basic.Shoppinglist.Introduced)
            {
                basic.Shoppinglist.Show(true);
                basic.Shoppinglist.Introduced = true;
   
            //basic.Camerahandler.CreateShakePoint();
        }*/
        }
        if (other.gameObject.CompareTag("Jellyfish"))
        {
            Jellyfish theJellyfish = other.gameObject.GetComponent<Jellyfish>();
            if (!theJellyfish) return;
			_hook.EnableJellyAttackEffect ();
            GameManager.Scorehandler.RemoveScore(true);

            // basic.Camerahandler.CreateShakePoint();

            GameManager.Levelmanager.UI.OnReelHook();
            //basic.combo.ClearPreviousCombo(false);
            //Create a new list maybe
            //Change animation for the fish and state
            //Remove fish from list 
            //Destroy fish

        }
        if (other.gameObject.CompareTag("Trash"))
        {
            trash theTrash = other.gameObject.GetComponent<trash>();
            if (!theTrash || !theTrash.Visible) return;

            theTrash.SetState(trash.TrashState.FollowHook);
            _hook.TrashOnHook.Add(theTrash);

            bool firstTime = GameManager.Scorehandler.CollectATrashPiece();
            
            GameManager.Levelmanager.UI.UpdateOceanProgressBar(firstTime);
            GameManager.Camerahandler.CreateShakePoint();

            StopTrashGlow(true);
            GameManager.Levelmanager.UI.OnReelHook();
            
            //basic.combo.ClearPreviousCombo(false);

        }
        if (other.gameObject.CompareTag("Cliff"))
        {
            _touchingCliff = 0;
            _hook.transform.Translate(new Vector3(0.5f, 0, 0));
        }
    }
    public override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Cliff"))
        {
            _touchingCliff = 1;
        }
    }
    
    public void StopFishGlow(bool pBool)
    {
        if (GameManager.Levelmanager.UI) GameManager.Levelmanager.UI.StopFishGlow(pBool);
    }
    public void StopTrashGlow(bool pBool)
    {
        if (GameManager.Levelmanager.UI) GameManager.Levelmanager.UI.StopTrashGlow(pBool);
    }
    private void Borders()
    {
        if (GameManager.Fishspawner)
        {
            if (_hook.gameObject.transform.position.x < GameManager.Fishspawner.LeftBorderX || _hook.gameObject.transform.position.x > GameManager.Fishspawner.RightBorderX)
            {
                _hook.gameObject.transform.position = new Vector3(previousPosition.x, _hook.gameObject.transform.position.y, _hook.gameObject.transform.position.z);
            }
        }
    }
  
}
