using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BurnStatusAttack", menuName = "SKill/Enemy/StatusAttackSkill/BurnStatusAttack")]
public class BurnStatusAttack : StatusAttackSkillSO
{
    [SerializeField] private Burn burn;
    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        if (targetCharacter.IsDeath || !hitSuccess) { return; }
        chance = Random.value;
        if (chance < CalcEffectChance(burn.StatusEffectType, targetCharacter, isCriticalHit))
        {   
            burn.IsCriticalHit = isCriticalHit;
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(burn, true);
        }
        else
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(burn, false);
        }
    }
}
