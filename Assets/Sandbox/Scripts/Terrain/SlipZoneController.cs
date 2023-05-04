using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipZoneController : MonoBehaviour
{
    [SerializeField] float _delayDestroyTime;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, _delayDestroyTime);
    }
}
