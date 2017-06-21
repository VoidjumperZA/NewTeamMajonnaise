using UnityEngine;
using System.Collections;

public class Scannable : MonoBehaviour
{
    //public Animator UIAnim;
    private bool locked;
    private float scanTime;
    private float timeLeft;
    private float timeOutTime;
    private fish fish;
    private trash trash;
    private bool permaShow;
    void Start()
    {
        locked = true;
        permaShow = false;
        fish = null;
        trash = null;
        if (gameObject.transform.parent.GetComponent<fish>() != null)
        {
            Debug.Log("Got a fish script.");
            fish = gameObject.transform.parent.GetComponent<fish>();
        }
        if (gameObject.transform.parent.GetComponent<trash>() != null)
        {
            Debug.Log("Got a trash script.");
            trash = gameObject.transform.parent.GetComponent<trash>();
        }

    }

    public void Ping()
    {
        //Debug.Log("Ping");

        if (gameObject.transform.parent.gameObject.GetComponent<fish>()) gameObject.transform.parent.gameObject.GetComponent<general>().Visible = true;
        if (gameObject.transform.parent.gameObject.GetComponent<trash>()) gameObject.transform.parent.gameObject.GetComponent<general>().Visible = true;
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
        
        if (fish != null)
        {
            permaShow = fish.GetFishState() != fish.FishState.Swim ? true : false;
        }
        if (trash != null)
        {
            permaShow = trash.GetTrashState() != trash.TrashState.Float ? true : false;
        }
        if (timeLeft <= 0 && permaShow == false)
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
