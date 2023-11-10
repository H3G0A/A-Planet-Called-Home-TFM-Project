using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public abstract class OrbBehaviour : MonoBehaviour
{
    [Header("Physics")]
    [Range(-20f, 0f)] public float Gravity = -9.8f;
    [SerializeField] float _lifeTime = 3;

    [Header("Audio")]
    [SerializeField] protected AudioClip _orbEffectSound;
    protected AudioSource _audioSource;

    Rigidbody _rb;

    public abstract int ID { get; protected set; }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Start() //Destroy after a while if has not collided
    {
        Invoke(nameof(DestroySelf), _lifeTime);
    }

    private void FixedUpdate()
    {
        if (!_rb.useGravity) _rb.AddForce(new(0, Gravity, 0), ForceMode.Acceleration);
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

    protected void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
