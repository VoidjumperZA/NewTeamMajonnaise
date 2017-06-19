using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelHookState : AbstractHookState {
    private boat _boat;
    private Rope rope;
    private int _reelSpeed;

	public ReelHookState(hook pHook, boat pBoat, int pReelSpeed) : base(pHook)
    {
        _boat = pBoat;
        _reelSpeed = pReelSpeed;
    }
    public override void Start()
    {
        rope = GameObject.Find("Rope").GetComponent<Rope>();
        rope.SwitchActiveLink(rope.GetLinks().Count - 1, true, rope.GetLinks()[0].transform, rope.GetObjectToFollow().transform);
       // _boat.StartCoroutine(counter());
    }

    public override void Update()
    {
        //float step = _reelSpeed;
        //Vector3 differenceVector = (_boat.gameObject.transform.position - _hook.gameObject.transform.position);
        //if (differenceVector.magnitude >= step) _hook.gameObject.transform.Translate(differenceVector.normalized * step);
        //float boatTrailingLinkDiff = Vector3.Distance(rope.GetLinks()[rope.GetNumberOfLinks() - 1].position, GameManager.Boat.gameObject.transform.position);
        //Debug.Log("Line length: " + rope.GetLineLength() + "(" + Mathf.FloorToInt(rope.GetLineLength()) + ")\t|\tNumber of Links: " + rope.GetNumberOfLinks());
        for (int i = 0; i < _reelSpeed; i++)
        {
            rope.RemoveLink();
        }
        

        if (rope.GetLinks().Count == 3)
        {
            rope.SwitchActiveLink(0, true, rope.GetObjectToFollow().transform, rope.GetLinks()[0].transform);
            _hook.gameObject.transform.position = _boat.gameObject.transform.position;
            SetState(hook.HookState.SetFree);
        }
    }
    public override void Refresh()
    {
        rope = GameObject.Find("Rope").GetComponent<Rope>();
        
    }
    public override hook.HookState StateType()
    {
        return hook.HookState.Reel;
    }
    private IEnumerator counter()
    {
        yield return new WaitForSeconds(_reelSpeed);
        
        Debug.Log("Removing links!");
    }
}
