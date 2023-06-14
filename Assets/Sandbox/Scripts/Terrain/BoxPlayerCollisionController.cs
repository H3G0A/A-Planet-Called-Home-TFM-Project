using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public class BoxPlayerCollisionController : MonoBehaviour
{
    float _verticalSpeed;
    Rigidbody _rb;
    Vector3 _spawnPoint;

    private void Awake()
    {
        _rb = this.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _spawnPoint = this.transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(PLAYER_TAG))
        {
            ManagePlayerCollision();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(WATER_TAG))
        {
            Respawn();
        }
    }

    private void ManagePlayerCollision()
    {
        _verticalSpeed = _rb.velocity.y;
        _rb.isKinematic = true;
        StartCoroutine(nameof(OriginalCostraints));
    }

    private void Respawn()
    {
        StartCoroutine(WaitAndRespawn());
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
    /// <summary>
    /// Waits some time before placing the box at its spawn point
    /// </summary>
    IEnumerator WaitAndRespawn()
    {
        yield return new WaitForSeconds(1);
        this.transform.position = _spawnPoint;
    }
}
