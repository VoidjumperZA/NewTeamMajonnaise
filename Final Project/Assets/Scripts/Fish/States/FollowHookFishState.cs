﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHookFishState : AbstractFishState
{
    public FollowHookFishState(fish pFish) : base(pFish)
    {
    }
    public override void Start()
    {
        foreach (GameObject gobj in _fish.DestroyOnCatch) GameObject.Destroy(gobj);
        //GameObject.Destroy(_fish.ScannableScript);
        //_fish.FishRenderer.enabled = true;
        //if (_fish.FishOutliner) _fish.FishOutliner.enabled = false;
        if (_fish.GetFishType() != fish.FishType.Special) GameManager.Hook.FishOnHook.Add(_fish);
        else if (!GameManager.Hook.SpecialFish)
        {
            GameManager.Hook.SpecialFish = _fish;
            GameManager.Hook.ToggleArrowActive(false);
            GameManager.Levelmanager.SetBouyToLightsToPass();
            Debug.Log("Assigned Special fish");
        }
        _fish.Animator.enabled = false;

        //_fish.Animator.SetBool("Death", true);*/
        // _fish.ToggleOutliner(false)
        GameObject.Destroy(_fish.gameObject.GetComponent<BoxCollider>());
        HandleJointsRigidBodies();


    }
    public override void Update()
    {
        if (_fish.Joints.Length > 0) _fish.Joints[0].gameObject.transform.position = GameManager.Hook.HookTip.position;
    }
    public override void Refresh()
    {

    }
    public override fish.FishState StateType()
    {
        return fish.FishState.FollowHook;
    }
    public override void OnTriggerEnter(Collider other)
    {

    }
    private void HandleJointsRigidBodies()
    {
        if (_fish.Joints.Length > 0)
        {
            foreach (GameObject joint in _fish.Joints)
            {
                joint.GetComponent<Rigidbody>().isKinematic = false;
            }
            _fish.Joints[0].transform.rotation = Quaternion.LookRotation(Vector3.right, -Vector3.forward);
        }
    }
}
