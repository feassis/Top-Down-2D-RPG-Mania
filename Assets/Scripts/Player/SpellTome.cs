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
        playerControl.Combat.ChangeSpell.performed += IncrementSpellIndex;

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

    private void IncrementSpellIndex(InputAction.CallbackContext obj)
    {
        currentSpellIndex++;

        if(currentSpellIndex >= spells.Count)
        {
            currentSpellIndex = 0;
        }

        OnSpellChange?.Invoke(this, EventArgs.Empty);
    }

    private void CastSelectedSpell(InputAction.CallbackContext obj)
    {
        avaliableSpells[currentSpellIndex].CastSpell();
    }

    private void Update()
    {
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
