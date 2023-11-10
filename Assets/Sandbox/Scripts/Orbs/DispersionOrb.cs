using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispersionOrb : OrbBehaviour
{
    [SerializeField] AudioClip _stickSound;
    [SerializeField] AudioClip _beepSound;

    [Header("Explosion")]
    [SerializeField] float _force;
    [SerializeField] float _radius;
    [SerializeField] bool _diagonalPush = true;
    [SerializeField] bool _verticalPush = false;

    GameObject _parent = null;
    ContactPoint _collisionContact;
    Vector3 _collisionOffset;

    ImpactReceiver _impactReceiver;
    CharacterController _charController;
    FirstPersonController _FPController;

    public override int ID { get; protected set; } = (int)GlobalParameters.Orbs.DISPERSION;

    private void Update()
    {
        StickToParent();
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        // Object wont dissapear once collided, but will automatically apply its effect after some time
        CancelInvoke(nameof(DestroySelf));
        Invoke(nameof(Activate), 7);

        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        this.gameObject.GetComponent<Collider>().enabled = false;

        _parent = collision.gameObject;
        _collisionContact = collision.GetContact(0);
        _collisionOffset = collision.gameObject.transform.position - this.transform.position;
        _audioSource.PlayOneShot(_stickSound);
    }

    private void ApplyEffect() //The orb pushes all objects inside radius away
    {
        Collider[] _colliders = Physics.OverlapSphere(transform.position, _radius); //Store every collider in range
        
        foreach(Collider _collider in _colliders)
        {
            Rigidbody colliderRb = _collider.GetComponent<Rigidbody>();
            if(colliderRb != null) //Range hits RigidBody
            {
                //Only move the object if the orb has not collided with its top face directly
                if(_parent != _collider.gameObject || _collisionContact.normal != Vector3.up)
                {
                    if (_collider.CompareTag(GlobalParameters.DISPLACE_BOX_TAG))
                    {
                        _collider.GetComponent<AudioSource>().Play();
                    }

                    Vector3 _forceDirection = CalculateForceDirection(this.transform.position, _collider.transform.position);
                    colliderRb.AddForce(_forceDirection * _force, ForceMode.Impulse);
                }
            }
            else if (_collider.gameObject.CompareTag(GlobalParameters.PLAYER_TAG)) //Range hits player
            {
                
                if (!_impactReceiver) _impactReceiver = _collider.GetComponent<ImpactReceiver>();
                if (!_charController) _charController = _collider.GetComponent<CharacterController>();
                if (!_FPController) _FPController = _collider.GetComponent<FirstPersonController>();

                Vector3 _forceDirection = (_collider.transform.TransformPoint(_charController.center) - this.transform.position);

                if(_FPController._verticalVelocity < 0) _FPController._verticalVelocity = 0;
                _impactReceiver.AddImpact(_forceDirection, _force);
            } 
            else if(_collider.gameObject.CompareTag(GlobalParameters.BREAKABLE_WALL_TAG)) //Range hits breakable wall
            {
                BreakableWallController breakableWallScript = _collider.GetComponent<BreakableWallController>();
                breakableWallScript.hitByDispersionOrb();            
            }else if(_collider.gameObject.CompareTag(GlobalParameters.LUMINOUS_CRYSTAL)) {
                LigthCrystalController _lightCrystalController = _collider.GetComponent<LigthCrystalController>();
                _lightCrystalController.increaseIntensity();
            }
        }

        AudioSource.PlayClipAtPoint(_orbEffectSound, this.transform.position);
    }

    private Vector3 CalculateForceDirection(Vector3 explosionCenter, Vector3 objectPosition) //Direction when pushing a non-player object
    {
        Vector3 _forceDirection = objectPosition - explosionCenter;

        float _XAxis = Mathf.Abs(_forceDirection.x);
        float _ZAxis = Mathf.Abs(_forceDirection.z);

        if (!_diagonalPush)
        {
            //Push only in the direction of the strongest axis
            if (_XAxis > _ZAxis)
            {
                _forceDirection.z = 0;
            }
            else
            {
                _forceDirection.x = 0;
            }
        }

        if (!_verticalPush) // Disable vertical movement
        {
            _forceDirection.y = 0; 
        }
        
        return _forceDirection.normalized;
    }

    private void StickToParent()
    {
        if (_parent != null)
        {
            this.transform.position = _parent.transform.position - _collisionOffset;
        }
    }

    public void Activate()
    {
        if (_parent is not null)
        {
            ApplyEffect();
            Destroy(gameObject);
        }
    }

    protected override void ApplyEffect(Collision collision)
    {
        return;
    }
}
