using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Stun : StatusEffect
{
    public override StatusEffectType StatusEffectType => StatusEffectType.Stun;
    public override StatusType StatusType => StatusType.TurnSkip;

    public Stun(Stun other)
    {
        count = other.count;
    }
    
    public Stun(int count)
    {
        this.count = count;
    }


    public override void ActivateEffect(Character character)
    {
        count--;
    }
    public override void AddEffect(StatusEffectBase effectBase, StatusEffectHandler statusEffectHandler)
    {
        if (effectBase is Stun stun)
        {
            Stun addEffect = new Stun(stun);
            Stun currentEffects = (Stun)statusEffectHandler.GetSpecificEffect(addEffect.StatusEffectType);
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
        return $"<color=#F7CB00>기절</color>로 인해 <color=#FFD500>{count}</color>턴 동안 <color=#FE332E>행동 불가</color> 상태입니다.";
    }
    public override string ShowGrantSuccessMessage()
    {
        return "<color=#F7CB00>기절</color> <color=#FFB74D>부여</color>";
    }

    public override string ShowGrantFailureMessage()
    {
        return "<color=#F7CB00>기절</color> <color=#B7C5CD>저항</color>";
    }
}
