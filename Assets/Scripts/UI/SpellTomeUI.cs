using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class SpellTomeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI spellName;
    [SerializeField] private TextMeshProUGUI spellCd;
    [SerializeField] private List<PowerUpUiPrefab> powerUpPrefabs;
    [SerializeField] private Transform powerUpHolder;

    private List<PowerUpUI> powerUpUiList = new List<PowerUpUI>();

    [Serializable]
    private struct PowerUpUiPrefab
    {
        public SpellType Spell;
        public Image PowerUpImage;
    }

    private class PowerUpUI
    {
        public SpellType Spell;
        public Image PowerUpImage;
        public float InitialTimer;
        public float Timer;

        public PowerUpUI(SpellType spell, Image powerUpImage, float initialTimer, float timer)
        {
            Spell = spell;
            PowerUpImage = powerUpImage;
            InitialTimer = initialTimer;
            Timer = timer;
        }
    }

    private void Start()
    {
        RefreshUI();

        SpellTome.Instance.OnSpellChange += SpellTome_OnSpellChange;
        SpellTome.Instance.OnPowerUpAdded += SpellTome_OnPowerUpAdded;
        SpellTome.Instance.OnPowerUpRemoved += SpellTome_OnPowerUpRemoved;
    }

    private void SpellTome_OnPowerUpRemoved(object sender, SpellTome.EventHandlerPowerUP e)
    {
        var powerUi = powerUpUiList.Find(p => p.Spell == e.SpellType);
        powerUpUiList.Remove(powerUi);
        RefreshUI();
    }

    private void SpellTome_OnPowerUpAdded(object sender, SpellTome.EventHandlerPowerUP e)
    {
        var powerUi = powerUpUiList.Find(p => p.Spell == e.SpellType);

        if(powerUi != null)
        {
            powerUi.InitialTimer = Mathf.Max(powerUi.InitialTimer, e.Timer);
            powerUi.Timer = e.Timer;
        }
        else
        {
            Image powerUIPrefab = powerUpPrefabs.Find(p => p.Spell == e.SpellType).PowerUpImage;
            Image powerUpUI = Instantiate<Image>(powerUIPrefab, powerUpHolder);
            powerUpUiList.Add(new PowerUpUI(e.SpellType, powerUpUI, e.Timer, e.Timer));
        }
        RefreshUI();
    }

    private void SpellTome_OnSpellChange(object sender, EventArgs e)
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        string powerUpSign = "";

        if (SpellTome.Instance.IsSelectedPowerUp())
        {
            powerUpSign = " + ";
        }

        spellName.text = SpellTome.Instance.GetSelectedSpellName() + powerUpSign;
        spellCd.text = SpellTome.Instance.GetSelectedSpellTimer().ToString();
    }

    private void Update()
    {
        spellCd.text = SpellTome.Instance.GetSelectedSpellTimer().ToString();

        foreach (var powerUp in powerUpUiList)
        {
            powerUp.Timer = SpellTome.Instance.GetSpellPowerUpTimer(powerUp.Spell);
            powerUp.PowerUpImage.fillAmount = powerUp.Timer / powerUp.InitialTimer;
        }
    }
}
