using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMainCamToCanvas : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
        gameObject.GetComponent<Canvas>().planeDistance = 13;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
