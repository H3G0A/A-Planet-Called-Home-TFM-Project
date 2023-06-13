using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _continueButton;

    private void Start()
    {
        if (DataPersistenceManager.Instance.HasGameData())
        {
            _continueButton.SetActive(true);
        }
        else
        {
            _continueButton.SetActive(false);
        }
    }

    public void OnContinue()
    {
        Scenes sceneToLoad = (Scenes) Enum.Parse(typeof(Scenes), GameManager.Instance.CurrentLevel.ToString());
        SceneLoader.Instance.LoadScene(sceneToLoad);
    }

    public void OnNewGame()
    {
        DataPersistenceManager.Instance.NewGame();
        SceneLoader.Instance.LoadScene(Scenes.NewGameCinematic);
    }

    public void OnExit()
    {
        GameManager.Instance.ExitGame();
    }
}
