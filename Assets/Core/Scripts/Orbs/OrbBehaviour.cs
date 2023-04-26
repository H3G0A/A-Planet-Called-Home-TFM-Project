using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OrbBehaviour : MonoBehaviour
{
    //Apply orb effect on collision and destroy gameObject
    private void OnCollisionEnter(Collision collision)
    {
        ApplyEffect();
        Destroy(gameObject);
    }

    protected abstract void ApplyEffect();
}
