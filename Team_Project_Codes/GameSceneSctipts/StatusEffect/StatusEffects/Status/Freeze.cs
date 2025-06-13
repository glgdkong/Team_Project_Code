using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Freeze : StatusEffect
{
    public override StatusEffectType StatusEffectType => StatusEffectType.Freeze;
    public override StatusType StatusType => StatusType.TurnSkip;
    public Freeze(Freeze other)
    {
        count = other.count;
    }
    public override void ActivateEffect(Character character)
    {
        count--;
        if(count <= 0)
        {
            Slow slow = new Slow(1);
            character.StatusEffectHandler.UpdateOrAddEffect(slow, true);
        }
    }
    public override void AddEffect(StatusEffectBase effectBase, StatusEffectHandler statusEffectHandler)
    {
        if (effectBase is Freeze freeze)
        {
            Freeze addEffect = new Freeze(freeze);
            Freeze currentEffects = (Freeze)statusEffectHandler.GetSpecificEffect(addEffect.StatusEffectType);
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
        return $"<color=#2149EF>빙결</color>로 인해 <color=#FFD500>{count}</color>턴 동안 <color=#FE332E>행동 불가</color> 상태입니다.\n <color=#2149EF>빙결</color> 상태가 끝난 후, <color=#709AD1>둔화</color> 상태가 됩니다.";
    }
    public override string ShowGrantSuccessMessage()
    {
        return "<color=#2149EF>빙결</color> <color=#FFB74D>부여</color>";
    }

    public override string ShowGrantFailureMessage()
    {
        return "<color=#2149EF>빙결</color> <color=#B7C5CD>저항</color>";
    }
}
