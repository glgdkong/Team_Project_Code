using RPGCharacterAnims.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoisonOrb", menuName = "SKill/Player/Wizard/AttackSkill/PoisonOrb")]
public class PoisonOrb : StatusAttackSkillSO
{
    [SerializeField, Header("중독 부여값")] private Poison poison;
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
