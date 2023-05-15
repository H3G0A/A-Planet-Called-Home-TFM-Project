using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenWater : MonoBehaviour
{
    [SerializeField] bool _isPermanent = false;
    [SerializeField] float _delayDestroyTimer = 10;

    void Start()
    {
        if (!_isPermanent) Destroy(gameObject, _delayDestroyTimer);
    }
}
