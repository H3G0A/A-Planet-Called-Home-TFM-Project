using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public abstract class OrbBehaviour : MonoBehaviour
{
    //List of object TAGS which their collisions will be ignored by the orb
    List<string> _ignoreCollisions = new(){ 
                                            ORB_TAG, PLAYER_TAG
                                        };

    ////////////////////////////////////////////////////////////////////////////////////

    private void Start()//Destroy after a while if has not collided
    {
        Destroy(this.gameObject, 10);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_ignoreCollisions.Contains(collision.collider.tag)) //Avoid collision between orbs
        {
            Debug.Log(collision.gameObject.name);
            DisableOrb();
            ApplyEffect();
            Destroy(this.gameObject);
        }
    }

    private void DisableOrb()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<Collider>().enabled = false;
    }
    protected abstract void ApplyEffect();
}
