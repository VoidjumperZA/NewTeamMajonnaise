using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLookAtObect : MonoBehaviour {
    private GameObject target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            gameObject.transform.rotation = Quaternion.LookRotation((target.transform.position - gameObject.transform.position).normalized, Vector3.forward);
            //gameObject.transform.LookAt(target.transform);
        }
        else
        {
            target = GameManager.Hook.GetArrowTarget();
        }
	}
    
}
