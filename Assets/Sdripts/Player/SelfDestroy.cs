using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;

    private void Update()
    {
        if(ps && !ps.IsAlive())
        {
            DestrotSelf();
        }
    }

    public void DestrotSelf()
    {
        Destroy(gameObject);
    }
}
