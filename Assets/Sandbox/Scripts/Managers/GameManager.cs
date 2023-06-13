using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public bool GamePaused { get; private set; }
    public PlayerInputController PlayerInputController_;
    public OrbLauncher OrbLauncher_;
    public FirstPersonController FirstPersonController_;
    public CharacterController PlayerController;
   
    public GlobalParameters.GameLevels CurrentLevel;

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
        data.ActiveOrbs = new List<int>();

        foreach(Orb orb in OrbStash)
        {
            int orbId = orb.Prefab.GetComponent<OrbBehaviour>().ID;

            if (orb.Available)
            {
                data.ActiveOrbs.Add(orbId);
            }
        }
    }
}
