using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OrbBehaviour : MonoBehaviour
{
    const string ORB_TAG = "Orb";

    //Destroy after a while if has not collided
    private void Start()
    {
        Destroy(this.gameObject, 10);
    }

    //Apply orb effect on collision and destroy gameObject
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(ORB_TAG)) //Avoid collision between orbs
        {
            DisableObject();
            ApplyEffect();
            Destroy(this.gameObject);
        }   
    }

    private void DisableObject()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<Collider>().enabled = false;
    }
    protected abstract void ApplyEffect();
}
