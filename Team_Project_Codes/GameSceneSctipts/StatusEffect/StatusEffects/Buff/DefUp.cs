using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DefUp : BuffEffect
{
    public override BuffType BuffType => BuffType.DEF_Up;

    public DefUp(DefUp other) 
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
            if (count <= 0)
            {
                character.StatusEffectHandler.ModifyBuffValues(def: -25);
            }
        }

    }
    public override void AddEffect(StatusEffectBase effectBase, StatusEffectHandler statusEffectHandler)
    {
        if(effectBase is DefUp defup)
        {
            DefUp addEffect = new DefUp(defup);
            DefUp currentEffects = (DefUp)statusEffectHandler.GetSpecificEffect(addEffect.BuffType);
            if (currentEffects != null)
            {
                currentEffects.Count += addEffect.Count;
            }
            else
            {
                statusEffectHandler.ModifyBuffValues(def: 25);
                statusEffectHandler.RegisterNewEffect(addEffect);
            }
        }
    }
    public override string StatusInfo()
    {
        return $"<color=#FFD500>{count}</color>턴 동안 <color=#38B6ff>방어력</color>이 <color=#FFD500>25</color>% <color=#5DADEC>증가</color>됩니다.";
    }
    public override string ShowGrantSuccessMessage()
    {
        return "방어력 <color=#5DADEC>증가</color>";
    }

    public override string ShowGrantFailureMessage()
    {
        return "";
    }
}
