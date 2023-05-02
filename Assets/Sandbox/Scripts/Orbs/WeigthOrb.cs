using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeigthOrb : OrbBehaviour
{
    [SerializeField] float _magnitude;
    [SerializeField] bool _augment;
    [SerializeField] float _radius;

    protected override void ApplyEffect(Collision collision) //The orb change the weigth of the object that impacts.
    {        
        Collider[] _colliders = Physics.OverlapSphere(transform.position, _radius); //Store every collider in range
        foreach(Collider col in _colliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if(rb != null) //Range hits RigidBody
            {
                if(_augment){
                    rb.mass = rb.mass * _magnitude;
                } else {
                    rb.mass = rb.mass / _magnitude;
                }
            }
        }
    }
}
