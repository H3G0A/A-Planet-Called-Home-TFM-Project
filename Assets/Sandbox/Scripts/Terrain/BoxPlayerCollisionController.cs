using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public class BoxPlayerCollisionController : MonoBehaviour
{
    private float _verticalSpeed;
    private Rigidbody _rb;

    void Awake()
    {
        _rb = this.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(PLAYER_TAG))
        {
            ManagePlayerCollision();
        }
    }

    private void ManagePlayerCollision()
    {
        _verticalSpeed = _rb.velocity.y;
        _rb.isKinematic = true;
        StartCoroutine(nameof(OriginalCostraints));
    }

    /// <summary>
    /// Waits 1 frame before turning the rigidbody back to non-kinematic
    /// </summary>
    IEnumerator OriginalCostraints()
    {
        yield return 0;
        _rb.isKinematic = false;
        _rb.velocity = new(0, _verticalSpeed, 0);
    }
}
