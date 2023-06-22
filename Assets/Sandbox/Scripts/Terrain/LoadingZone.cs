using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public class LoadingZone : MonoBehaviour
{
    [SerializeField] Scenes _nextScene;
    [SerializeField] AudioClip _portalEnterSound;
    [SerializeField] bool _isPortal = false;
    AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            if(_isPortal) _audioSource.PlayOneShot(_portalEnterSound);

            other.GetComponent<PlayerInputController>().enabled = false;

            //If next scene is a gameplay level and not a menu nor a cinematic, set a new "GameLevel" in GameManager
            GameLevels nextLevel;
            bool isGameLevel = Enum.TryParse(_nextScene.ToString(), true, out nextLevel);
            
            if (isGameLevel)
            {
                GameManager.Instance.CurrentLevel = nextLevel;
            }
            

            DataPersistenceManager.Instance.SaveGame();
            SceneLoader.Instance.LoadScene(_nextScene);
        }
    }
}
