using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public class MainMenu : MonoBehaviour
{
    public void OnContinue()
    {
        SceneLoader.Instance.LoadScene(Scenes.HegoaSandbox);
    }

    public void OnNewGame()
    {
        SceneLoader.Instance.LoadScene(Scenes.HegoaSandbox);
    }

    public void OnExit()
    {
        GameManager.Instance.ExitGame();
    }
}
