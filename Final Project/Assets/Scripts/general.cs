﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class general : MonoBehaviour
{
    [HideInInspector]
    public bool Visible = false;
    [HideInInspector]
    public bool Revealed = false;

    public virtual void Start()
    {
    }
    public virtual void Update()
    {

    }
    public virtual void FixedUpdate()
    {

    }
    public virtual void Reveal(float pFadeOutDuration, int pCollectableStaysVisibleRange)
    {

    }
    public virtual void Hide()
    {

    }
    public string GetTag()
    {
        return gameObject.tag;
    }
    public virtual void FinalizeInitialization()
    {

    }
}
