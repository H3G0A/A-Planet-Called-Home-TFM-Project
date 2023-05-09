using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceOrb : OrbBehaviour
{
    [SerializeField] GameObject _iceZone;
    [SerializeField] float _radius;

    protected override void ApplyEffect(Collision collision)
    {        
        Collider[] _colliders = Physics.OverlapSphere(transform.position, _radius); //Store every collider in range
        foreach(Collider col in _colliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if(rb != null) //Range hits RigidBody
            {
                 Instantiate(_iceZone, this.transform.localPosition, Quaternion.identity);
            }
        }
    }
}
