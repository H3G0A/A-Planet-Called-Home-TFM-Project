using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public abstract class OrbBehaviour : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] protected AudioClip _orbEffectSound;
    protected AudioSource _audioSource;

    public abstract int ID { get; protected set; }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start() //Destroy after a while if has not collided
    {
        Destroy(this.gameObject, 3);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        
        DisableOrb();
        ApplyEffect(collision);
        Destroy(this.gameObject);
        
    }

    protected void DisableOrb()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<Collider>().enabled = false;
    }

    protected abstract void ApplyEffect(Collision collision);
}
