using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterUI : CharacterUI
{
    [SerializeField] protected Image manaBar;

    public virtual void UpdateMana(float amount)
    {
        manaBar.fillAmount = amount;
    }
}