using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Taunt : DebuffEffect
{
    public override StatusEffectType StatusEffectType => StatusEffectType.Weakness;
    public override StatusType StatusType => StatusType.Debuff;
    public override DebuffType DebuffType => DebuffType.Taunt;

    public Taunt(Taunt other)
    {
        count = other.count;
        isCurrentTurnGranted = other.isCurrentTurnGranted;
    }
    public override void ActivateEffect(Character character)
    {
        if (isCurrentTurnGranted)
        {
            isCurrentTurnGranted = false;
        }
        else
        {
            count--;
        }
    }
    public override void AddEffect(StatusEffectBase effectBase, StatusEffectHandler statusEffectHandler)
    {
        if (effectBase is Taunt taunt)
        {
            Taunt addEffect = new Taunt(taunt);
            Taunt currentEffects = (Taunt)statusEffectHandler.GetSpecificEffect(addEffect.DebuffType);
            if (currentEffects != null)
            {
                currentEffects.Count += addEffect.Count;
            }
            else
            {
                statusEffectHandler.RegisterNewEffect(addEffect);
            }
        }
    }

    public override string StatusInfo()
    {
        return $"<color=#FFD500>{count}</color>턴 동안 <color=#FE332E>적</color>의 <color=#FE332E>공격</color>을 <color=#FF7F00>우선적</color>으로 받게 됩니다.";
    }
    public override string ShowGrantSuccessMessage()
    {
        return "<color=#FF7F00>도발</color> <color=#FFB74D>부여</color>";
    }

    public override string ShowGrantFailureMessage()
    {
        return "<color=#FF7F00>도발</color> <color=#B7C5CD>저항</color>";
    }
}
