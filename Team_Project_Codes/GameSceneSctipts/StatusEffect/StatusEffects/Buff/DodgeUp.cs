using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DodgeUp : BuffEffect
{
    public override BuffType BuffType => BuffType.Dodge_Up;

    public DodgeUp(DodgeUp other)
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
                character.StatusEffectHandler.ModifyBuffValues(evd: -0.25f);
            }
        }
    }
    public override void AddEffect(StatusEffectBase effectBase, StatusEffectHandler statusEffectHandler)
    {
        if (effectBase is DodgeUp atkUpEffect) // 특정한 버프 타입이라면
        {
            DodgeUp addEffect = new DodgeUp(atkUpEffect);
            DodgeUp currentEffect = (DodgeUp)statusEffectHandler.GetSpecificEffect(addEffect.BuffType);

            if (currentEffect != null)
            {
                currentEffect.Count += addEffect.Count;
            }
            else
            {
                statusEffectHandler.ModifyBuffValues(evd: 0.25f);
                statusEffectHandler.RegisterNewEffect(addEffect);
            }
        }
        else
        {
            Debug.LogError($"올바르지 않은 상태이상 타입: {effectBase.GetType()}");
        }
    }
    public override string StatusInfo()
    {
        return $"<color=#FFD500>{count}</color>턴 동안 <color=#7F7F7F>회피</color> 확률이 <color=#FFD500>25</color>% <color=#5DADEC>증가</color>됩니다.";
    }
    public override string ShowGrantSuccessMessage()
    {
        return "회피 확률 <color=#5DADEC>증가</color>";
    }

    public override string ShowGrantFailureMessage()
    {
        return "";
    }
}
