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
    private Color orginalMainTexColour;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        activated = false;
        renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.enabled = false;
        offset = 0.0f;
        if (renderer.material.GetTexture("_MainTex") != null)
        {
            renderer.material.GetTexture("_MainTex").wrapMode = TextureWrapMode.Clamp;
            orginalMainTexColour = renderer.material.GetColor("_MainTex");
        }
        renderer.material.GetTexture("_BumpMap").wrapMode = TextureWrapMode.Clamp;
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

            if (renderer.material.GetTexture("_MainTex") != null)
            {
                //Fade out the tint colour
                Color colour = renderer.material.GetColor("_MainTex");
                Color faded = Color.Lerp(colour, Color.white, Time.deltaTime);
                //Scroll the textures
                renderer.material.SetTextureOffset("_MainTex", new Vector2(0.0f, offset));
                renderer.material.SetColor("_MainTex", faded);
            }
            
            renderer.material.SetTextureOffset("_BumpMap", new Vector2(0.0f, offset));

            //If we have scrolled off
            if (renderer.material.GetTextureOffset("_BumpMap").y >= 1.0f)
            {
                Deactivate();
            }

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
        //StartCoroutine(FadeOut());
    }

    public void Deactivate()
    {
        //Reset main texture colour and offset and normal map offset
        renderer.material.SetTextureOffset("_BumpMap", new Vector2(0.0f, 0.0f));
        if (renderer.material.GetTexture("_MainTex") != null)
        {
            renderer.material.SetTextureOffset("_MainTex", new Vector2(0.0f, 0.0f));
            renderer.material.SetColor("_MainTex", orginalMainTexColour);
        }

        renderer.enabled = false;
        activated = false;
        offset = 0.0f;
    }

    public IEnumerator FadeOut()
    {        
        yield return new WaitForSeconds(fadeSpeed);
        //renderer.material.GetTexture("_MainTex").wrapMode = TextureWrapMode.Clamp;
        //renderer.material.GetTexture("_BumpMap").wrapMode = TextureWrapMode.Clamp;
        if (renderer.material.GetTextureOffset("_BumpMap").y >= 1.0f)
        {
            renderer.enabled = false;
            activated = false;
        }
        
    }
}
