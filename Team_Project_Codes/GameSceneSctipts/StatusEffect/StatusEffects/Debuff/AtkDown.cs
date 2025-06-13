using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AtkDown : DebuffEffect
{
    public override DebuffType DebuffType => DebuffType.ATK_Down;

    public AtkDown(AtkDown other)
    {
        count = other.count;
    }
    public override void ActivateEffect(Character character)
    {
        count--;
        if (count <= 0)
        {
            character.StatusEffectHandler.ModifyBuffValues(atk: 0.25f);
        }
    }
    public override void AddEffect(StatusEffectBase effectBase, StatusEffectHandler statusEffectHandler)
    {
        if (effectBase is AtkDown atkDown)
        {
            AtkDown addEffect = new AtkDown(atkDown);
            AtkDown currentEffects = (AtkDown)statusEffectHandler.GetSpecificEffect(addEffect.DebuffType);
            if (currentEffects != null)
            {
                currentEffects.Count += addEffect.Count;
            }
            else
            {
                statusEffectHandler.ModifyBuffValues(atk: -0.25f);
                statusEffectHandler.RegisterNewEffect(addEffect);
            } 
        }
    }

    public override string StatusInfo()
    {
        return $"<color=#FFD500>{count}</color>턴 동안 <color=#FE332E>공격력</color>이 <color=#FFD500>25</color>% <color=#FE332E>감소</color>됩니다.";
    }
    public override string ShowGrantSuccessMessage()
    {
        return "공격력 <color=#FE332E>감소</color> <color=#FFB74D>부여</color>";
    }

    public override string ShowGrantFailureMessage()
    {
        return "공격력 <color=#FE332E>감소</color> <color=#B7C5CD>저항</color>";
    }
}
