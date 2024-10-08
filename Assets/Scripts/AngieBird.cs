using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AngieBird : MonoBehaviour
{
    private Rigidbody2D _rb;
    private CircleCollider2D _circleCollider;

    private bool _hasBeenLaunched;
    private bool _shouldFaceVelDirection;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        _rb.isKinematic = true;
        _circleCollider.enabled = false;
    }

    private void FixedUpadte()
    {
        if (_hasBeenLaunched && _shouldFaceVelDirection)
        {

            transform.right = _rb.velocity;

        }



    }


    public void LaunchBird(Vector2 direction, float force)
    {
        _rb.isKinematic = false;  // Set isKinematic to false before applying force
        _circleCollider.enabled = true;  // Enable collider

        // Apply the force to launch the bird
        _rb.AddForce(direction * force, ForceMode2D.Impulse);

        _hasBeenLaunched = true;
        _shouldFaceVelDirection = true;


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        _shouldFaceVelDirection = false;


    }

}
