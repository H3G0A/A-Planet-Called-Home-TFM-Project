using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWallController : MonoBehaviour
{
    [SerializeField] bool _readyToBreak;
    [SerializeField] Material _frozenMaterial;
    
    public void hitByIceOrb()
    {
        _readyToBreak = true;
        gameObject.GetComponent<Renderer>().material = _frozenMaterial;
    }

    public void hitByDispersionOrb(){
        if(_readyToBreak){
            Destroy(gameObject);
        }
    }
}
