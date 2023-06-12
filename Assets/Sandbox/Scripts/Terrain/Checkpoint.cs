using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public class Checkpoint : MonoBehaviour
{
    public Transform playerSpawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            FirstPersonController playerFPController = other.GetComponent<FirstPersonController>();
            
            if(playerFPController._lastCheckpoint != playerSpawnPoint.position)
            {
                playerFPController._lastCheckpoint = playerSpawnPoint.position;

                DataPersistenceManager.Instance.SaveGame();
            }
        }
    }
}
