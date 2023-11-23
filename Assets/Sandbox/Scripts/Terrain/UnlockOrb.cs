using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public class UnlockOrb : MonoBehaviour, IInteractable
{
    [SerializeField] Orbs _orbToUnlock;
    [SerializeField] AudioClip _orbPickupSound;
    [SerializeField] GameObject _orbMesh;

    public void TriggerInteraction()
    {
        AudioSource.PlayClipAtPoint(_orbPickupSound, this.transform.position);

        GameManager.Instance.EnableOrb(_orbToUnlock);

        gameObject.layer = 0;
        _orbMesh.SetActive(false);
    }
}
