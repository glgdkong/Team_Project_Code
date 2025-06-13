using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    ATK_Up,
    DEF_Up,
    Resistance_Up,
    Crit_Up,
    Dodge_Up
}

public abstract class BuffEffect : StatusEffectBase
{
    public override EffectCategory Category => EffectCategory.Buff;
    public abstract BuffType BuffType { get; }
}
