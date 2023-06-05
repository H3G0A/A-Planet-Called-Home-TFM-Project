using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool GamePaused { get; private set; }

    // Player
    public PlayerInputController PlayerInputController_;

    // Time
    float _lastTimeScale = 1;

    private void Awake()
    {
        Initialize();
        GamePaused = false;
    }

    private void Initialize()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        GamePaused = true;

        // Stop all time based events
        _lastTimeScale = Time.timeScale;
        Time.timeScale = 0;

        // Stop listening for some inputs
        if (PlayerInputController_ != null) PlayerInputController_.enabled = false;

        // Stop camera from moving

    }

    

    public void ResumeGame()
    {
        GamePaused = false;
        Time.timeScale = _lastTimeScale;

        if (PlayerInputController_ != null) PlayerInputController_.enabled = true;
    }
}
