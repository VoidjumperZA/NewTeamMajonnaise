using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelHookState : AbstractHookState {
    private boat _boat;
    private Rope _rope;
    private int _reelSpeed;

	public ReelHookState(hook pHook, boat pBoat, Rope pRope, int pReelSpeed) : base(pHook)
    {
        _boat = pBoat;
        _reelSpeed = pReelSpeed;
        _rope = pRope;
    }
    public override void Start()
    {
        //rope = GameObject.Find("Rope").GetComponent<Rope>();
        _rope.SwitchActiveLink(_rope.GetLinks().Count - 1, true, _rope.GetLinks()[0].transform, _rope.GetObjectToFollow().transform);
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
            _rope.RemoveLink();
        }
        
        if (_rope.GetLinks().Count <= 10)
        {
            GameManager.Hook.GetWaterDropEffect().SetActive(true);
            GameManager.Hook.GetWaterDropEffect().GetComponent<WaterdropDistortion>().Start();
            GameManager.Hook.GetWaterDropEffect().GetComponent<WaterdropDistortion>().Activate();
        }

        if (_rope.GetLinks().Count == 3)
        {
            _rope.SwitchActiveLink(0, true, _rope.GetObjectToFollow().transform, _rope.GetLinks()[0].transform);
            _hook.gameObject.transform.position = _boat.gameObject.transform.position;
            SetState(hook.HookState.SetFree);
        }
    }
    public override void Refresh()
    {
        _rope = GameObject.Find("Rope").GetComponent<Rope>();
        
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
