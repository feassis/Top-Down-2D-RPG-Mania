using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SpellTomeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI spellName;
    [SerializeField] private TextMeshProUGUI spellCd;

    private void Start()
    {
        RefreshUI();

        SpellTome.Instance.OnSpellChange += SpellTome_OnSpellChange;
    }

    private void SpellTome_OnSpellChange(object sender, EventArgs e)
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        spellName.text = SpellTome.Instance.GetSelectedSpellName();
        spellCd.text = SpellTome.Instance.GetSelectedSpellTimer().ToString();
    }

    private void Update()
    {
        spellCd.text = SpellTome.Instance.GetSelectedSpellTimer().ToString();
    }
}
