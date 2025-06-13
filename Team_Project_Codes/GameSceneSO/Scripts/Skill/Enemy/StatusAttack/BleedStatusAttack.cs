using RPGCharacterAnims.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BleedStatusAttack", menuName = "SKill/Enemy/StatusAttackSkill/BleedStatusAttack")]
public class BleedStatusAttack : StatusAttackSkillSO
{
    [SerializeField] private Bleed bleed;
    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        if (targetCharacter.IsDeath || !hitSuccess) { return; }
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
