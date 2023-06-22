using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public class UnlockOrb : MonoBehaviour
{
    [SerializeField] Orbs _orbToUnlock;
    [SerializeField] AudioClip _orbPickupSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            AudioSource.PlayClipAtPoint(_orbPickupSound, this.transform.position);

            GameManager.Instance.EnableOrb(_orbToUnlock);
            this.gameObject.SetActive(false);
        }
    }
}
