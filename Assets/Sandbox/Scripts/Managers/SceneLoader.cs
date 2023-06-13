using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GlobalParameters;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    public GameObject LoadingScreen;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(GlobalParameters.Scenes sceneName)
    {
        StartCoroutine(LoadSceneCorrutine(sceneName));
    }

    private IEnumerator LoadSceneCorrutine(GlobalParameters.Scenes sceneName)
    {
        //Show loading screen while scene is loading
        LoadingScreen.SetActive(true);
        
        Cursor.lockState = CursorLockMode.Locked;

        //Start loading scene
        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName.ToString());

        //Prevent scene from completely loading
        scene.allowSceneActivation = false;

        //Wait in case loading is too fast, for better transition
        yield return new WaitForSecondsRealtime(0.35f);
        
        //Wait until scene is loaded
        while (scene.progress < 0.9f)
        {
            yield return null;
        }

        //Allow scene to completely load
        scene.allowSceneActivation = true;

        //Hide loading screen
        LoadingScreen.SetActive(false);
    }
}
