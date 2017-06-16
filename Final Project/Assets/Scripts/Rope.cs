using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField]
    private Transform[] links;
    [SerializeField]
    private float distance;
    [SerializeField]
    private float minDistance;
    [SerializeField]
    private float maxDistance;
    private LineRenderer lineRenderer;
    private GameObject activeCube;
    private int activeLink;
    delegate void CalculationType();
    private CalculationType calculationType;
    private Vector3 previousPoint;
    private int linkRenderLength;
    // Use this for initialization
    void Start()
    {
        activeLink = 0;
        linkRenderLength = links.Length;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = links.Length;
        calculationType = calculateFor;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            distance -= 0.1f;
            if (distance < minDistance)
            {
                distance = minDistance;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            distance += 0.1f;
            if (distance > maxDistance)
            {
                distance = maxDistance;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            activeLink = 0;
            Debug.Log("Switching link to " + activeLink);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            activeLink = links.Length - 1;
            Debug.Log("Switching link to " + activeLink);
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

    private void calculateFor()
    {
        if (activeLink == 0)
        {
            for (int i = 0; i < links.Length; i++)
            {
                calcuate(links[i]);
            }
            renderLineTrail();
        }
        else
        {
            for (int i = links.Length - 1; i > -1; i--)
            {
                calcuate(links[i]);
            }
            renderLineTrail();
        }
    }

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
        float time = Time.time;
        for (int i = 0; i < linkRenderLength; i++)
        {
            lineRenderer.SetPosition(i, links[i].transform.position);
        }
    }

    private void calcuate(Transform pLink)
    {
        Vector3 direction = (previousPoint - pLink.position).normalized;
        pLink.position = previousPoint - direction * distance;
        previousPoint = pLink.position;
    }
}
