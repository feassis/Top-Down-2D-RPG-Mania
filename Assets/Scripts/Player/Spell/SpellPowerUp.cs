using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellPowerUp : MonoBehaviour
{
    [SerializeField] private SpellType spell;
    [SerializeField] private int levelIncrease;
    [SerializeField] private float powerUpDuration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            SpellTome.Instance.AddPowerUp(spell, levelIncrease, powerUpDuration);

            Destroy(gameObject);
        }
    }
}
