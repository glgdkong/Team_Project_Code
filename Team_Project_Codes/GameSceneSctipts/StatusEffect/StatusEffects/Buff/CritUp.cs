using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CritUp : BuffEffect
{
    public override BuffType BuffType => BuffType.Crit_Up;

    public CritUp(CritUp other)
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
                character.StatusEffectHandler.ModifyBuffValues(crit: -0.25f);
            }
        }
    }
    public override void AddEffect(StatusEffectBase effectBase, StatusEffectHandler statusEffectHandler)
    {
        if (effectBase is CritUp atkUpEffect) // 특정한 버프 타입이라면
        {
            CritUp addEffect = new CritUp(atkUpEffect);
            CritUp currentEffect = (CritUp)statusEffectHandler.GetSpecificEffect(addEffect.BuffType);

            if (currentEffect != null)
            {
                currentEffect.Count += addEffect.Count;
            }
            else
            {
                statusEffectHandler.ModifyBuffValues(crit: 0.25f);
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
        return $"<color=#FFD500>{count}</color>턴 동안 <color=#FE332E>치명타</color> 확률이 <color=#FFD500>25</color>% <color=#5DADEC>증가</color>됩니다.";
    }
    public override string ShowGrantSuccessMessage()
    {
        return "치명타 확률 <color=#5DADEC>증가</color>";
    }

    public override string ShowGrantFailureMessage()
    {
        return "";
    }
}
