using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DebuffType
{
    Taunt,
    Slow,
    ATK_Down,
    DEF_Down
}
[System.Serializable]
public abstract class DebuffEffect : StatusEffect
{
    public override EffectCategory Category => EffectCategory.Debuff;
    public override StatusEffectType StatusEffectType => StatusEffectType.Weakness;
    public override StatusType StatusType => StatusType.Debuff;

    public abstract DebuffType DebuffType { get; }
}
