using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ResUp : BuffEffect
{
    [SerializeField] private List<StatusResistance> statusResistances = new List<StatusResistance>();
    public List<StatusResistance> StatusResistances => statusResistances; 
    public override BuffType BuffType => BuffType.Resistance_Up;

    public ResUp(ResUp other)
    {
        count = other.count;
        statusResistances = other.statusResistances;
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
        if(effectBase is ResUp resUp)
        {
            ResUp addEffect = new ResUp(resUp);
            statusEffectHandler.RegisterNewEffect(addEffect);
        }
    }
    public override string StatusInfo()
    {
        return null;
    }
    public override string ShowGrantSuccessMessage()
    {
        return "저항력 <color=#5DADEC>증가</color>";
    }

    public override string ShowGrantFailureMessage()
    {
        return "";
    }
}
