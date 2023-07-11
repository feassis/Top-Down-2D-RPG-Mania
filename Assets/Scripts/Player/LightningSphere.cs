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
}
