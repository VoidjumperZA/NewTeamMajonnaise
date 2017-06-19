using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour {
    private Animator _animator;
    private bool _active = false;
    private bool _downWards = true;
    private Vector3 _destination;
    private Vector3 _down = new Vector3(0, -1191, 0);
    private Vector3 _up = new Vector3(0, 1003, 0);

	void Start () {
        _animator = GetComponent<Animator>();
        gameObject.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
		/*if (_active)
        {
            gameObject.transform.position = Camera.main.WorldToScreenPoint(Vector3.Lerp(gameObject.transform.position, _destination, 0.1f));
            if ((_destination - gameObject.transform.position).magnitude < 0.1f)
            {
                gameObject.transform.position = _destination;
                _active = false;
            }
        }*/
	}
    public void Activate(bool pDownWards)
    {
        //_downWards = pDownWards;
        //_destination = _downWards ? _down : _up;

        //gameObject.transform.position = Camera.main.WorldToScreenPoint(_downWards ? _up : _down);
        StartCoroutine(SetAnimation(pDownWards));
    }
    private IEnumerator SetAnimation(bool pDownWards)
    {
        if (!gameObject.activeInHierarchy) gameObject.SetActive(true);
        yield return !pDownWards ? null: new WaitForSeconds(3);
        if (!_animator.enabled) _animator.enabled = true;
        _animator.SetBool("Down", pDownWards);
        //_animator.enabled = false;
    }
    public void UpWards()
    {
        StartCoroutine(SetAnimation(false));
    }
    public void DownWards()
    {
        StartCoroutine(SetAnimation(true));
    }

}
