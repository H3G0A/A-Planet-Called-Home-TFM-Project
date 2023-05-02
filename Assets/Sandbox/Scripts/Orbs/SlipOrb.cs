using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipOrb : OrbBehaviour
{
    [SerializeField] GameObject SlipZone;
    [SerializeField] float _radius;

    protected override void ApplyEffect(Collision collision) //The orb change the weigth of the object that impacts.
    {        
        Collider[] _colliders = Physics.OverlapSphere(transform.position, _radius); //Store every collider in range
        foreach(Collider col in _colliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if(rb != null) //Range hits RigidBody
            {
                 Instantiate(SlipZone, this.transform.localPosition, col.transform.localRotation);
            }
        }
    }
}
