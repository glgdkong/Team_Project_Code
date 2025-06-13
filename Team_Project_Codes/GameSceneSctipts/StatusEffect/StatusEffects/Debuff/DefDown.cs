using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
[System.Serializable]
public class DefDown : DebuffEffect
{
    public override DebuffType DebuffType => DebuffType.DEF_Down;
    public DefDown(DefDown other) 
    {
        count = other.count;
    }
    public override void ActivateEffect(Character character)
    {
        count--;
        if (count <= 0) 
        {
            character.StatusEffectHandler.ModifyBuffValues(def: 25);
        }
    }
    public override void AddEffect(StatusEffectBase effectBase, StatusEffectHandler statusEffectHandler)
    {
        if (effectBase is DefDown defDown)
        {
            DefDown addEffect = new DefDown(defDown);
            DefDown currentEffects = (DefDown)statusEffectHandler.GetSpecificEffect(addEffect.DebuffType);
            if (currentEffects != null)
            {
                currentEffects.Count += addEffect.Count;
            }
            else
            {
                statusEffectHandler.ModifyBuffValues(def: -25);
                statusEffectHandler.RegisterNewEffect(addEffect);
            }
        }
    }
    public override string StatusInfo()
    {
        return $"<color=#FFD500>{count}</color>턴 동안 <color=#38B6ff>방어력</color>이 <color=#FFD500>25</color>% <color=#FE332E>감소</color>됩니다.";
    }
    public override string ShowGrantSuccessMessage()
    {
        return "방어력 <color=#FE332E>감소</color> <color=#FFB74D>부여</color>";
    }

    public override string ShowGrantFailureMessage()
    {
        return "방어력 <color=#FE332E>감소</color> <color=#B7C5CD>저항</color>";
    }
}
