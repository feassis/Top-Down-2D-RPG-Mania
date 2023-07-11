using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : DamageSource
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    public Vector3 ShootDirection;

    private void Awake()
    {
        OnObstacleHited += DestroySelf;
    }

    private void DestroySelf(object sender, CollisionEventHandler e)
    {
        if(e.Collision.gameObject.tag == "Player" 
            || e.Collision.gameObject.tag == "CameraConfiner")
        {
            return;
        }

        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + ShootDirection * speed * Time.fixedDeltaTime);
    }
}
