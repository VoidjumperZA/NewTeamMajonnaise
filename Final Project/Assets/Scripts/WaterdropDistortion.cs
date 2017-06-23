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
    private MeshRenderer _renderer;
    float offset;
    private float distortionVal;
    private Color orginalMainTexColour;
    bool onceStart = false;

    // Use this for initialization
    public void Start()
    {
        if (onceStart == false)
        {
            DontDestroyOnLoad(gameObject);
            activated = false;
            _renderer = gameObject.GetComponent<MeshRenderer>();
            gameObject.SetActive(false);
            _renderer.enabled = false;
            offset = 0.0f;
            distortionVal = distortionAmount;
            if (_renderer.material.GetTexture("_MainTex") != null)
            {
                _renderer.material.GetTexture("_MainTex").wrapMode = TextureWrapMode.Clamp;
                orginalMainTexColour = _renderer.material.GetColor("_MainTex");
            }
            _renderer.material.GetTexture("_BumpMap").wrapMode = TextureWrapMode.Clamp;
            _renderer.material.SetFloat("_BumpAmt", distortionAmount);
            onceStart = true;
        }

        
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

            if (_renderer.material.GetTexture("_MainTex") != null)
            {
                //Fade out the tint colour
                Color colour = _renderer.material.GetColor("_MainTex");
                Color faded = Color.Lerp(colour, Color.white, Time.deltaTime);
                //Scroll the textures
                _renderer.material.SetTextureOffset("_MainTex", new Vector2(0.0f, offset));
                _renderer.material.SetColor("_MainTex", faded);
            }
            
            _renderer.material.SetTextureOffset("_BumpMap", new Vector2(0.0f, offset));
            _renderer.material.SetFloat("_BumpAmt", distortionVal);

            //If we have scrolled off
            if (_renderer.material.GetFloat("_BumpAmt") <= 0.0f)
            {
                Deactivate();
            }

            //Debug.Log("Offset = " + offset);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Activate();
        }
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        activated = true;
        _renderer.enabled = true;
        //StartCoroutine(FadeOut());
    }

    public void Deactivate()
    {
        if (activated == true)
        {
            //Reset main texture colour and offset and normal map offset
            _renderer.material.SetTextureOffset("_BumpMap", new Vector2(0.0f, 0.0f));
            if (_renderer.material.GetTexture("_MainTex") != null)
            {
                _renderer.material.SetTextureOffset("_MainTex", new Vector2(0.0f, 0.0f));
                _renderer.material.SetColor("_MainTex", orginalMainTexColour);
            }

            activated = false;
            offset = 0.0f;
            distortionVal = distortionAmount;
            _renderer.material.SetFloat("_BumpAmt", distortionAmount);
            gameObject.SetActive(false);
            _renderer.enabled = false;
        }
        
    }

    public IEnumerator FadeOut()
    {        
        yield return new WaitForSeconds(fadeSpeed);
        //renderer.material.GetTexture("_MainTex").wrapMode = TextureWrapMode.Clamp;
        //renderer.material.GetTexture("_BumpMap").wrapMode = TextureWrapMode.Clamp;
        if (_renderer.material.GetTextureOffset("_BumpMap").y >= 1.0f)
        {
            gameObject.SetActive(false);
            _renderer.enabled = false;
            activated = false;
        }
        
    }
}
