using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public class IceOrb : OrbBehaviour
{
    [SerializeField] GameObject _iceZone;
    [SerializeField] float _radius;

    protected override void ApplyEffect(Collision collision)
    {
        GameObject objectCollision = collision.gameObject;

        if(objectCollision.tag.Equals(BREAKABLE_WALL_TAG))
        {
            Renderer _breakablWallRenderer = objectCollision.GetComponent<Renderer>();
            BreakableWallController breakableWallScript = objectCollision.GetComponent<BreakableWallController>();

            _breakablWallRenderer.material.SetColor("_Color", Color.white);
            breakableWallScript.hitByIceOrb();
        }
        else if(objectCollision.layer == GROUND_LAYER)
        {
            Instantiate(_iceZone, transform.position, Quaternion.identity);
        }
    }
}
