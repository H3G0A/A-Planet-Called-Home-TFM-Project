using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public abstract class OrbBehaviour : MonoBehaviour
{
    public abstract int ID { get; protected set; }
    [SerializeField] protected GameObject _prefab;

    //List of object TAGS which their collisions with the orb will be ignored
    readonly List<string> _ignoreCollision = new(){
                                                    ORB_TAG,
                                                    PLAYER_TAG
                                                    };

    readonly List<string> _applyOnTrigger = new() {
                                                   WATER_TAG
                                                   };

    private void Start() //Destroy after a while if has not collided
    {
        Destroy(this.gameObject, 5);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_ignoreCollision.Contains(collision.collider.tag))
        {
            DisableOrb();
            ApplyEffect(collision);
            Destroy(this.gameObject);
        }
    }

    protected void DisableOrb()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<Collider>().enabled = false;
    }

    protected abstract void ApplyEffect(Collision collision);
}
