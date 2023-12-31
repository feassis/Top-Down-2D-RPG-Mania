
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField] private GameObject destroyVFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<DamageSource>(out DamageSource dmgSource))
        {
            Instantiate(destroyVFX, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
