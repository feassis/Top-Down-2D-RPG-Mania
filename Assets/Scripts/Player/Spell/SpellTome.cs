using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellTome : Singleton<SpellTome>
{
    [SerializeField] private List<Spell> spells;
    [SerializeField] private int baseSpellLevel = 0;

    private List<SpellPowerUp> powerUps = new List<SpellPowerUp>();
    private PlayerControls playerControl;
    private int currentSpellIndex = 0;
    private List<AvaliableSpells> avaliableSpells = new List<AvaliableSpells>();

    public event EventHandler OnSpellChange;
    public event EventHandler<EventHandlerPowerUP> OnPowerUpRemoved;
    public event EventHandler<EventHandlerPowerUP> OnPowerUpAdded;

    public class EventHandlerPowerUP : EventArgs
    {
        public SpellType SpellType;
        public float Timer;
    }

    private class SpellPowerUp
    {
        public SpellType Spell;
        public int LevelUp;
        public float Timer;

        public SpellPowerUp(SpellType spellType, int level, float timer)
        {
            Spell = spellType;
            LevelUp = level;
            Timer = timer;
        }
    }

    public float GetSpellPowerUpTimer(SpellType spell)
    {
        var powerUp = powerUps.Find(p => p.Spell == spell);
        return powerUp.Timer;
    }

    private int GetSpellLevel(SpellType spell)
    {
        var powerUp = powerUps.Find(p => p.Spell == spell);

        if(powerUp != null)
        {
            return baseSpellLevel + powerUp.LevelUp;
        }

        return baseSpellLevel;
    }

    private class AvaliableSpells
    {
        public Spell Spell;
        public float Timer = 0;

        public AvaliableSpells (Spell spell)
        {
            Spell = spell;
        }

        public void CastSpell(int level)
        {
            if(Timer > 0)
            {
                return;
            }
            
            Spell.CastSpell(level);
            Timer = Spell.GetCoolDownTine(level);
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
        int level = GetSpellLevel(avaliableSpells[currentSpellIndex].Spell.SpellType);
        avaliableSpells[currentSpellIndex].CastSpell(level);
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

        List<SpellPowerUp> powerUpsToBeRemoved = new List<SpellPowerUp>();

        foreach (SpellPowerUp powerUp in powerUps)
        {
            powerUp.Timer = Mathf.Clamp(powerUp.Timer - reduction, 0, Mathf.Infinity);

            if(powerUp.Timer <= 0)
            {
                powerUpsToBeRemoved.Add(powerUp);
            }
        }

        foreach (var item in powerUpsToBeRemoved)
        {
            powerUps.Remove(item);
            OnPowerUpRemoved?.Invoke(this, new EventHandlerPowerUP { SpellType = item.Spell });
        }
    }

    public bool IsSelectedPowerUp()
    {
        var spell = avaliableSpells[currentSpellIndex];

        var powerUp = powerUps.Find(p => p.Spell == spell.Spell.SpellType);

        if (powerUp != null)
        {
            return true;
        }

        return false;
    }

    public string GetSelectedSpellName() => 
        avaliableSpells[currentSpellIndex].GetSpellName();

    public float GetSelectedSpellTimer() => 
        avaliableSpells[currentSpellIndex].GetTimer();

    public void AddPowerUp(SpellType spellType, int level, float powerDuration)
    {
        var powerUp = powerUps.Find(p => p.Spell == spellType);

        if(powerUp != null)
        {
            powerUp.LevelUp = Mathf.Max(powerUp.LevelUp, level);
            powerUp.Timer = Mathf.Max(powerUp.Timer, powerDuration);
        }
        else
        {
            powerUps.Add(new SpellPowerUp(spellType, level, powerDuration));
        }

        OnPowerUpAdded?.Invoke(this, new EventHandlerPowerUP { SpellType = spellType, Timer = powerDuration });
    }
}
