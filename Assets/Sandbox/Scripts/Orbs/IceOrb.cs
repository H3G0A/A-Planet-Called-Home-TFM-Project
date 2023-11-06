using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public class IceOrb : OrbBehaviour
{
    [SerializeField] float _radius;
    [SerializeField] GameObject _iceZone;
    [SerializeField] GameObject _frozenWater;
    [SerializeField] GameObject _frozenBlock;
    [SerializeField] LayerMask _groundLayers;

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
        else if(objectCollision.CompareTag(WET_FLOOR_TAG))
        {
            Instantiate(_iceZone, transform.position, Quaternion.identity);
        }

        AudioSource.PlayClipAtPoint(_orbEffectSound, this.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(WATER_TAG))
        {
            AudioSource.PlayClipAtPoint(_orbEffectSound, this.transform.position);

            DisableOrb();

            Vector3 pos = new(transform.position.x, other.transform.position.y, transform.position.z);
            Instantiate(_frozenWater, pos, Quaternion.identity);
            
            Destroy(this.gameObject);
        }
        else if (other.gameObject.CompareTag(WATERFALL_TAG))
        {
            DisableOrb();

            AudioSource.PlayClipAtPoint(_orbEffectSound, this.transform.position);

            Vector3 pos = new(other.bounds.center.x, transform.position.y, other.bounds.center.z);
            GameObject block = Instantiate(_frozenBlock, pos, Quaternion.identity);
            other.GetComponent<FrozenBlockCounter>().SetNewBlock(block);
            
            Destroy(this.gameObject);
        }
    }
}
