using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject _continueButton;
    AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

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
        _audioSource.Play();

        Scenes sceneToLoad = (Scenes) Enum.Parse(typeof(Scenes), GameManager.Instance.CurrentLevel.ToString());
        SceneLoader.Instance.LoadScene(sceneToLoad);
    }

    public void OnNewGame()
    {
        _audioSource.Play();

        DataPersistenceManager.Instance.NewGame();
        SceneLoader.Instance.LoadScene(Scenes.NewGameCinematic);
    }

    public void OnExit()
    {
        _audioSource.Play();

        GameManager.Instance.ExitGame();
    }
}
