using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeigthOrb : OrbBehaviour
{
    [SerializeField] bool _augment;

    public override int ID { get; protected set; } = (int)GlobalParameters.Orbs.WEIGHT;
    //int _id = (int)GlobalParameters.Orbs.WEIGHT;

    protected override void ApplyEffect(Collision collision) //The orb change the weigth of the object that impacts.
    {        
        GameObject objectCollision = collision.gameObject;
        Rigidbody rb = objectCollision.GetComponent<Rigidbody>();
        if(objectCollision.tag.Equals(GlobalParameters.DISPLACE_BOX_TAG)){
            if(_augment){
                rb.constraints = RigidbodyConstraints.FreezePosition;
                Debug.Log(rb.constraints.ToString());
            }else{
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                Debug.Log(rb.constraints.ToString());
            }
        }
        if(objectCollision.tag.Equals(GlobalParameters.ELEVATOR_TAG)){
            ElevatorMovementController elevator = objectCollision.GetComponent<ElevatorMovementController>();
            elevator.WeigthOrbImpact(_augment);
        }
    }

    public void changeAugment(bool _newAugment){
        _augment = _newAugment;
    }
}
