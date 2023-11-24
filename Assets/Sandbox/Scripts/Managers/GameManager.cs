using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public bool GamePaused { get; private set; }
    public string CurrentControlScheme = GlobalParameters.KEYBOARD_SCHEME;
    public bool CanInteract = false;

    public PlayerInputController PlayerInputController_;
    public OrbLauncher OrbLauncher_;
    public FirstPersonController FirstPersonController_;
    public CharacterController PlayerController_;
    public InteractionHUD InteractionHUD_;
   
    public GlobalParameters.GameLevels CurrentLevel;

    public GameObject _HUDTextNote;

    int _pauseCounter;

    [Serializable]
    public class Orb
    {
        public GameObject Prefab;
        public bool Available;
    }
    public List<Orb> OrbStash;

    float _lastTimeScale = 1;

   
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Initialize();
        
        GamePaused = false;

    }

    

    private void Update()
    {
        if (FirstPersonController_ == null) return;

        if (FirstPersonController_.InWater)
        {
            OrbLauncher_.IsEnabled = false;
        }
        else if(!FirstPersonController_.InWater && !OrbLauncher_.IsEnabled)
        {
            OrbLauncher_.IsEnabled = true;
        }
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
            //If gameObject is active it's LoadData method will be called on scene load along with the persistence manager's
            gameObject.SetActive(false);
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
        
        // No need to pause again if it was already paused
        if(_pauseCounter++ > 0) return;

        // Stop all time based events
        _lastTimeScale = Time.timeScale;
        Time.timeScale = 0;

        // Stop listening for some inputs
        if (PlayerInputController_ != null) PlayerInputController_.enabled = false;

        // Pause audio
        AudioListener.pause = true;

    }

    public void ResumeGame()
    {
        // Do not resume until every element that pauses the game dissapears
        if(--_pauseCounter > 0) return;

        GamePaused = false;
        Time.timeScale = _lastTimeScale;

        if (PlayerInputController_ != null) PlayerInputController_.enabled = true;

        _pauseCounter = 0;

        AudioListener.pause = false;
    }

    public void ForceResumeGame()
    {
        _pauseCounter = 0;
        ResumeGame();
    }

    public void EnableOrb(GlobalParameters.Orbs orbName)
    {
        int orbID = (int)orbName;
        foreach(Orb selectedOrb in OrbStash)
        {
            int selectedOrbID = selectedOrb.Prefab.GetComponent<OrbBehaviour>().ID;
            if(orbID == selectedOrbID)
            {
                selectedOrb.Available = true;
            }
        }

        OrbLauncher_.LoadOrbs();
    }

    public void DisableOrb(GlobalParameters.Orbs orbName)
    {
        int orbID = (int)orbName;
        foreach (Orb selectedOrb in OrbStash)
        {
            int selectedOrbID = selectedOrb.Prefab.GetComponent<OrbBehaviour>().ID;
            if (orbID == selectedOrbID)
            {
                selectedOrb.Available = false;
            }
        }

        OrbLauncher_.LoadOrbs();
    }

    public void LoadData(GameData data)
    {
        //Level
        this.CurrentLevel = (GlobalParameters.GameLevels) Enum.Parse(typeof(GlobalParameters.GameLevels), data.CurrentLevel);

        //Orbs
        foreach(Orb orb in OrbStash)
        {
            orb.Available = false;
            int orbId = orb.Prefab.GetComponent<OrbBehaviour>().ID;

            if (data.ActiveOrbs.Contains(orbId))
            {
                orb.Available = true;
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        //Level
        data.CurrentLevel = this.CurrentLevel.ToString();

        //Orbs
        data.ActiveOrbs.Clear();

        foreach (Orb orb in OrbStash)
        {
            int orbId = orb.Prefab.GetComponent<OrbBehaviour>().ID;

            if (orb.Available)
            {
                data.ActiveOrbs.Add(orbId);
            }
        }
    }

    public void SetInteractPromptActive(bool active)
    {
        InteractionHUD_.SetPromptActive(active);
    }

    public void ReadNote(string _text)
    {
        PauseGame();
        _HUDTextNote.GetComponentInChildren<TextMeshProUGUI>().text = _text;
        _HUDTextNote.SetActive(true);
    }

    public void HideNote()
    {
        ResumeGame();
        _HUDTextNote.SetActive(false);
    }

    public void LinkObject(GameObject obj)
    {
        switch (obj.tag)
        {
            case GlobalParameters.HUD_TEXT_NOTE_TAG:
                _HUDTextNote = obj;
                break;

            default:
                break;
        };
    }
}
