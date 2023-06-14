using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LigthCrystalController : MonoBehaviour
{
    [SerializeField] bool _iluminate;

    [SerializeField] Light _ligthElement = null;
    [SerializeField] float _lighIncrement = 0.5f;
    float _ligthIntensity = 0.0f;
    
    void Start()
    {
        _ligthElement = this.GetComponent<Light>();    
    }

    // Update is called once per frame
    void Update()
    {
        lightIntensityChanger();
    }

    public void increaseIntensity()
    {
        _iluminate = true;
    }

    public void decreaseIntensity()
    {
        _iluminate = false;
    }

    void lightIntensityChanger(){
        if(_iluminate)
        {
            if(_ligthIntensity < 1.5f )
            {
                _ligthIntensity = _ligthIntensity + _lighIncrement * Time.deltaTime;
            } else
            {
                //decreaseIntensity();
            }
        } else 
        {
            if(_ligthIntensity > 0 ) 
            {
                _ligthIntensity = _ligthIntensity - (_lighIncrement / 2) * Time.deltaTime;
            }
        }
        _ligthElement.intensity = _ligthIntensity;
    }
}
