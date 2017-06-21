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
    private float distortionAmount;
    [SerializeField]
    private float distrtionFadeSpeed;
    [SerializeField]
    private bool nonUniformDropMovement;
    private bool activated;
    private MeshRenderer renderer;
    float offset;
    private float distortionVal;
    private Color orginalMainTexColour;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        activated = false;
        renderer = gameObject.GetComponent<MeshRenderer>();
        gameObject.SetActive(false);
        renderer.enabled = false;
        offset = 0.0f;
        distortionVal = distortionAmount;
        if (renderer.material.GetTexture("_MainTex") != null)
        {
            renderer.material.GetTexture("_MainTex").wrapMode = TextureWrapMode.Clamp;
            orginalMainTexColour = renderer.material.GetColor("_MainTex");
        }
        renderer.material.GetTexture("_BumpMap").wrapMode = TextureWrapMode.Clamp;
        renderer.material.SetFloat("_BumpAmt", distortionAmount);
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
                distortionVal -= Time.deltaTime * Random.Range(0.01f, distrtionFadeSpeed);//(0.0001f, 0.007f);
            }
            else
            {
                offset += (Time.deltaTime * scrollSpeed);
                distortionVal -= (Time.deltaTime * distrtionFadeSpeed); 
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
            renderer.material.SetFloat("_BumpAmt", distortionVal);

            //If we have scrolled off
            if (renderer.material.GetFloat("_BumpAmt") <= 0.0f)
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
        gameObject.SetActive(true);
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

        activated = false;
        offset = 0.0f;
        distortionVal = distortionAmount;
        renderer.material.SetFloat("_BumpAmt", distortionAmount);
        gameObject.SetActive(false);
        renderer.enabled = false;
    }

    public IEnumerator FadeOut()
    {        
        yield return new WaitForSeconds(fadeSpeed);
        //renderer.material.GetTexture("_MainTex").wrapMode = TextureWrapMode.Clamp;
        //renderer.material.GetTexture("_BumpMap").wrapMode = TextureWrapMode.Clamp;
        if (renderer.material.GetTextureOffset("_BumpMap").y >= 1.0f)
        {
            gameObject.SetActive(false);
            renderer.enabled = false;
            activated = false;
        }
        
    }
}
