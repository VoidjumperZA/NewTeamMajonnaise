﻿using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{
	private Camera m_Camera;
    void Start()
    {
        m_Camera = Camera.main;
    }

	void Update()
	{
        if (!m_Camera) return;
		transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
			m_Camera.transform.rotation * Vector3.up);
	}
}