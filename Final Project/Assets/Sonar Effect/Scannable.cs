﻿using UnityEngine;
using System.Collections;

public class Scannable : MonoBehaviour
{
    //public Animator UIAnim;
    private bool locked;
    private float scanTime;
    private float timeLeft;
    private float timeOutTime;
    void Start()
    {
        locked = true;
    }

    public void Ping()
    {
        Debug.Log("Ping");

        if (gameObject.transform.parent.gameObject.GetComponent<fish>()) gameObject.transform.parent.gameObject.GetComponent<general>().Visible = true;
        gameObject.GetComponent<cakeslice.Outline>().enabled = true;
        if (gameObject.GetComponent<SkinnedMeshRenderer>()) gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
        if (gameObject.GetComponent<MeshRenderer>()) gameObject.GetComponent<MeshRenderer>().enabled = true;
        timeOutTime = scanTime;
        timeLeft = scanTime;
        //StartCoroutine(TimeOutOutline());   
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            gameObject.GetComponent<cakeslice.Outline>().enabled = false;
            if (gameObject.GetComponent<SkinnedMeshRenderer>()) gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
            if (gameObject.GetComponent<MeshRenderer>()) gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    IEnumerator TimeOutOutline()
    {
        yield return new WaitForSeconds(scanTime);
        gameObject.GetComponent<cakeslice.Outline>().enabled = false;
        if (gameObject.GetComponent<SkinnedMeshRenderer>()) gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
        if (gameObject.GetComponent<MeshRenderer>()) gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    public void SetLockState(bool pState)
    {
        locked = pState;
    }

    public bool IsLocked()
    {
        return locked;
    }

    public void SetScanTime(float pTimeAsSeconds)
    {
        scanTime = pTimeAsSeconds;
    }
}
