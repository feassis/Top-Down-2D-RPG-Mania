using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSphere : DamageSource
{
    [SerializeField] private float lifetime = 0.5f;

    private void Awake()
    {
        StartCoroutine(DestroySelf());
    }

    private IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    public void SetColliderRadious(float radius)
    {
        transform.localScale = new Vector3 (radius, radius, radius);
    }
}
