using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private List<GameObject> _objectsToDestroyOnRestart;
    
    private AsyncOperation async = null;
    public void LoadScene(int pIndex)
    {
        SceneManager.LoadScene(pIndex);
    }
    public void LoadSceneAndDestroy(int pIndex)
    {
        //Needs to destroy boat / hook / game manager, etc if replaying the game
        foreach (GameObject go in _objectsToDestroyOnRestart)
        {
            Debug.Log(go.name);
            Destroy(go);
        }
        GameManager.NextScene = 2;
        SceneManager.LoadScene(pIndex);
    }
    public void LoadSceneAsync(int pIndex, float pWaitTime)
    {
        StartCoroutine(CoroutineLoadSceneAsync(pIndex, pWaitTime));
    }
    private IEnumerator CoroutineLoadSceneAsync(int pIndex, float pWaitTime)
    {
        async = SceneManager.LoadSceneAsync(pIndex);
        async.allowSceneActivation = false;


        yield return new WaitForSeconds(pWaitTime);
        Camera.main.transform.SetParent(gameObject.transform);
        async.allowSceneActivation = true;

        // COMMENT if the async load is not complete but the start button was pressed, show the loading screen and loading progress bar. When its complete return.
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) LoadSceneAndDestroy(0);
    }
}
