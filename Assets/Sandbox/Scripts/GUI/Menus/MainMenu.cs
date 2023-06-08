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
        SceneLoader.Instance.LoadScene(GameManager.Instance.CurrentLevel);
    }

    public void OnNewGame()
    {
        DataPersistenceManager.Instance.NewGame();
        SceneLoader.Instance.LoadScene(GameManager.Instance.CurrentLevel);
    }

    public void OnExit()
    {
        GameManager.Instance.ExitGame();
    }
}
