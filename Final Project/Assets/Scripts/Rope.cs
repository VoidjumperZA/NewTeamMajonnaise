using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{    
    [Header("Links")]
    [SerializeField]
    private List<Transform> links;
    [SerializeField]
    private Transform beginningLink;
    [SerializeField]
    private Transform trailingLink;
    [SerializeField]
    private GameObject standardLink;
    [SerializeField]
    private LineRenderer lineRenderer;
    [Header("Settings")]
    [SerializeField]
    private bool destroyOnLoad;
    [SerializeField]
    private float distance;
    [SerializeField]
    private float minDistance;
    [SerializeField]
    private float maxDistance;
    [Header("On Start")]
    [SerializeField]
    private bool moveToPointOnStart;
    [SerializeField]
    private GameObject pointToMoveTo;
    [SerializeField]
    private bool parentToObjectOnStart;
    [SerializeField]
    private GameObject objectToParent;
    private GameObject originalParent;

    private GameObject activeCube;
    private int activeLink;
    delegate void CalculationType();
    private CalculationType calculationType;
    private Vector3 previousPoint;
    private int linkRenderLength;
    // Use this for initialization
    void Start()
    {       
        //Set up our list.
        links = new List<Transform>();
        links.Add(beginningLink);
        links.Add(trailingLink);

        //Make sure our rope can move between scenes if so desired.
        if (destroyOnLoad == false)
        {
            DontDestroyOnLoad(gameObject);
        }

        //Set up.
        originalParent = gameObject;
        activeLink = 0;
        if (lineRenderer == null)
        {
            Debug.Log("No line renderer detected. Continuing without drawing the rope.");
        }
        else
        {
            lineRenderer.positionCount = links.Count;
        }
        
        calculationType = calculateFor;

        //Setting up parents and positioning.
        if (moveToPointOnStart == true)
        {
            gameObject.transform.position = pointToMoveTo.transform.position;
        }
        if (parentToObjectOnStart == true)
        {
            links[activeLink].transform.parent = objectToParent.transform;
            links[activeLink].transform.position = objectToParent.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            RemoveLink();   
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AddLink();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SwitchActiveLink(0);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SwitchActiveLink(links.Count - 1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            calculationType = calculateFor;
            Debug.Log("Switching delegate to 'For'");
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            calculationType = calculateForeach;
            Debug.Log("Switching delegate to 'Foreach'");
        }
        previousPoint = links[activeLink].gameObject.transform.position;
        calculationType();
    }


    /// <summary>
    /// Adds a link to the rope at one behind the active link.
    /// </summary>
    public void AddLink()
    {
        GameObject newLink = Instantiate(standardLink, gameObject.transform);
        int insertPos;
        insertPos = activeLink == 0 ? (activeLink + 1) : (activeLink - 1);
        //If our active link is the trailing link while trying to add, we'll add at 0 instead of 1 while fine at other positions
        //correct for this by making sure if that particular case is true, set the insert pos back to 1
        if (insertPos == 0) {insertPos = 1;}
        links.Insert(insertPos, newLink.transform);
        lineRenderer.positionCount = links.Count;
        //make sure to update the active link
        SwitchActiveLink(activeLink == 0 ? activeLink : links.Count - 1);
    }

    /// <summary>
    /// Removes a link of the rope at one behind the active link.
    /// </summary>
    public void RemoveLink()
    {
        if (links.Count > 3)
        {
            int insertPos;
            insertPos = activeLink == 0 ? (activeLink + 1) : (activeLink - 1);
            links.RemoveAt(insertPos);
        }
        lineRenderer.positionCount = links.Count;
        //make sure to update the active link
        SwitchActiveLink(activeLink == 0 ? activeLink : links.Count - 1);
    }

    /// <summary>
    /// Switch the active link of the rope. It is advised to use either the leading or trailing links.
    /// </summary>
    /// <param name="pNewActiveLink"></param>
    private void SwitchActiveLink(int pNewActiveLink)
    {
        if (parentToObjectOnStart == true)
        {
            links[activeLink].transform.parent = originalParent.transform;
            links[activeLink].transform.position = originalParent.transform.position;
        }
        activeLink = pNewActiveLink;
        Debug.Log("Switching link to " + activeLink);
        if (parentToObjectOnStart == true)
        {
            links[activeLink].transform.parent = objectToParent.transform;
            links[activeLink].transform.position = objectToParent.transform.position;
        }
    }

    //Process our calculations using a for loop
    private void calculateFor()
    {
        if (activeLink == 0)
        {
            for (int i = 0; i < links.Count; i++)
            {
                calcuate(links[i]);
            }
            renderLineTrail();
        }
        else
        {
            for (int i = links.Count - 1; i > -1; i--)
            {
                calcuate(links[i]);
            }
            renderLineTrail();
        }
    }

    //Process our calculations using a foreach loop
    private void calculateForeach()
    {
        foreach (Transform link in links)
        {
            calcuate(link);
        }
        renderLineTrail();
    }

    private void renderLineTrail()
    {
        //Let the rope continue to work, even if the user has decided to run it without a line renderer.
        if (lineRenderer != null)
        {
            for (int i = 0; i < links.Count; i++)
            {
                lineRenderer.SetPosition(i, links[i].transform.position);
            }
        }    
    }

    /// <summary>
    /// Calculate the positions of each link, making sure they stay a distance away from each other.
    /// </summary>
    /// <param name="pLink"></param>
    private void calcuate(Transform pLink)
    {
        Vector3 direction = (previousPoint - pLink.position).normalized;
        pLink.position = previousPoint - direction * distance;
        previousPoint = pLink.position;
    }

    /// <summary>
    /// Returns the distance links keep from each other;
    /// </summary>
    public float GetLinkDistance()
    {
        return distance;
    }
}
