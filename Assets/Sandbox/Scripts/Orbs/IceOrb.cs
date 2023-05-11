using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceOrb : OrbBehaviour
{
    [SerializeField] GameObject _iceZone;
    [SerializeField] float _radius;

    protected override void ApplyEffect(Collision collision)
    {
        Instantiate(_iceZone, transform.position, Quaternion.identity);
    }
}
