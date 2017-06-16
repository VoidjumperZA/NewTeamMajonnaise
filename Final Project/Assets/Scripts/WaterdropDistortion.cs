using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterdropDistortion : MonoBehaviour
{ 
    [SerializeField]
    private float scrollSpeed;
    [SerializeField]
    private float fadeSpeed;
    [SerializeField]
    private bool nonUniformDropMovement;
    private bool activated;
    private MeshRenderer renderer;
    float offset;

    // Use this for initialization
    void Start()
    {
        activated = false;
        renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.enabled = false;
       offset = 0.0f;
        // renderer.material.GetTexture("_MainTex").wrapMode = TextureWrapMode.Repeat;
        // renderer.material.GetTexture("_BumpMap").wrapMode = TextureWrapMode.Repeat;
    }

    // Update is called once per frame
    void Update()
    {
        if (activated == true)
        {
            
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                offset += scrollSpeed;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                offset -= scrollSpeed;
            }
            
            if (nonUniformDropMovement == true)
            {
                offset += Time.deltaTime * Random.Range(0.01f, scrollSpeed);//(0.0001f, 0.007f);
            }
            else
            {
                offset += (Time.deltaTime * scrollSpeed);
            }
           
            renderer.material.SetTextureOffset("_MainTex", new Vector2(0.0f, offset));
            renderer.material.SetTextureOffset("_BumpMap", new Vector2(0.0f, offset));

            Debug.Log("Offset = " + offset);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Activate();
        }
    }

    public void Activate()
    {
        activated = true;
        renderer.enabled = true;
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeOut()
    {        
        yield return new WaitForSeconds(fadeSpeed);
        renderer.enabled = false;
        activated = false;
    }
}
