using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public class UnlockOrb : MonoBehaviour
{
    [SerializeField] Orbs _orbToUnlock;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            GameManager.Instance.EnableOrb(_orbToUnlock);
            this.gameObject.SetActive(false);
        }
    }
}
