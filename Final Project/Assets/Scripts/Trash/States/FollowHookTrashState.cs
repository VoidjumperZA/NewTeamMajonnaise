using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHookTrashState : AbstractTrashState
{
    public FollowHookTrashState(trash pTrash) : base(pTrash)
    {
    }
    public override void Start()
    {
        // _trash.ToggleOutliner(false);
        GameObject.Destroy(_trash.gameObject.GetComponent<BoxCollider>());
    }
    public override void Update()
    {
        _trash.gameObject.transform.position = GameManager.Hook.HookTip.position;
    }
    public override void Refresh()
    {

    }
    public override trash.TrashState StateType()
    {
        return trash.TrashState.FollowHook;
    }
    public override void OnTriggerEnter(Collider other)
    {

    }
}
