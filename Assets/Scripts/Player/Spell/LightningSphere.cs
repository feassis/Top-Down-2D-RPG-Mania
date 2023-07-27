using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSphere : DamageSource
{
    [SerializeField] private float lifetime = 0.5f;
    [SerializeField] private GameObject visuals;
    [SerializeField] private GameObject particles;
    [SerializeField] private Collider2D collider;
    [SerializeField] private AudioSource lightningAudioSource;

    private void Awake()
    {
        StartCoroutine(DestroySelf());
    }

    private IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(lifetime);

        visuals.gameObject.SetActive(false);
        particles.gameObject.SetActive(false);
        collider.enabled = false;

        while (lightningAudioSource.isPlaying)
        {
            yield return null;
        }

        Destroy(gameObject);
    }

    public void SetColliderRadious(float radius)
    {
        transform.localScale = new Vector3 (radius, radius, radius);
    }
}
