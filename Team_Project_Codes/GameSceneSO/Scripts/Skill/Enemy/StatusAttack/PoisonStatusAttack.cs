using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoisonStatusAttack", menuName = "SKill/Enemy/StatusAttackSkill/PoisonStatusAttack")]
public class PoisonStatusAttack : StatusAttackSkillSO
{
    [SerializeField] private Poison poison;
    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        if (targetCharacter.IsDeath || !hitSuccess) { return; }
        chance = Random.value;
        if (chance < CalcEffectChance(poison.StatusEffectType, targetCharacter, isCriticalHit))
        {
            poison.IsCriticalHit = isCriticalHit;
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(poison, true);
        }
        else
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(poison, false);
        }
    }
}
