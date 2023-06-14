using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWallController : MonoBehaviour
{
    [SerializeField] bool _readyToBreak;
    
    public void hitByIceOrb()
    {
        _readyToBreak = true;
    }

    public void hitByDispersionOrb(){
        if(_readyToBreak){
            Destroy(gameObject);
        }
    }
}
