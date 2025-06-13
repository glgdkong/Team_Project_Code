using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ElectrocuteStatusAttack", menuName = "SKill/Enemy/StatusAttackSkill/ElectrocuteStatusAttack")]
public class ElectrocuteStatusAttack : StatusAttackSkillSO
{
    [SerializeField] private Electrocute electrocute;
    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        if (targetCharacter.IsDeath || !hitSuccess) { return; }
        chance = Random.value;
        if (chance < CalcEffectChance(electrocute.StatusEffectType, targetCharacter, isCriticalHit))
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(electrocute, true);
        }
        else
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(electrocute, false);
        }
    }
}
