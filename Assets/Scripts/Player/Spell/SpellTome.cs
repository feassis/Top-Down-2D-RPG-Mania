using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellTome : Singleton<SpellTome>
{
    [SerializeField] private List<Spell> spells;

    private PlayerControls playerControl;
    private int currentSpellIndex = 0;
    private List<AvaliableSpells> avaliableSpells = new List<AvaliableSpells>();

    public event EventHandler OnSpellChange;

    private class AvaliableSpells
    {
        public Spell Spell;
        public float Timer = 0;

        public AvaliableSpells (Spell spell)
        {
            Spell = spell;
        }

        public void CastSpell()
        {
            if(Timer > 0)
            {
                return;
            }

            Spell.CastSpell();
            Timer = Spell.SpellCoolDown;
        }

        public string GetSpellName() => Spell.SpellName;
        public float GetTimer() => (Mathf.Round(Timer * 10f)) / 10.0f;
    }

    protected override void Awake()
    {
        base.Awake();

        playerControl = new PlayerControls();
        playerControl.Combat.CastSpell.performed += CastSelectedSpell;

        foreach (var spell in spells)
        {
            avaliableSpells.Add( new AvaliableSpells (spell));
        }
    }

    private void OnEnable()
    {
        playerControl.Enable();
    }
    private void OnDisable()
    {
        playerControl.Disable();
    }

    private void IncrementSpellIndex(Vector2 scroll)
    {
        currentSpellIndex = currentSpellIndex + Mathf.RoundToInt(scroll.y);

        if(currentSpellIndex >= spells.Count)
        {
            currentSpellIndex = 0;
        }

        if(currentSpellIndex < 0)
        {
            currentSpellIndex = spells.Count - 1;
        }

        OnSpellChange?.Invoke(this, EventArgs.Empty);
    }

    private void CastSelectedSpell(InputAction.CallbackContext obj)
    {
        avaliableSpells[currentSpellIndex].CastSpell();
    }

    private void Update()
    {
        Vector2 scroll = Mouse.current.scroll.ReadValue().normalized;

        if (scroll != Vector2.zero)
        {
            IncrementSpellIndex(scroll);
        }  

        float reduction = Time.deltaTime;
        foreach (var spell in avaliableSpells)
        {
            spell.Timer = Mathf.Clamp(spell.Timer - reduction, 0, Mathf.Infinity);
        }
    }

    public string GetSelectedSpellName() => 
        avaliableSpells[currentSpellIndex].GetSpellName();

    public float GetSelectedSpellTimer() => 
        avaliableSpells[currentSpellIndex].GetTimer();
}
