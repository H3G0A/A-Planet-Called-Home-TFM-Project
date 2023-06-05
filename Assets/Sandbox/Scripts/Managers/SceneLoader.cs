using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [SerializeField] GameObject _loadingScreen;

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
        StartCoroutine(LoadingCorrutine(sceneName));
    }

    private IEnumerator LoadingCorrutine(GlobalParameters.Scenes sceneName)
    {
        Cursor.lockState = CursorLockMode.Locked;

        //Start loading scene
        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName.ToString());

        //Show loading screen while scene is loading
        _loadingScreen.SetActive(true);

        //Prevent scene from completely loading
        scene.allowSceneActivation = false;

        //Wait until scene is loaded
        while (scene.progress < 0.9f)
        {
            yield return null;
        }

        //Wait 1 second in case loading is too fast, for better transition
        yield return new WaitForSeconds(1);

        //Allow scene to completely load
        scene.allowSceneActivation = true;
        //Hide loading screen
        _loadingScreen.SetActive(false);
    }
}
