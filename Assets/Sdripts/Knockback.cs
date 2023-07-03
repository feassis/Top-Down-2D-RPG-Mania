using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float knockbackTimer = 0.5f;

    public bool GettingKnockedBack { get; private set; }

    public void GetknockedBack(Vector3 damageSource, float knockBackForce)
    {
        GettingKnockedBack = true;
        Vector2 diference = (transform.position - damageSource).normalized * knockBackForce * rb.mass;
        rb.AddForce(diference, ForceMode2D.Impulse);

        StartCoroutine(ResetKnockBack());
    }

    private IEnumerator ResetKnockBack()
    {
        yield return new WaitForSeconds(knockbackTimer);
        GettingKnockedBack = false;
    }
}
