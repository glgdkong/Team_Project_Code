using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class StatusEffectUI : StatusUI
{
    [SerializeField] protected Sprite[] statusEffectSprites;

    public override void SetEffectInfo(StatusEffectBase statusEffectBase)
    {
        if (statusEffectBase is StatusEffect statusEffect) 
        {
            statusImage.sprite = statusEffectSprites[(int)statusEffect.StatusEffectType];
        }
        this.statusEffect = statusEffectBase;
    }
}
