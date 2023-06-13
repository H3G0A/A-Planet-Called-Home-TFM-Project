using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public class LoadingZone : MonoBehaviour
{
    [SerializeField] Scenes _nextScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
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
