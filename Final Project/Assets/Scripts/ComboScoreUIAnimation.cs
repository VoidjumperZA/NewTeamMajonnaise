using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboScoreUIAnimation : MonoBehaviour {
    [SerializeField]
    private Text selfText;
    private Image selfImage;
    private Combo combo;
    private bool fading;
    [SerializeField]
    private bool moveToCentre;
    [SerializeField]
    private float timeWaitingBeforeAutoFade;
    // Use this for initialization
    void Start () {
        selfImage = gameObject.GetComponent<Image>();
        //combo = GameObject.Find("Manager").GetComponent<Combo>();
        fading = false;
        if (moveToCentre == false)
        {
            StartCoroutine(FadeAfterTime());
        }
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        //Moving towards the score
        if (fading == false && moveToCentre == true)
        {
            Vector3 hookPosition = Camera.main.WorldToScreenPoint(GameManager.Hook.transform.position);
            Vector3 assetPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            float speed = 0.05f;// combo.GetComboScoreUIMovementSpeed();

            Vector3 differenceVector = (hookPosition - gameObject.transform.position);
            if (differenceVector.magnitude >= speed) gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, hookPosition, speed);
            if (differenceVector.magnitude < 0.1f)
            {
                fading = true;
                GameManager.Scorehandler.AddComboScore();
                /*ScoreHandler scoreHandler = GameObject.Find("Manager").GetComponent<ScoreHandler>();
                scoreHandler.AddScore(scoreHandler.GetComboScoreValue(), false, false);*/
            }
        }
        //Fading out
        else
        {
            float alphaFade = 0.03f;
            //TEXT FADING
            //Create a new colour that is equal to the text
            Color faded = selfText.GetComponent<Text>().color;
            //Minus the value we want to fade by
            faded.a = faded.a - alphaFade;
            //Set our text back to that colour
            selfText.GetComponent<Text>().color = faded;

            //IMAGE FADING
            //Use that same colour, but set it to the image's colour this time
            faded = selfImage.GetComponent<Image>().color;
            //Minus the faded value
            faded.a = faded.a - alphaFade;
            //And set it back
            selfImage.GetComponent<Image>().color = faded;
            //           if (faded.a < alphaFade) Destroy(gameObject);
        }
		
    }

    public void SetScoreText(int pScore)
    {
        selfText.text = "+" + pScore + "!";
    }

    private IEnumerator FadeAfterTime()
    {
        yield return new WaitForSeconds(timeWaitingBeforeAutoFade);
        fading = true;
    }
}
