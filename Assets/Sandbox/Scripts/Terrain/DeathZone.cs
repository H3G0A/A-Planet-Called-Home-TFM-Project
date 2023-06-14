using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            other.GetComponent<FirstPersonController>().PlayerRespawn();
        }
    }
}
