using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : DamageSource
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private Collider2D myCollider;
    [SerializeField] private SpriteRenderer mySprite;
    [SerializeField] private GameObject trail;
    [SerializeField] private GameObject light2D;
     
    public Vector3 ShootDirection;

    private bool hasHited;

    private void Awake()
    {
        OnObstacleHited += DestroySelf;
    }

    private void DestroySelf(object sender, CollisionEventHandler e)
    {
        if(SameTeam(e) || e.Collision.gameObject.tag == "CameraConfiner"
            || e.Collision.gameObject.tag == "Canopy"
            || e.Collision.gameObject.tag == "Goal")
        {
            return;
        }

        hasHited = true;

        if(hitSound != null)
        {
            hitSound.Play();

            StartCoroutine(StartDestructionCorrotine());

            return;
        }

        Destroy(gameObject);
    }

    private IEnumerator StartDestructionCorrotine()
    {
        myCollider.enabled = false;
        mySprite.enabled = false;
        trail.SetActive(false);
        light2D.SetActive(false);

        while (hitSound.isPlaying)
        {
            yield return null;
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
        if (hasHited)
        {
            return;
        }

        rb.MovePosition(transform.position + ShootDirection * speed * Time.fixedDeltaTime);
    }
}
