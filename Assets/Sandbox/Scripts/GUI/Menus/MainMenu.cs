using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GlobalParameters;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button _continueButton;
    AudioSource _audioSource;


    private void Awake()
    {   
        _audioSource = GetComponent<AudioSource>();

    }

    private void Start()
    {
        if (DataPersistenceManager.Instance.HasGameData())
        {
            _continueButton.interactable = true;
        }
        else
        {
            _continueButton.interactable = false;
            _continueButton.GetComponentInChildren<TextMeshProUGUI>().alpha = .15f;
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
