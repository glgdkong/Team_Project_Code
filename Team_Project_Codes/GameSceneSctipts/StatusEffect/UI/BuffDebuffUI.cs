using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDebuffUI : StatusUI
{
    [SerializeField] protected Sprite[] weaknessEffectSprites;
    [SerializeField] protected Sprite[] buffEffectSprites;

    public override void SetEffectInfo(StatusEffectBase statusEffectBase)
    {
        if (statusEffectBase is BuffEffect buffEffect)
        {
            statusImage.sprite = buffEffectSprites[(int)buffEffect.BuffType];
        }
        if (statusEffectBase is StatusEffect statusEffect) 
        {
            DebuffEffect debuffEffect = (DebuffEffect)statusEffect;
            statusImage.sprite = weaknessEffectSprites[(int)debuffEffect.DebuffType];
        }
        this.statusEffect = statusEffectBase;

    }
}
