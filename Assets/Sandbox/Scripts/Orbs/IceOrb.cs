using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceOrb : OrbBehaviour
{
    [SerializeField] GameObject _iceZone;
    [SerializeField] float _radius;

    protected override void ApplyEffect(Collision collision)
    {
        GameObject objectCollision = collision.gameObject;
        if(objectCollision.tag.Equals(GlobalParameters.BREAKABLE_WALL_TAG)){
            Renderer _breakablWallRenderer = objectCollision.GetComponent<Renderer>();
            _breakablWallRenderer.material.SetColor("_Color", Color.white);
            BreakableWallController breakableWallScript = objectCollision.GetComponent<BreakableWallController>();
            breakableWallScript.hitByIceOrb();
        }else {
            Instantiate(_iceZone, transform.position, Quaternion.identity);
        }
    }
}
