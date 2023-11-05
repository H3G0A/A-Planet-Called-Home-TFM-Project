using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public class IceZone : MonoBehaviour
{
    [SerializeField] bool _isPermanent = false;
    [SerializeField] float _delayDestroyTimer;
    [SerializeField] Vector3 _initialScale;
    [SerializeField] Vector3 _targetScale;
    [SerializeField] float _expandDuration;
    [SerializeField] LayerMask _iceLayers;

    private void Awake()
    {
        transform.localScale = _initialScale;
        transform.Rotate(new(270,0,0));
    }

    void Start()
    {
        if(!_isPermanent) Destroy(gameObject, _delayDestroyTimer);

        StartCoroutine(ExpandIce());
    }

    private IEnumerator ExpandIce()
    {
        float durationDelta = 0;
        while(durationDelta < _expandDuration)
        {
            durationDelta += Time.deltaTime;
            this.transform.localScale = Vector3.Lerp(_initialScale, _targetScale, durationDelta/_expandDuration);
            yield return null;
        }
    }
}
