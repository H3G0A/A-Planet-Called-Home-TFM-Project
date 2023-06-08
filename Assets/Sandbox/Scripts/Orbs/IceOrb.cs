using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public class IceOrb : OrbBehaviour
{
    [SerializeField] float _radius;
    [SerializeField] GameObject _iceZone;
    [SerializeField] GameObject _frozenWater;

    public override int ID { get; protected set; } = (int)GlobalParameters.Orbs.ICE;
    // readonly public int _id = (int)GlobalParameters.Orbs.ICE;

    protected override void ApplyEffect(Collision collision)
    {
        GameObject objectCollision = collision.gameObject;

        if(objectCollision.CompareTag(BREAKABLE_WALL_TAG))
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(WATER_TAG))
        {
            DisableOrb();

            Vector3 _pos = new(transform.position.x, other.transform.position.y, transform.position.z);
            Instantiate(_frozenWater, _pos, Quaternion.identity);
            
            Destroy(this.gameObject);
        }
    }
}
