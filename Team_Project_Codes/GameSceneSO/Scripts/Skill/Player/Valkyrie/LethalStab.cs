using RPGCharacterAnims.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LethalStab", menuName = "SKill/Player/Valkyrie/AttackSkill/LethalStab")]
public class LethalStab : StatusAttackSkillSO
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
