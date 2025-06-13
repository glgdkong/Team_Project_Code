using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Final_Descent", menuName = "SKill/Enemy/BossSkill/AttackSkill/Final_Descent")]
public class Final_Descent : StatusAttackSkillSO
{
    [SerializeField] private Poison poison;
    [SerializeField] private Bleed bleed;
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
        if (chance < CalcEffectChance(bleed.StatusEffectType, targetCharacter, isCriticalHit))
        {
            bleed.IsCriticalHit = isCriticalHit;
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(bleed, true);
        }
        else
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(bleed, false);
        }
    }

}
