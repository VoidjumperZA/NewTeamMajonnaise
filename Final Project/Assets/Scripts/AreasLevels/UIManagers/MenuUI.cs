using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : BaseUI {

    [SerializeField] private Button _playTutorial;
    [SerializeField] private Button _skipTutorial;
    [SerializeField]
    private Image _playAnim;
    [SerializeField]
    private Image _replayAnim;

    public override void Start ()
    {
        SetActiveButtons(true, _playTutorial, _skipTutorial);
        SetActive(false, _playAnim.gameObject);
        SetActive(false, _replayAnim.gameObject);
        //Debug.Log("MenuUI - Start();");
    }
    public override void Update()
    {

    }
    public void OnPlayTutorialClick()
    {
        StartCoroutine(PlayAnim());
       
    }
    public void OnSkipTutorialClick()
    {
        StartCoroutine(ReplayAnim());
    }
    public override void LeaveSceneTransition()
    {
        TransitionCurtain.GetComponent<Transition>().DownWards();
    }

    private IEnumerator PlayAnim()
    {
        _playTutorial.gameObject.SetActive(false);
        _playAnim.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.6f);

        _playAnim.gameObject.SetActive(false);
        _skipTutorial.gameObject.SetActive(false);
        GameManager.LoadSceneAsync(1, 4);
        GameManager.Camerahandler.SetViewPoint(CameraHandler.FocusPoint.End);
        GameManager.Camerahandler.Play();
        GameManager.Boat.SetState(boat.BoatState.LeaveScene);
        LeaveSceneTransition();
    }

    private IEnumerator ReplayAnim()
    {
        _skipTutorial.gameObject.SetActive(false);
        _replayAnim.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.6f);

        _replayAnim.gameObject.SetActive(false);
        _playTutorial.gameObject.SetActive(false);
        
        GameManager.LoadSceneAsync(3, 4);
        GameManager.Camerahandler.SetViewPoint(CameraHandler.FocusPoint.End);
        GameManager.Camerahandler.Play();
        //GameManager.Camerahandler.Play();
        GameManager.Boat.SetState(boat.BoatState.LeaveScene);
        LeaveSceneTransition();
    }
}
