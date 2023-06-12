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
            GameManager.Instance.CurrentLevel = _nextScene;
            SceneLoader.Instance.LoadScene(_nextScene);
        }
    }
}
