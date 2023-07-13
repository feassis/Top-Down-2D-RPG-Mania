using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float knockbackTimer = 0.3f;

    public event EventHandler OnKnockbackStart;
    public event EventHandler OnKnockbackFinish;

    public bool GettingKnockedBack { get; private set; }

    public void GetknockedBack(Vector3 damageSource, float knockBackForce)
    {
        OnKnockbackStart?.Invoke(this, EventArgs.Empty);
        GettingKnockedBack = true;
        Vector2 diference = (transform.position - damageSource).normalized * knockBackForce * rb.mass;
        rb.AddForce(diference, ForceMode2D.Impulse);

        StartCoroutine(ResetKnockBack());
    }

    private IEnumerator ResetKnockBack()
    {
        yield return new WaitForSeconds(knockbackTimer);
        GettingKnockedBack = false;
        OnKnockbackFinish?.Invoke(this, EventArgs.Empty);
    }
}
