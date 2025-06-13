using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Electrocute : StatusEffect
{
    
    public override StatusType StatusType => StatusType.TurnDealy;
    public Electrocute(Electrocute other)
    {
        count = other.count;
    }

    public override StatusEffectType StatusEffectType => StatusEffectType.Electrocute;
    
    public override void ActivateEffect(Character character)
    {
        count--;
    }
    public override void AddEffect(StatusEffectBase effectBase, StatusEffectHandler statusEffectHandler)
    {
        if (effectBase is Electrocute electrocute)
        {
            Electrocute addEffect = new Electrocute(electrocute);
            Electrocute currentEffects = (Electrocute)statusEffectHandler.GetSpecificEffect(addEffect.StatusEffectType);
            if (currentEffects != null)
            {
                currentEffects.Count += addEffect.Count;
                if (currentEffects.Count >= 2)
                {
                    Stun stun = new Stun(1);
                    statusEffectHandler.UpdateOrAddEffect(stun, true);
                    statusEffectHandler.RemoveStatus(currentEffects);
                }
            }
            else
            {
                statusEffectHandler.RegisterNewEffect(addEffect);
            }
        }
    }

    public override string StatusInfo()
    {
        return $"<color=#DEC308>감전</color> 상태로 인해 행동 <color=#FE332E>순서</color>가 <color=#FE332E>마지막</color>으로 미뤄졌습니다.";
    }
    public override string ShowGrantSuccessMessage()
    {
        return "<color=#DEC308>감전</color> <color=#FFB74D>부여</color>";
    }

    public override string ShowGrantFailureMessage()
    {
        return "<color=#DEC308>감전</color> <color=#B7C5CD>저항</color>";
    }
}
