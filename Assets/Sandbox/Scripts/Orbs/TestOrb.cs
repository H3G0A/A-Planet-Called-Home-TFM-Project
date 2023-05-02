using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOrb : OrbBehaviour
{
    protected override void ApplyEffect(Collision collision)
    {
        Debug.Log("Apply orb effect");
    }
}
