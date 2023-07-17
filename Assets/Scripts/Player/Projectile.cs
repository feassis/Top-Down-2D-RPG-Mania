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
        if(SameTeam(e) || e.Collision.gameObject.tag == "CameraConfiner"
            || e.Collision.gameObject.tag == "Canopy")
        {
            return;
        }

        Destroy(gameObject);
    }

    private bool SameTeam(CollisionEventHandler e)
    {
        if (isPlayer)
        {
            return e.Collision.gameObject.tag == "Player";
        }

        return e.Collision.gameObject.tag == "Enemy";
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + ShootDirection * speed * Time.fixedDeltaTime);
    }
}
