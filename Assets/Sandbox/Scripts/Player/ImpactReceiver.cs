using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactReceiver : MonoBehaviour
{
    [SerializeField] float mass = 1;
    Vector3 impact = Vector3.zero;
    CharacterController _charController;

    ////////////////////////////////////////////////////////////////////////////////

    private void Start()
    {
        _charController = this.GetComponent<CharacterController>();
    }

    private void Update()
    {
        if(impact.magnitude > 0.2)
        {
            _charController.Move(impact * Time.deltaTime);
            impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    
    public void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();
        impact += dir * force / mass;
    }
}
