using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum StatusType
{
    TurnDealy,
    TurnSkip,
    DotDamage,
    Debuff,
    Move
}

public abstract class StatusEffect : StatusEffectBase
{
    public override EffectCategory Category => EffectCategory.Status;
    public abstract StatusEffectType StatusEffectType { get; }
    public abstract StatusType StatusType { get; }
}
