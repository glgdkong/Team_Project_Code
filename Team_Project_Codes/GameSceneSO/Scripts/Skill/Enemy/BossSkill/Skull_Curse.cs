using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skull_Curse", menuName = "SKill/Enemy/BossSkill/AttackSkill/Skull_Curse")]
public class Skull_Curse : StatusAttackSkillSO
{
    [SerializeField] private Poison poison;
    [SerializeField] private Slow slow;

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
        chance = Random.value;
        if (chance < CalcEffectChance(slow.StatusEffectType, targetCharacter, isCriticalHit))
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(slow, true);
        }
        else
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(slow, false);
        }
    }
}

